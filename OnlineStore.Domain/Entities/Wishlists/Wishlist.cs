using OnlineStore.Domain.Entities.BaseEntities;
using OnlineStore.Domain.Entities.Identity;

namespace OnlineStore.Domain.Entities.Wishlists
{
    public class Wishlist : BaseEntity
    {
        public Guid UserId { get; set; }
        public ICollection<WishlistItem> Items { get; set; } = new List<WishlistItem>();
        public ApplicationUser User { get; set; } = default!;
    }
}
