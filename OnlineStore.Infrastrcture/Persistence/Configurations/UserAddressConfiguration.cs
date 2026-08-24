using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Domain.Entities.Identity;

namespace OnlineStore.Infrastructure.Persistence.Configurations
{
    public class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Country)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.IsDefault)
                .HasDefaultValue(false);

            // Only one default address per user
            builder.HasIndex(a => new { a.UserId, a.IsDefault })
                .IsUnique()
                .HasFilter("[IsDefault] = 1");
        }
    }
}
