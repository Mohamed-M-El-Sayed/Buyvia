using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities.Orders;

namespace OnlineStore.Infrastructure.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.Property(p => p.Amount)
                  .HasColumnType("decimal(18,2)");


            builder.Property(p => p.Status)
                .HasConversion<string>();

            builder.Property(p => p.Method)
                .HasConversion<string>();

            builder.HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId);

        }
    }
}
