using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Infrastructure.Persistence.Configurations
{
    public class ProductOptionConfigurations : IEntityTypeConfiguration<ProductOption>
    {
        public void Configure(EntityTypeBuilder<ProductOption> builder)
        {
            builder.Property(o => o.Name)
                .IsRequired()
                .HasMaxLength(100);

            //builder.Property(o => o.DisplayOrder)
            //    .HasDefaultValue(0);

            builder.HasIndex(o => new { o.ProductId, o.Name })
                .IsUnique();

            //builder.HasIndex(o => new { o.ProductId, o.DisplayOrder });

            builder.HasOne(o => o.Product)
                .WithMany(p => p.Options)
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
