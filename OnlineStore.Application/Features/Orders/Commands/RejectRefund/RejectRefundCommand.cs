using MediatR;

namespace OnlineStore.Application.Features.Orders.Commands.RejectRefund
{
    public class RejectRefundCommand : IRequest<Unit>
    {
        public int RefundRequestId { get; set; }
        public string? Reason { get; set; }
    }
}
