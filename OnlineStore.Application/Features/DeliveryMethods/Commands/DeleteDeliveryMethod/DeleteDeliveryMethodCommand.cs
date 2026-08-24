using MediatR;

namespace OnlineStore.Application.Features.DeliveryMethods.Commands.DeleteDeliveryMethod
{
    public class DeleteDeliveryMethodCommand(int id) : IRequest
    {
        public int Id { get; } = id;
    }
}
