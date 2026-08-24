using MediatR;

namespace OnlineStore.Application.Features.Orders.Commands.ApproveRefund
{
    public class ApproveRefundCommand(int refundRequestId) : IRequest<Unit>
    {
        public int RefundRequestId { get; } = refundRequestId;
    }
}
