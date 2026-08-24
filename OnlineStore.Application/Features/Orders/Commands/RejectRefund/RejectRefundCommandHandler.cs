using MediatR;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Domain.Entities.Orders;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Orders.Commands.RejectRefund
{
    public class RejectRefundCommandHandler(
        IUnitOfWork unitOfWork)
        : IRequestHandler<RejectRefundCommand, Unit>
    {
        public async Task<Unit> Handle(
            RejectRefundCommand request,
            CancellationToken cancellationToken)
        {


            var refundRequest = await unitOfWork
                .Repository<RefundRequest>()
                .GetByIdAsync(request.RefundRequestId);

            if (refundRequest is null)
                throw new NotFoundException(
                    nameof(RefundRequest),
                    request.RefundRequestId.ToString());

            if (refundRequest.Status != RefundRequestStatus.Pending)
                throw new BadRequestException(
                    "Only pending refund requests can be rejected.");

            refundRequest.Status = RefundRequestStatus.Rejected;
            refundRequest.RejectionReason = request.Reason;
            refundRequest.ReviewedAt = DateTime.UtcNow;
            await unitOfWork.CompleteAsync();
            return Unit.Value;
        }
    }
}
