using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Contracts.Services.BackgroundJobs;
using OnlineStore.Application.Features.Carts.Specifications;
using OnlineStore.Domain.Entities.Identity;
using OnlineStore.Domain.Entities.Orders;
using OnlineStore.Domain.Entities.ShoppingCart;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Orders.Commands.PlaceOrder
{
    public class PlaceOrderCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<PlaceOrderCommandHandler> logger,
        ICurrentUserService currentUserService,
        IBackgroundJobService backgroundJobService)
        : IRequestHandler<PlaceOrderCommand, int>
    {
        public async Task<int> Handle(
            PlaceOrderCommand request,
            CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId
                ?? throw new UnauthorizedException("User must be authenticated.");

            logger.LogInformation(
                "Placing order for user {UserId}",
                userId);

            await unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                const int expirationMinutes = 30;

                var cart = await unitOfWork.Repository<Cart>()
                    .GetEntityWithSpecAsync(
                        new CartForCheckoutSpecification(userId),
                        cancellationToken)
                    ?? throw new NotFoundException(
                        nameof(Cart),
                        userId.ToString());

                if (!cart.Items.Any())
                    throw new BadRequestException("Cart is empty.");

                var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>()
                    .GetByIdAsync(request.DeliveryMethodId)
                    ?? throw new NotFoundException(
                        nameof(DeliveryMethod),
                        request.DeliveryMethodId.ToString());

                if (!deliveryMethod.IsActive)
                    throw new BadRequestException(
                        "Delivery method is inactive.");

                var address = await unitOfWork.Repository<UserAddress>()
                    .FindAsync(
                        a => a.Id == request.AddressId &&
                             a.UserId == userId,
                        cancellationToken)
                    ?? throw new NotFoundException(
                        nameof(UserAddress),
                        request.AddressId.ToString());

                if (!address.IsPhoneVerified)
                {
                    throw new BadRequestException(
                        "The selected address phone number is not verified.");
                }

                decimal subtotal = 0m;
                decimal itemsDiscount = 0m;

                var orderItems = new List<OrderItem>();
                var affectedVariantIds = new List<int>();

                foreach (var item in cart.Items)
                {
                    var variant = item.ProductVariant;

                    if (variant.Stock < item.Quantity)
                    {
                        throw new BadRequestException(
                            $"Not enough stock for '{variant.Product.Name} - {variant.GetVariantName()}'.");
                    }

                    var unitPrice = variant.Price;

                    var discountAmount =
                        variant.Discount?.CalculateDiscount(unitPrice) ?? 0m;

                    subtotal += unitPrice * item.Quantity;
                    itemsDiscount += discountAmount * item.Quantity;

                    orderItems.Add(new OrderItem
                    {
                        ProductId = variant.ProductId,
                        ProductVariantId = variant.Id,
                        ProductName = variant.Product.Name,
                        VariantName = variant.GetVariantName(),
                        ImageUrl = variant.Images
                            .FirstOrDefault(i => i.IsMainImage)?.ImageUrl
                            ?? "default.png",
                        Quantity = item.Quantity,
                        UnitPrice = unitPrice,
                        UnitDiscountAmount = discountAmount
                    });

                    variant.Stock -= item.Quantity;

                    // Check low stock after decreasing the stock.
                    if (variant.Stock <= variant.StockThreshold &&
                        variant.LowStockAlertedAt == null)
                    {
                        affectedVariantIds.Add(variant.Id);
                    }
                }

                var amountAfterItemsDiscount =
                    subtotal - itemsDiscount;

                decimal couponDiscount = 0m;

                if (cart.Coupon is not null)
                {
                    if (!cart.Coupon.IsValid(amountAfterItemsDiscount))
                        throw new BadRequestException(
                            "Coupon is no longer valid.");

                    couponDiscount =
                        cart.Coupon.CalculateDiscount(
                            amountAfterItemsDiscount);

                    cart.Coupon.UsedCount++;
                }

                var deliveryFee = deliveryMethod.Price;

                var total =
                    amountAfterItemsDiscount
                    - couponDiscount
                    + deliveryFee;

                var order = new Order
                {
                    UserId = userId,
                    Status = OrderStatus.Pending,
                    ExpireAt =
                        DateTime.UtcNow.AddMinutes(expirationMinutes),

                    ShippingAddress = new OrderAddress
                    {
                        City = address.City,
                        Country = address.Country,
                        FirstName = address.FirstName,
                        LastName = address.LastName,
                        PhoneNumber = address.PhoneNumber,
                        Street = address.Street
                    },

                    DeliveryMethodId = deliveryMethod.Id,
                    Subtotal = subtotal,
                    ItemsDiscount = itemsDiscount,
                    CouponCode = cart.Coupon?.Code,
                    CouponId = cart.Coupon?.Id,
                    CouponDiscount = couponDiscount,
                    DeliveryFee = deliveryFee,
                    Total = total,
                    Items = orderItems,

                    Payment = new Payment
                    {
                        Method = request.PaymentMethod,
                        Amount = total,
                        Status = PaymentStatus.Pending
                    }
                };

                await unitOfWork.Repository<Order>()
                    .AddAsync(order, cancellationToken);

                cart.Items.Clear();
                cart.Coupon = null;
                cart.CouponId = null;

                await unitOfWork.CompleteAsync(cancellationToken);

                await unitOfWork.CommitTransactionAsync(cancellationToken);

                // Enqueue low stock check only after the order transaction is committed.
                if (affectedVariantIds.Any())
                {
                    backgroundJobService.Enqueue<ILowStockCheckJob>(
                        job => job.ExecuteAsync(affectedVariantIds));
                }

                logger.LogInformation(
                    "Order {OrderId} placed successfully for user {UserId}",
                    order.Id,
                    userId);

                return order.Id;
            }
            catch
            {
                await unitOfWork.RollbackTransactionAsync(
                    cancellationToken);

                throw;
            }
        }
    }
}