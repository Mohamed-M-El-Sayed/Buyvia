using MediatR;

namespace OnlineStore.Application.Features.DeliveryMethods.Commands.SetDeliveryMethodActiveStatus
{
    public class SetDeliveryMethodActiveStatusCommand(int deliveryMethodId, bool isActive) : IRequest<Unit>
    {
        public int DeliveryMethodId { get; } = deliveryMethodId;
        public bool IsActive { get; } = isActive;
    }
}
