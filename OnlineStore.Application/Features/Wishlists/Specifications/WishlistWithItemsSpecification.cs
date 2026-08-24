using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Wishlists;

namespace OnlineStore.Application.Features.Wishlists.Specifications
{
    public class WishlistWithItemsSpecification : BaseSpecification<Wishlist>
    {
        public WishlistWithItemsSpecification(Guid userId)
        {
            Criteria = w => w.UserId == userId;
            ApplyInclude(w => w.Items);
        }
    }
}
