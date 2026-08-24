using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Carts.Specifications;
using OnlineStore.Domain.Entities.Promotions;
using OnlineStore.Domain.Entities.ShoppingCart;

namespace OnlineStore.Application.Features.Carts.Commands.ApplyCoupon
{
    public class ApplyCouponCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        ILogger<ApplyCouponCommandHandler> logger) : IRequestHandler<ApplyCouponCommand, Unit>
    {
        public async Task<Unit> Handle(ApplyCouponCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId
                ?? throw new UnauthorizedException("User must be authenticated.");

            logger.LogInformation(
                "Applying coupon {CouponCode} to cart of user {UserId}.",
                request.CouponCode, userId);

            var coupon = await unitOfWork.Repository<Coupon>()
                .FindAsync(c => c.Code == request.CouponCode, cancellationToken)
                ?? throw new NotFoundException(nameof(Coupon), request.CouponCode);

            var now = DateTime.UtcNow;

            if (!coupon.IsActive)
                throw new BadRequestException("Coupon is not active.");

            if (coupon.StartsAt.HasValue && coupon.StartsAt.Value > now)
                throw new BadRequestException("Coupon has not started yet.");

            if (coupon.ExpiresAt.HasValue && coupon.ExpiresAt.Value < now)
                throw new BadRequestException("Coupon has expired.");

            if (coupon.UsageLimit.HasValue && coupon.UsedCount >= coupon.UsageLimit.Value)
                throw new BadRequestException("Coupon usage limit has been reached.");

            var cart = await unitOfWork.Repository<Cart>()
                .GetEntityWithSpecAsync(new CartWithCouponPricingSpecification(userId), cancellationToken)
                ?? throw new NotFoundException($"Cart for user '{userId}' was not found.");

            if (!cart.Items.Any())
                throw new BadRequestException("Cannot apply a coupon to an empty cart.");

            var subTotal = cart.Items.Sum(i => i.UnitPrice * i.Quantity);
            var itemsDiscount = cart.Items.Sum(i =>
                (i.ProductVariant.Discount?.CalculateDiscount(i.UnitPrice) ?? 0m) * i.Quantity);
            var amountAfterItemDiscount = subTotal - itemsDiscount;

            if (amountAfterItemDiscount < coupon.MinOrderAmount)
                throw new BadRequestException($"Minimum order amount is {coupon.MinOrderAmount}.");

            cart.CouponId = coupon.Id;
            unitOfWork.Repository<Cart>().Update(cart);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Unit.Value;
        }
    }
}