using MediatR;
using OnlineStore.Application.Features.Orders.Dtos;

namespace OnlineStore.Application.Features.DeliveryMethods.Queries.GetDeliveryById
{
    public class GetDeliveryByIdQuery(int id) : IRequest<OrderDeliveryMethodDto>
    {
        public int Id { get; } = id;
    }
}
