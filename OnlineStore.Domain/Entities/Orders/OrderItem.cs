using OnlineStore.Domain.Entities.BaseEntities;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Domain.Entities.Orders
{
    public class OrderItem : SoftDeletableEntity
    {

        public int ProductId { get; set; }
        public int ProductVariantId { get; set; }
        public string ProductName { get; set; } = default!;
        public string VariantName { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitDiscountAmount { get; set; }
        public decimal FinalPrice => (UnitPrice - UnitDiscountAmount) * Quantity;
        public int OrderId { get; set; }
        public Order Order { get; set; } = default!;
        public ProductVariant ProductVariant { get; set; } = default!;

    }
}
