using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Entities;
using OnlineStore.Domain.Entities.Identity;
using OnlineStore.Domain.Entities.Products;
using OnlineStore.Domain.Entities.Promotions;
using OnlineStore.Domain.Entities.Orders;
using OnlineStore.Domain.Entities.ShoppingCart;

namespace OnlineStore.Infrastructure.Persistence
{
    public class OnlineStoreDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public OnlineStoreDbContext(DbContextOptions<OnlineStoreDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> Categories { get; set; }
        public DbSet<ProductBrand> Brands { get; set; }
        public DbSet<ProductImage> Images { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ProductOption> ProductOptions { get; set; }
        public DbSet<ProductOptionValue> ProductOptionValues { get; set; }
        public DbSet<VariantOption> VariantOptions { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}