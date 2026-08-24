using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities.Promotions;

namespace OnlineStore.Infrastructure.Persistence.Configurations
{
    public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {

            builder.HasIndex(c => c.Code)
                .IsUnique();

            builder.Property(c => c.Type)
                .HasConversion<string>();

            builder.Property(c => c.DiscountValue)
                .HasColumnType("decimal(18,2)");

            builder.Property(c => c.MinOrderAmount)
                .HasColumnType("decimal(18,2)");
            builder.HasQueryFilter(c => !c.IsDeleted);
        }
    }
}
