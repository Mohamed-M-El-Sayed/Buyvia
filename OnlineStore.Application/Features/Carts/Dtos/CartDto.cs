namespace OnlineStore.Application.Features.Carts.Dtos
{
    public class CartDto
    {
        public int Id { get; set; }
        public decimal SubTotal => Items.Sum(i => i.UnitPrice * i.Quantity);
        public decimal ItemsDiscount => Items.Sum(i => i.DiscountAmount * i.Quantity);
        public string? CouponCode { get; set; }
        public decimal CouponDiscount { get; set; }
        public decimal Total => SubTotal - ItemsDiscount - CouponDiscount;
        public List<CartItemDto> Items { get; set; } = new();

    }
}