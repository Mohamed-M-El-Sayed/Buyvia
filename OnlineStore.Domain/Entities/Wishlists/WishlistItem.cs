using OnlineStore.Domain.Entities.BaseEntities;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Domain.Entities.Wishlists
{
    public class WishlistItem : BaseEntity
    {
        public int WishlistId { get; set; }
        public Wishlist Wishlist { get; set; } = default!;
        public int ProductVariantId { get; set; }
        public ProductVariant ProductVariant { get; set; } = default!;
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    }
}
