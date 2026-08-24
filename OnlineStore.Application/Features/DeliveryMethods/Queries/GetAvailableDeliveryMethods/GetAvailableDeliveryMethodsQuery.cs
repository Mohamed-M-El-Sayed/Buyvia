using MediatR;
using OnlineStore.Application.Features.DeliveryMethods.Dtos;

namespace OnlineStore.Application.Features.DeliveryMethods.Queries.GetAvailableDeliveryMethods
{
    public class GetAvailableDeliveryMethodsQuery : IRequest<List<DeliveryMethodDto>>
    {
    }
}
