using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Infrastructure.Persistence.Configurations
{
    public class ProductVariantConfigurations : IEntityTypeConfiguration<ProductVariant>
    {
        public void Configure(EntityTypeBuilder<ProductVariant> builder)
        {
            builder.Property(v => v.Price)
                .HasColumnType("decimal(18,2)");
            builder.HasQueryFilter(v => !v.IsDeleted);

            // Add index on IsActive for performance on customer-facing queries
            builder.HasIndex(v => v.IsActive);
            builder.HasIndex(v => v.IsDefault);


            builder.HasIndex(v => new { v.ProductId, v.IsDefault })
                    .IsUnique()
                    .HasFilter("[IsDefault] = 1");
        }
    }
}
