using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Contracts.Services.Payment;
using OnlineStore.Application.Features.Payments.Specifications;
using OnlineStore.Domain.Entities.Orders;
using OnlineStore.Domain.Entities.Promotions;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Payments.Commands.HandleWebhook
{
    public class HandleWebhookCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<HandleWebhookCommandHandler> logger,
        IPaymentService paymentService)
        : IRequestHandler<HandleWebhookCommand, Unit>
    {
        public async Task<Unit> Handle(
            HandleWebhookCommand request,
            CancellationToken cancellationToken)
        {
            var eventType = paymentService.ParseWebhookEvent(
                request.Payload,
                request.Signature);

            using var json = JsonDocument.Parse(request.Payload);

            // Get the PaymentIntent object from the webhook
            var paymentIntent = json.RootElement
                .GetProperty("data")
                .GetProperty("object");

            var intentId = paymentIntent
                .GetProperty("id")
                .GetString()
                ?? throw new BadRequestException(
                    "Invalid payment intent ID.");

            var payment = await unitOfWork
                .Repository<Payment>()
                .GetEntityWithSpecAsync(
                    new PaymentByIntentIdSpecification(intentId),
                    cancellationToken)
                ?? throw new NotFoundException(
                    nameof(Payment),
                    intentId);

            switch (eventType)
            {
                case PaymentEventType.Succeeded:

                    // Ignore duplicate successful webhook
                    if (payment.Status == PaymentStatus.Paid)
                    {
                        logger.LogInformation(
                            "Duplicate succeeded webhook ignored for Payment {PaymentId}.",
                            payment.Id);

                        break;
                    }

                    // Do not reactivate an expired order
                    if (payment.Order.Status == OrderStatus.Expired)
                    {
                        logger.LogWarning(
                            "Payment succeeded for expired Order {OrderId}. " +
                            "Payment will be refunded.",
                            payment.OrderId);

                        payment.Status = PaymentStatus.Paid;
                        payment.PaidAt = DateTime.UtcNow;

                        if (paymentIntent.TryGetProperty(
                                "latest_charge",
                                out var latestCharge) &&
                            latestCharge.ValueKind != JsonValueKind.Null)
                        {
                            payment.TransactionId =
                                latestCharge.GetString();
                        }

                        await paymentService.RefundAsync(
                            payment.PaymentIntentId!,
                            cancellationToken);

                        break;
                    }

                    // Do not move a failed/cancelled payment to paid
                    if (payment.Status is
                        PaymentStatus.Failed or
                        PaymentStatus.Cancelled or
                        PaymentStatus.Refunded)
                    {
                        logger.LogWarning(
                            "Ignoring succeeded webhook for Payment {PaymentId} " +
                            "because current status is {Status}.",
                            payment.Id,
                            payment.Status);

                        break;
                    }

                    payment.Status = PaymentStatus.Paid;
                    payment.PaidAt = DateTime.UtcNow;

                    if (paymentIntent.TryGetProperty(
                            "latest_charge",
                            out var charge) &&
                        charge.ValueKind != JsonValueKind.Null)
                    {
                        payment.TransactionId =
                            charge.GetString();
                    }

                    payment.Order.Status =
                        OrderStatus.PaymentReceived;

                    logger.LogInformation(
                        "Payment succeeded for Order {OrderId}.",
                        payment.OrderId);

                    break;


                case PaymentEventType.Failed:

                    // Ignore if payment was already completed
                    if (payment.Status is
                        PaymentStatus.Paid or
                        PaymentStatus.Refunded)
                    {
                        logger.LogWarning(
                            "Ignoring failed webhook for Payment {PaymentId} " +
                            "because current status is {Status}.",
                            payment.Id,
                            payment.Status);

                        break;
                    }

                    // Ignore duplicate webhook
                    if (payment.Status == PaymentStatus.Failed)
                    {
                        logger.LogInformation(
                            "Duplicate failed webhook ignored for Payment {PaymentId}.",
                            payment.Id);

                        break;
                    }

                    payment.Status = PaymentStatus.Failed;
                    payment.FailureReason = "Payment failed";

                    // Restore resources only if order is still pending
                    if (payment.Order.Status == OrderStatus.Pending)
                    {
                        payment.Order.Status =
                            OrderStatus.Cancelled;

                        await RestoreOrderResourcesAsync(
                            payment,
                            cancellationToken);
                    }

                    logger.LogWarning(
                        "Payment failed for Order {OrderId}.",
                        payment.OrderId);

                    break;


                case PaymentEventType.Canceled:

                    // Ignore if payment was already completed
                    if (payment.Status is
                        PaymentStatus.Paid or
                        PaymentStatus.Refunded)
                    {
                        logger.LogWarning(
                            "Ignoring cancelled webhook for Payment {PaymentId} " +
                            "because current status is {Status}.",
                            payment.Id,
                            payment.Status);

                        break;
                    }

                    // Ignore duplicate webhook
                    if (payment.Status == PaymentStatus.Cancelled)
                    {
                        logger.LogInformation(
                            "Duplicate cancelled webhook ignored for Payment {PaymentId}.",
                            payment.Id);

                        break;
                    }

                    payment.Status = PaymentStatus.Cancelled;
                    payment.FailureReason =
                        "Payment cancelled";

                    // Restore resources only if order is still pending
                    if (payment.Order.Status == OrderStatus.Pending)
                    {
                        payment.Order.Status =
                            OrderStatus.Cancelled;

                        await RestoreOrderResourcesAsync(
                            payment,
                            cancellationToken);
                    }

                    logger.LogWarning(
                        "Payment cancelled for Order {OrderId}.",
                        payment.OrderId);

                    break;


                case PaymentEventType.RequiresAction:

                    // Do not change a final payment back to pending
                    if (payment.Status is
                        PaymentStatus.Paid or
                        PaymentStatus.Failed or
                        PaymentStatus.Cancelled or
                        PaymentStatus.Refunded)
                    {
                        logger.LogInformation(
                            "Ignoring requires_action webhook for Payment {PaymentId} " +
                            "because current status is {Status}.",
                            payment.Id,
                            payment.Status);

                        break;
                    }

                    payment.Status = PaymentStatus.Pending;

                    logger.LogInformation(
                        "Payment requires action for Order {OrderId}.",
                        payment.OrderId);

                    break;


                case PaymentEventType.Processing:

                    // Do not change a final payment back to pending
                    if (payment.Status is
                        PaymentStatus.Paid or
                        PaymentStatus.Failed or
                        PaymentStatus.Cancelled or
                        PaymentStatus.Refunded)
                    {
                        logger.LogInformation(
                            "Ignoring processing webhook for Payment {PaymentId} " +
                            "because current status is {Status}.",
                            payment.Id,
                            payment.Status);

                        break;
                    }

                    payment.Status = PaymentStatus.Pending;

                    logger.LogInformation(
                        "Payment processing for Order {OrderId}.",
                        payment.OrderId);

                    break;


                case PaymentEventType.Unknown:

                    // Ignore unsupported events
                    logger.LogWarning(
                        "Unhandled payment event for PaymentIntent {PaymentIntentId}.",
                        intentId);

                    break;
            }

            await unitOfWork.CompleteAsync(cancellationToken);

            return Unit.Value;
        }

        private async Task RestoreOrderResourcesAsync(
            Payment payment,
            CancellationToken cancellationToken)
        {
            // Restore stock
            foreach (var item in payment.Order.Items)
            {
                item.ProductVariant.Stock += item.Quantity;
            }

            // Restore coupon usage
            if (payment.Order.CouponId.HasValue)
            {
                var coupon = await unitOfWork
                    .Repository<Coupon>()
                    .GetByIdAsync(payment.Order.CouponId.Value);

                if (coupon is not null && coupon.UsedCount > 0)
                {
                    coupon.UsedCount--;
                }
            }
        }
    }
}