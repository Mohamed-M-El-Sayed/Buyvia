using MediatR;

namespace OnlineStore.Application.Features.DeliveryMethods.Commands.CreateDeliveryMethod
{
    public class CreateDeliveryMethodCommand : IRequest<int>
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public int EstimatedDeliveryDays { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
