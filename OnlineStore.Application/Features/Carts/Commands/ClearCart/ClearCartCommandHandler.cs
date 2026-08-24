using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Carts.Specifications;
using OnlineStore.Domain.Entities.ShoppingCart;

namespace OnlineStore.Application.Features.Carts.Commands.ClearCart
{
    public class ClearCartCommandHandler(ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork,
        ILogger<ClearCartCommandHandler> logger) : IRequestHandler<ClearCartCommand, Unit>
    {
        public async Task<Unit> Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId
                ?? throw new UnauthorizedException("User is not authenticated");

            logger.LogInformation("Clearing cart for user {UserId}", userId);
            var cart = await unitOfWork.Repository<Cart>()
                .GetEntityWithSpecAsync(new CartWithItemsSpecification(userId), cancellationToken)
                ?? throw new NotFoundException($"Cart not found for user {userId}");
            if (!cart.Items.Any())
            {
                logger.LogInformation("Cart already empty for user {UserId}", userId);
                return Unit.Value;
            }
            cart.CouponId = null;
            cart.Coupon = null;
            cart.Items.Clear();
            await unitOfWork.CompleteAsync(cancellationToken);
            return Unit.Value;
        }
    }

}
