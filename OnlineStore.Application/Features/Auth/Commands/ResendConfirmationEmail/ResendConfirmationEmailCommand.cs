using MediatR;
using OnlineStore.Application.Features.Auth.Dtos;

namespace OnlineStore.Application.Features.Auth.Commands.ResendConfirmationEmail
{
    public class ResendConfirmationEmailCommand : IRequest<MessageResponseDto>
    {
        public string Email { get; set; } = default!;

    }
}
