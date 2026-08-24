using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities.Orders;

namespace OnlineStore.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasIndex(o => o.UserId);
            builder.Property(o => o.Subtotal)
               .HasColumnType("decimal(18,2)");

            builder.Property(o => o.ItemsDiscount)
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.CouponDiscount)
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.DeliveryFee)
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.Total)
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.Status)
                .HasConversion<string>();

            builder.OwnsOne(o => o.ShippingAddress);

            builder.HasOne(o => o.DeliveryMethod)
                .WithMany()
                .HasForeignKey(o => o.DeliveryMethodId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(o => o.Items)
               .WithOne(i => i.Order)
               .HasForeignKey(i => i.OrderId)
               .OnDelete(DeleteBehavior.Cascade);

            //builder.HasOne<ApplicationUser>()
            //   .WithMany(o => o.Orders)
            //   .HasForeignKey(o => o.UserId)
            //   .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
