namespace OnlineStore.Application.Features.Orders.Dtos
{
    public class OrderItemDto
    {
        public string ProductName { get; set; } = default!;
        public string VariantName { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitDiscountAmount { get; set; }
        public decimal LineTotal => (UnitPrice - UnitDiscountAmount) * Quantity;
    }
}
