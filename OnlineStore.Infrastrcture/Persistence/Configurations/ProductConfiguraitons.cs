using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguraitons : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            builder.HasQueryFilter(v => !v.IsDeleted);

            builder.HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(p => p.Status)
                .HasConversion<string>();
        }
    }
}
