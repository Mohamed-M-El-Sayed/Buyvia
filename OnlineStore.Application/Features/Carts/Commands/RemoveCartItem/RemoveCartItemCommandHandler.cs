using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Carts.Specifications;
using OnlineStore.Domain.Entities.ShoppingCart;

namespace OnlineStore.Application.Features.Carts.Commands.RemoveCartItem
{
    public class RemoveCartItemCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<RemoveCartItemCommandHandler> logger,
        ICurrentUserService currentUserService) : IRequestHandler<RemoveCartItemCommand, Unit>
    {
        public async Task<Unit> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId
                ?? throw new UnauthorizedException("User is not authenticated");

            logger.LogInformation(
                "Removing variant {VariantId} from cart for user {UserId}.",
                request.ProductVariantId, userId);

            var cart = await unitOfWork.Repository<Cart>()
                .GetEntityWithSpecAsync(new CartWithItemsSpecification(userId), cancellationToken)
                ?? throw new NotFoundException($"Cart not found for user {userId}");

            var item = cart.Items.FirstOrDefault(i => i.ProductVariantId == request.ProductVariantId)
                ?? throw new NotFoundException($"Cart item not found for product variant {request.ProductVariantId}");

            cart.Items.Remove(item);

            await unitOfWork.CompleteAsync(cancellationToken);

            return Unit.Value;
        }
    }
}