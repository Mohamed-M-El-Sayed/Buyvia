using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities.ShoppingCart;
using OnlineStore.Domain.Entities.Identity;

namespace OnlineStore.Infrastructure.Persistence.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {

            builder.HasOne<ApplicationUser>()
                .WithOne()
                .HasForeignKey<Cart>(c => c.UserId);

            builder.HasIndex(c => c.UserId)
                .IsUnique();

            builder.HasMany(c => c.Items)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(c => c.User)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}