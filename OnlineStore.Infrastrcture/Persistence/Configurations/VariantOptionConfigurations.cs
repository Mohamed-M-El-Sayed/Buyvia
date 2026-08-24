using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Infrastructure.Persistence.Configurations
{
    public class VariantOptionConfigurations : IEntityTypeConfiguration<VariantOption>
    {
        public void Configure(EntityTypeBuilder<VariantOption> builder)
        {


            builder.HasIndex(v => new { v.VariantId, v.OptionId })
                .IsUnique();

            builder.HasIndex(v => new { v.VariantId, v.OptionValueId })
                .IsUnique();

            builder.HasOne(v => v.Variant)
                .WithMany(pv => pv.Options)
                .HasForeignKey(v => v.VariantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(v => v.Option)
                .WithMany()
                .HasForeignKey(v => v.OptionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(v => v.Value)
                .WithMany()
                .HasForeignKey(v => new { v.OptionValueId, v.OptionId })
                .HasPrincipalKey(v => new { v.Id, v.OptionId })
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
