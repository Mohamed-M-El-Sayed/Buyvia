using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities.Identity;
using OnlineStore.Domain.Entities.Wishlists;

namespace OnlineStore.Infrastructure.Persistence.Configurations
{
    public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
    {
        public void Configure(EntityTypeBuilder<Wishlist> builder)
        {
            builder.HasIndex(w => w.UserId)
                .IsUnique();

            builder.HasOne<ApplicationUser>()
                .WithOne()
                .HasForeignKey<Wishlist>(w => w.UserId);

            builder.HasMany(w => w.Items)
                .WithOne(i => i.Wishlist)
                .HasForeignKey(i => i.WishlistId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(w => w.User)
                .WithOne(u => u.Wishlist)
                .HasForeignKey<Wishlist>(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
