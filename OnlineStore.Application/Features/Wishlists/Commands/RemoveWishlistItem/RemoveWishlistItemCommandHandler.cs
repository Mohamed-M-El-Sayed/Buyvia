using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Wishlists;

namespace OnlineStore.Application.Features.Wishlists.Commands.RemoveWishlistItem
{
    public class RemoveWishlistItemCommandHandler(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        ILogger<RemoveWishlistItemCommandHandler> logger)
        : IRequestHandler<RemoveWishlistItemCommand, Unit>
    {
        public async Task<Unit> Handle(
            RemoveWishlistItemCommand request,
            CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId
                ?? throw new UnauthorizedException("User must be authenticated.");

            logger.LogInformation(
                "Removing product variant {ProductVariantId} from wishlist for user {UserId}.",
                request.ProductVariantId,
                userId);

            var wishlist = await unitOfWork.Repository<Wishlist>()
                .FindAsync(
                    w => w.UserId == userId,
                    cancellationToken);

            if (wishlist is null)
                return Unit.Value;

            var wishlistItem = await unitOfWork.Repository<WishlistItem>()
                .FindAsync(
                    i => i.WishlistId == wishlist.Id &&
                          i.ProductVariantId == request.ProductVariantId,
                    cancellationToken);

            if (wishlistItem is null)
                return Unit.Value;

            unitOfWork.Repository<WishlistItem>().Delete(wishlistItem);

            await unitOfWork.CompleteAsync(cancellationToken);
            logger.LogInformation(
                "Product variant {ProductVariantId} removed from wishlist for user {UserId}.",
                request.ProductVariantId,
                userId);
            return Unit.Value;
        }
    }
}