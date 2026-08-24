using MediatR;
using OnlineStore.Application.Features.Payments.Dtos;

namespace OnlineStore.Application.Features.Payments.Commands.CreatePaymentIntent
{
    public class CreatePaymentIntentCommand : IRequest<PaymentIntentDto>
    {
        public int OrderId { get; set; }

    }
}
