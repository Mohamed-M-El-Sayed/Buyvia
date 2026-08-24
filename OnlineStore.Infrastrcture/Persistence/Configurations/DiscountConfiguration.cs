using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities.Promotions;

namespace OnlineStore.Infrastructure.Persistence.Configurations
{
    public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {



            builder.Property(d => d.Type)
                   .IsRequired()
                   .HasConversion<string>();

            builder.Property(d => d.Value)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(d => d.MaxDiscountAmount)
                   .HasColumnType("decimal(18,2)");

            builder.HasMany(d => d.Variants)
            .WithOne(v => v.Discount)
            .HasForeignKey(v => v.DiscountId)
            .OnDelete(DeleteBehavior.SetNull);

            builder.HasQueryFilter(d => !d.IsDeleted);

        }
    }
}
