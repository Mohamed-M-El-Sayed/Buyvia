using MediatR;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Contracts.Services.Payment;
using OnlineStore.Application.Features.Orders.Specifications;
using OnlineStore.Domain.Entities.Orders;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Orders.Commands.ApproveRefund
{
    public class ApproveRefundCommandHandler(
        IUnitOfWork unitOfWork,
        IPaymentService paymentService)
        : IRequestHandler<ApproveRefundCommand, Unit>
    {
        public async Task<Unit> Handle(
            ApproveRefundCommand request,
            CancellationToken cancellationToken)
        {
            var spec = new RefundRequestForApprovalSpecification(
                request.RefundRequestId);

            var refundRequest = await unitOfWork
                .Repository<RefundRequest>()
                .GetEntityWithSpecAsync(spec);

            if (refundRequest is null)
                throw new NotFoundException(
                    nameof(RefundRequest),
                    request.RefundRequestId.ToString());

            if (refundRequest.Status != RefundRequestStatus.Pending)
                throw new BadRequestException(
                    "Only pending refund requests can be approved.");

            var payment = refundRequest.Order.Payment;

            if (payment.Status != PaymentStatus.Paid)
                throw new BadRequestException(
                    "Only paid orders can be refunded.");

            if (string.IsNullOrWhiteSpace(payment.PaymentIntentId))
                throw new BadRequestException(
                    "Payment intent was not found.");

            // Refund through Stripe first
            var refund = await paymentService.RefundAsync(
                payment.PaymentIntentId,
                cancellationToken);

            if (refund.Status != "succeeded")
                throw new BadRequestException(
                    "The refund could not be completed.");

            // Start database transaction
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                // Restore stock
                foreach (var item in refundRequest.Order.Items)
                {
                    item.ProductVariant.Stock += item.Quantity;
                }

                // Update payment
                payment.Status = PaymentStatus.Refunded;
                payment.RefundId = refund.RefundId;

                // Update order status
                refundRequest.Order.Status = OrderStatus.Refunded;

                // Restore coupon usage
                if (refundRequest.Order.Coupon is not null &&
                    refundRequest.Order.Coupon.UsedCount > 0)
                {
                    refundRequest.Order.Coupon.UsedCount--;
                }

                // Update refund request
                refundRequest.Status = RefundRequestStatus.Refunded;
                refundRequest.ReviewedAt = DateTime.UtcNow;

                await unitOfWork.CompleteAsync(cancellationToken);

                await unitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }

            return Unit.Value;
        }
    }
}