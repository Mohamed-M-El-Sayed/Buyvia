using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Infrastructure.Persistence.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasIndex(r => new { r.UserId, r.ProductId })
                .IsUnique();
            // user can write only one review per variant
            //builder.Property(r => r.IsVerifiedPurchase)
            //       .HasDefaultValue(false);

            //builder.HasOne<ApplicationUser>()
            //       .WithMany()
            //       .HasForeignKey(r => r.UserId);

            builder.HasOne(r => r.Product)
                   .WithMany(v => v.Reviews)
                   .HasForeignKey(r => r.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasQueryFilter(r => !r.IsDeleted);
            builder.HasOne(r => r.PurchasedVariant)
                .WithMany()
                .HasForeignKey(r => r.PurchasedVariantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
