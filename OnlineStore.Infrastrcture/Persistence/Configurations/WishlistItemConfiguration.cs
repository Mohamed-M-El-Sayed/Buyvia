using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities.Wishlists;

namespace OnlineStore.Infrastructure.Persistence.Configurations
{
    public class WishlistItemConfiguration : IEntityTypeConfiguration<WishlistItem>
    {
        public void Configure(EntityTypeBuilder<WishlistItem> builder)
        {
            builder.HasIndex(wi => new { wi.WishlistId, wi.ProductVariantId })
               .IsUnique();
        }
    }
}
