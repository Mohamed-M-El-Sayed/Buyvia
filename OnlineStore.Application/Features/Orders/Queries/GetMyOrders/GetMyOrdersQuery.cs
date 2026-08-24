using MediatR;
using OnlineStore.Application.Common.Models;
using OnlineStore.Application.Features.Orders.Dtos;

namespace OnlineStore.Application.Features.Orders.Queries.GetMyOrders
{
    public class GetMyOrdersQuery : IRequest<PageResult<OrderSummaryDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
