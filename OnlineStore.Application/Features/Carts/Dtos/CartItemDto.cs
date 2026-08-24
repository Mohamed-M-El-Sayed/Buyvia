namespace OnlineStore.Application.Features.Carts.Dtos
{
    public class CartItemDto
    {
        // public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public string VariantName { get; set; } = default!;
        public string? ImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalUnitPrice => UnitPrice - DiscountAmount;
        public decimal TotalPrice => FinalUnitPrice * Quantity;
    }
}
