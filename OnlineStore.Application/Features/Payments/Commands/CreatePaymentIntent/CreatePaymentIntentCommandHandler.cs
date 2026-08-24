using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Contracts.Services.Payment;
using OnlineStore.Application.Features.Orders.Specifications;
using OnlineStore.Application.Features.Payments.Dtos;
using OnlineStore.Domain.Entities.Orders;
using OnlineStore.Domain.Enums;

namespace OnlineStore.Application.Features.Payments.Commands.CreatePaymentIntent
{
    public class CreatePaymentIntentCommandHandler(
        IUnitOfWork unitOfWork,
        IPaymentService paymentService,
        ILogger<CreatePaymentIntentCommandHandler> logger)
        : IRequestHandler<CreatePaymentIntentCommand, PaymentIntentDto>
    {
        public async Task<PaymentIntentDto> Handle(
            CreatePaymentIntentCommand request, CancellationToken cancellationToken)
        {
            var order = await unitOfWork.Repository<Order>()
                .GetEntityWithSpecAsync(new OrderWithPaymentSpecification(request.OrderId))
                ?? throw new NotFoundException(nameof(Order), request.OrderId.ToString());

            if (order?.Payment?.Method == PaymentMethod.CashOnDelivery)
                throw new BadRequestException(
                    "Cash on delivery does not require a payment intent");

            if (order?.Payment?.Status != PaymentStatus.Pending)
                throw new BadRequestException(
                    "Order is already paid or failed");

            var paymentIntent = await paymentService.CreatePaymentIntentAsync(
                order.Payment.Amount, order.Id, cancellationToken: cancellationToken);

            order.Payment.PaymentIntentId = paymentIntent.PaymentIntentId;
            await unitOfWork.CompleteAsync(cancellationToken);

            logger.LogInformation(
                "PaymentIntent {PaymentIntentId} created for Order {OrderId}",
                paymentIntent.PaymentIntentId, order.Id);

            return new PaymentIntentDto
            {
                PaymentIntentId = paymentIntent.PaymentIntentId,
                ClientSecret = paymentIntent.ClientSecret
            };
        }
    }
}