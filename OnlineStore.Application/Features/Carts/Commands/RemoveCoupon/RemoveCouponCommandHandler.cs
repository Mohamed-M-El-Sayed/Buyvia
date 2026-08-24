using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Carts.Specifications;
using OnlineStore.Domain.Entities.ShoppingCart;

namespace OnlineStore.Application.Features.Carts.Commands.RemoveCoupon
{
    public class RemoveCouponCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        ILogger<RemoveCouponCommandHandler> logger) : IRequestHandler<RemoveCouponCommand, Unit>
    {
        public async Task<Unit> Handle(RemoveCouponCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId
                ?? throw new UnauthorizedException("User must be authenticated.");

            logger.LogInformation("Removing coupon from cart of user {UserId}.", userId);

            var cart = await unitOfWork.Repository<Cart>()
                .GetEntityWithSpecAsync(new CartWithItemsSpecification(userId), cancellationToken)
                ?? throw new NotFoundException($"Cart for user '{userId}' was not found.");

            cart.CouponId = null;
            unitOfWork.Repository<Cart>().Update(cart);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Unit.Value;
        }
    }
}