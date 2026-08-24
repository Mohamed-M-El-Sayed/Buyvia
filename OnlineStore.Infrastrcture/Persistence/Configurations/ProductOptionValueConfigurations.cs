using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Infrastructure.Persistence.Configurations
{
    public class ProductOptionValueConfigurations : IEntityTypeConfiguration<ProductOptionValue>
    {
        public void Configure(EntityTypeBuilder<ProductOptionValue> builder)
        {
            builder.Property(v => v.Value)
                .IsRequired()
                .HasMaxLength(100);

            /* builder.Property(v => v.DisplayOrder)
                 .HasDefaultValue(0);*/

            builder.HasIndex(v => new { v.OptionId, v.Value })
                .IsUnique();

            // builder.HasIndex(v => new { v.OptionId, v.DisplayOrder });

            builder.HasAlternateKey(v => new { v.Id, v.OptionId });

            builder.HasOne(v => v.Option)
                .WithMany(o => o.Values)
                .HasForeignKey(v => v.OptionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
