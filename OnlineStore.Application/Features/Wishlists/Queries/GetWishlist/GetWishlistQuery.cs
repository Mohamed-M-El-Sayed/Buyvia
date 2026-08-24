using MediatR;
using OnlineStore.Application.Features.Wishlists.Dtos;

namespace OnlineStore.Application.Features.Wishlists.Queries.GetWishlist
{
    public class GetWishlistQuery : IRequest<WishlistDto>
    {
    }
}
