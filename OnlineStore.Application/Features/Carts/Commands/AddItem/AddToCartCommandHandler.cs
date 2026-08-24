using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Carts.Specifications;
using OnlineStore.Domain.Entities.Products;
using OnlineStore.Domain.Entities.ShoppingCart;

namespace OnlineStore.Application.Features.Carts.Commands.AddItem
{
    public class AddToCartCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        ILogger<AddToCartCommandHandler> logger) : IRequestHandler<AddToCartCommand, Unit>
    {
        public async Task<Unit> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId
                ?? throw new UnauthorizedException("User must be authenticated.");

            logger.LogInformation("User {UserId} adding variant {VariantId} x{Quantity} to cart.",
                userId, request.VariantId, request.Quantity);

            var cart = await unitOfWork.Repository<Cart>()
                .GetEntityWithSpecAsync(new CartWithItemsSpecification(userId), cancellationToken)
                ?? new Cart { UserId = userId };

            var variant = await unitOfWork.Repository<ProductVariant>()
                .GetByIdAsync(request.VariantId)
                ?? throw new NotFoundException("Product variant", request.VariantId.ToString());

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductVariantId == request.VariantId);
            var requestedQuantity = (existingItem?.Quantity ?? 0) + request.Quantity;

            if (requestedQuantity > variant.Stock)
                throw new BadRequestException($"Only {variant.Stock} units available in stock.");

            if (existingItem is not null)
            {
                existingItem.Quantity = requestedQuantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductVariantId = request.VariantId,
                    Quantity = request.Quantity,
                    UnitPrice = variant.Price
                });
            }

            if (cart.Id == 0)
                await unitOfWork.Repository<Cart>().AddAsync(cart, cancellationToken);
            else
                unitOfWork.Repository<Cart>().Update(cart);

            await unitOfWork.CompleteAsync(cancellationToken);

            return Unit.Value;
        }
    }
}