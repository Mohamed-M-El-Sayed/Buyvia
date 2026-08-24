using MediatR;

namespace OnlineStore.Application.Features.Auth.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest<Unit>
    {
        public string UserId { get; set; } = default!;
        public string Token { get; set; } = default!;
    }
}
