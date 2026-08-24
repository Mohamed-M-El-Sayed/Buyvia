using OnlineStore.Domain.Entities.BaseEntities;
using OnlineStore.Domain.Entities.Promotions;

namespace OnlineStore.Domain.Entities.Products
{
    public class ProductVariant : SoftDeletableEntity
    {
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int StockThreshold { get; set; }
        public DateTime? LowStockAlertedAt { get; set; }
        public int? DiscountId { get; set; }
        public bool IsDefault { get; set; } = true;
        public bool IsActive { get; set; } = true;
        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;
        public ICollection<VariantOption> Options { get; set; } = new List<VariantOption>();
        public Discount? Discount { get; set; }

        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public string GetVariantName()
        => string.Join(" - ", Options.Select(o => o.Value.Value));

        public decimal FinalPrice =>
            Discount is null
                ? Price
                : Price - Discount.CalculateDiscount(Price);
    }
}
