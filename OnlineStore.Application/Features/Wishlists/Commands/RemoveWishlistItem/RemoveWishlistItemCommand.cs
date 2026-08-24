using MediatR;

namespace OnlineStore.Application.Features.Wishlists.Commands.RemoveWishlistItem
{
    public class RemoveWishlistItemCommand(int productVariantId) : IRequest<Unit>
    {
        public int ProductVariantId { get; } = productVariantId;
    }
}
