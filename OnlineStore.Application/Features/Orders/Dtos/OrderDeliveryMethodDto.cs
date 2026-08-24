namespace OnlineStore.Application.Features.Orders.Dtos
{
    public class OrderDeliveryMethodDto
    {
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public int EstimatedDeliveryDays { get; set; }
    }
}
