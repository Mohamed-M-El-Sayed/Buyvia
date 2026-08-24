using MediatR;
using OnlineStore.Application.Features.Auth.Dtos;

namespace OnlineStore.Application.Features.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest<MessageResponseDto>
    {
        public string Email { get; set; } = default!;
    }
}
