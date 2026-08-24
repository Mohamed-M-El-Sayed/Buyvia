using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Products;
using OnlineStore.Domain.Entities.Wishlists;

namespace OnlineStore.Application.Features.Wishlists.Commands.AddToWishlist
{
    public class AddWishlistItemCommandHandler(IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        ILogger<AddWishlistItemCommandHandler> logger) : IRequestHandler<AddWishlistItemCommand, Unit>
    {
        public async Task<Unit> Handle(AddWishlistItemCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId
                ?? throw new UnauthorizedException("User must be authenticated.");


            logger.LogInformation(
                "Adding product variant {ProductVariantId} to wishlist for user {UserId}.",
                request.ProductVariantId,
                userId);

            var variantExists = await unitOfWork.Repository<ProductVariant>()
                .AnyAsync(v => v.Id == request.ProductVariantId, cancellationToken);

            if (!variantExists)
                throw new NotFoundException(
                    nameof(ProductVariant),
                    request.ProductVariantId.ToString());

            var wishlist = await unitOfWork.Repository<Wishlist>()
                .FindAsync(w => w.UserId == userId, cancellationToken);

            if (wishlist is null)
            {
                wishlist = new Wishlist
                {
                    UserId = userId
                };

                await unitOfWork.Repository<Wishlist>()
                    .AddAsync(wishlist, cancellationToken);

                // Save to generate Wishlist.Id
                await unitOfWork.CompleteAsync(cancellationToken);
            }

            var alreadyExists = await unitOfWork.Repository<WishlistItem>()
                .AnyAsync(
                    x => x.WishlistId == wishlist.Id &&
                         x.ProductVariantId == request.ProductVariantId,
                    cancellationToken);

            if (alreadyExists)
                throw new BadRequestException("Item already exists in wishlist.");

            var item = new WishlistItem
            {
                WishlistId = wishlist.Id,
                ProductVariantId = request.ProductVariantId
            };

            await unitOfWork.Repository<WishlistItem>()
                .AddAsync(item, cancellationToken);

            await unitOfWork.CompleteAsync(cancellationToken);

            logger.LogInformation(
                "Product variant {ProductVariantId} added to wishlist for user {UserId}.",
                request.ProductVariantId,
                userId);

            return Unit.Value;
        }
    }
}
