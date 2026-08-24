using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Carts.Specifications;
using OnlineStore.Domain.Entities.Products;
using OnlineStore.Domain.Entities.ShoppingCart;

namespace OnlineStore.Application.Features.Carts.Commands.UpdateCartItem
{
    public class UpdateCartItemQuantityCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        ILogger<UpdateCartItemQuantityCommandHandler> logger)
        : IRequestHandler<UpdateCartItemCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId
                ?? throw new UnauthorizedException("User must be authenticated.");

            logger.LogInformation(
                "Updating quantity of variant {VariantId} to {Quantity} for user {UserId}.",
                request.ProductVariantId, request.Quantity, userId);

            var cart = await unitOfWork.Repository<Cart>()
                .GetEntityWithSpecAsync(new CartWithItemsSpecification(userId), cancellationToken)
                ?? throw new NotFoundException("Cart", userId.ToString());

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductVariantId == request.ProductVariantId)
                ?? throw new NotFoundException("Cart item", request.ProductVariantId.ToString());

            var variant = await unitOfWork.Repository<ProductVariant>()
                .GetByIdAsync(request.ProductVariantId)
                ?? throw new NotFoundException(nameof(ProductVariant), request.ProductVariantId.ToString());

            if (request.Quantity > variant.Stock)
                throw new BadRequestException($"Only {variant.Stock} units available.");

            cartItem.Quantity = request.Quantity;

            unitOfWork.Repository<Cart>().Update(cart);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Unit.Value;
        }
    }
}