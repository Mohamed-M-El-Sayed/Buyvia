using OnlineStore.Application.Common.Specifications;
using OnlineStore.Domain.Entities.Wishlists;

namespace OnlineStore.Application.Features.Wishlists.Specifications
{
    public class WishlistDetailsSpecification : BaseSpecification<Wishlist>
    {
        public WishlistDetailsSpecification(Guid userId, bool asNoTracking = true)
        {
            Criteria = wishlist => wishlist.UserId == userId;
            ApplyInclude(wishlist => wishlist.Items);
            ApplyInclude("Items.ProductVariant");
            ApplyInclude("Items.ProductVariant.Product");
            ApplyInclude("Items.ProductVariant.Options.Value");
            ApplyInclude("Items.ProductVariant.Discount");
            ApplyInclude("Items.ProductVariant.Images");
            if (asNoTracking)
                AsNoTracking();
        }
    }

}
