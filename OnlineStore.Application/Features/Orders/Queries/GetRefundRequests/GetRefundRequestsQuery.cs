using MediatR;
using OnlineStore.Application.Common.Models;
using OnlineStore.Application.Features.Orders.Dtos;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Orders.Queries.GetRefundRequests
{
    public class GetRefundRequestsQuery
        : IRequest<PageResult<RefundRequestDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public RefundRequestStatus? Status { get; set; }
    }
}
