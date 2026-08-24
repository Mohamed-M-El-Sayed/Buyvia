namespace OnlineStore.Application.Features.DeliveryMethods.Dtos
{
    public class DeliveryMethodDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public int EstimatedDeliveryDays { get; set; }
        public bool IsActive { get; set; }
    }
}
