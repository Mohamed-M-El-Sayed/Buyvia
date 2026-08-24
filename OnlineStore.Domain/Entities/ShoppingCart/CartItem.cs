using OnlineStore.Domain.Entities.BaseEntities;
using OnlineStore.Domain.Entities.Products;

namespace OnlineStore.Domain.Entities.ShoppingCart
{
    public class CartItem : BaseEntity
    {
        public int CartId { get; set; }
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        // public decimal Subtotal { get; set; }

        public Cart Cart { get; set; } = default!;
        public ProductVariant ProductVariant { get; set; } = default!;
    }
}