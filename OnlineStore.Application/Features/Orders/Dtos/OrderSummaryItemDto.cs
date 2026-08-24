namespace OnlineStore.Application.Features.Orders.Dtos
{
    public class OrderSummaryItemDto
    {
        public string ProductName { get; set; } = default!;

        public string VariantName { get; set; } = default!;

        public string ImageUrl { get; set; } = default!;

        public int Quantity { get; set; }
    }
}
