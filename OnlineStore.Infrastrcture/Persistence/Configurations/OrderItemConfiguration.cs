using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities.Orders;

namespace OnlineStore.Infrastructure.Persistence.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {

        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(i => i.UnitPrice)
                 .HasColumnType("decimal(18,2)");

            builder.Property(i => i.UnitDiscountAmount)
                    .HasColumnType("decimal(18,2)");

        }
    }
}
