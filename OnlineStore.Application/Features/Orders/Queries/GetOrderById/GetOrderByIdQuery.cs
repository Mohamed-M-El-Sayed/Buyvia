using MediatR;
using OnlineStore.Application.Features.Orders.Dtos;

namespace OnlineStore.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQuery(int orderId) : IRequest<OrderDto>
    {
        public int OrderId { get; set; } = orderId;
    }
}
