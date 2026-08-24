using System.Text.Json.Serialization;
using MediatR;

namespace OnlineStore.Application.Features.DeliveryMethods.Commands.UpdateDeliveryMethod
{
    public class UpdateDeliveryMethodCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; init; } = default!;
        public string Description { get; init; } = default!;
        public decimal Price { get; init; }
        public int EstimatedDeliveryDays { get; init; }
    }
}
