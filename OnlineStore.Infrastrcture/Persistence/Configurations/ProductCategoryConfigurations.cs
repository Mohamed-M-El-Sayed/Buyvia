using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Infrastructure.Persistence.Configurations
{
    public class ProductCategoryConfigurations : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.Ignore(x => x.IsTopLevel);

            builder.HasOne(x => x.Parent)
            .WithMany(x => x.SubCategories)
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
        }
    }
}
