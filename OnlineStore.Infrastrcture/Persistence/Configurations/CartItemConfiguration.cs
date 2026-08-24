using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities.ShoppingCart;

namespace OnlineStore.Infrastructure.Persistence.Configurations
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Quantity);

            builder.Property(ci => ci.UnitPrice)
                .HasColumnType("decimal(18,2)");

            //builder.Property(ci => ci.Subtotal)
            //    .HasColumnType("decimal(18,2)");

            builder.HasOne(ci => ci.ProductVariant)
                .WithMany()
                .HasForeignKey(ci => ci.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("CartItems");
        }
    }
}