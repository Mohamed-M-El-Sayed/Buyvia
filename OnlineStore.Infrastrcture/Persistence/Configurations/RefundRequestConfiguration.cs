using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities.Orders;

namespace OnlineStore.Infrastructure.Persistence.Configurations
{
    public class RefundRequestConfiguration
        : IEntityTypeConfiguration<RefundRequest>
    {
        public void Configure(
            EntityTypeBuilder<RefundRequest> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasQueryFilter(x => !x.IsDeleted);

            builder.Property(x => x.Amount)
                .HasPrecision(18, 2);

            builder.Property(x => x.Status)
                .HasConversion<string>();

            builder.HasOne(x => x.Order)
                .WithMany(x => x.RefundRequests)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new
            {
                x.UserId,
                x.Status
            });
        }
    }
}