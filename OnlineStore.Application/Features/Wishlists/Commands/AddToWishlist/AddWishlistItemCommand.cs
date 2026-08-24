using MediatR;

namespace OnlineStore.Application.Features.Wishlists.Commands.AddToWishlist
{
    public class AddWishlistItemCommand : IRequest<Unit>
    {
        public int ProductVariantId { get; set; }
    }


}
