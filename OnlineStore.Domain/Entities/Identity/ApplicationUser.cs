using Microsoft.AspNetCore.Identity;
using OnlineStore.Domain.Entities.Orders;
using OnlineStore.Domain.Entities.ShoppingCart;
using OnlineStore.Domain.Entities.Wishlists;
namespace OnlineStore.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? ProfilePictureUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public ICollection<UserAddress> Addresses { get; set; } = new List<UserAddress>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public Cart? Cart { get; set; }
        public Wishlist? Wishlist { get; set; }

    }
}
