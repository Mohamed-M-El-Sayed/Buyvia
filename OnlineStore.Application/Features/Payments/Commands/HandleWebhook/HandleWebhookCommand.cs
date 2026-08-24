using MediatR;

namespace OnlineStore.Application.Features.Payments.Commands.HandleWebhook
{
    public class HandleWebhookCommand : IRequest<Unit>
    {
        public string Payload { get; set; } = default!;
        public string Signature { get; set; } = default!;
    }
}
