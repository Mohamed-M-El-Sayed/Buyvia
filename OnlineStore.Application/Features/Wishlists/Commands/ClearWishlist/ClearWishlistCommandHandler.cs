using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Features.Wishlists.Specifications;
using OnlineStore.Domain.Entities.Wishlists;

namespace OnlineStore.Application.Features.Wishlists.Commands.ClearWishlist
{
    public class ClearWishlistCommandHandler(IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        ILogger<ClearWishlistCommandHandler> logger) : IRequestHandler<ClearWishlistCommand, Unit>
    {
        public async Task<Unit> Handle(ClearWishlistCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId
                ?? throw new UnauthorizedException("User must be authenticated.");

            logger.LogInformation(
                "Clearing wishlist for user {UserId}.",
                userId);

            var wishlist = await unitOfWork.Repository<Wishlist>()
                .GetEntityWithSpecAsync(new WishlistWithItemsSpecification(userId), cancellationToken)
                           ?? throw new NotFoundException($"Wishlist for user {userId} was not found.");
            wishlist.Items.Clear();
            await unitOfWork.CompleteAsync();
            logger.LogInformation(
                "Wishlist cleared for user {UserId}.",
                userId);
            return Unit.Value;
        }
    }
}
