namespace OnlineStore.Application.Features.Orders.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Status { get; set; } = default!;

        // public PaymentStatus PaymentStatus { get; set; }

        public decimal Subtotal { get; set; }

        public decimal ItemsDiscount { get; set; }

        public decimal CouponDiscount { get; set; }

        public decimal DeliveryFee { get; set; }

        public decimal Total { get; set; }

        public string? CouponCode { get; set; }

        public OrderDeliveryMethodDto DeliveryMethod { get; set; } = default!;
        public OrderAddressDto ShippingAddress { get; set; } = default!;

        public List<OrderItemDto> Items { get; set; } = [];
    }
}
