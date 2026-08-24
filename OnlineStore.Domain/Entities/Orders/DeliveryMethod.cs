using OnlineStore.Domain.Entities.BaseEntities;

namespace OnlineStore.Domain.Entities.Orders
{
    public class DeliveryMethod : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public int EstimatedDeliveryDays { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
