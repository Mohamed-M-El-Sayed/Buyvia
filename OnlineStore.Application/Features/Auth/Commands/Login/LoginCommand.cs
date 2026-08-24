using MediatR;
using OnlineStore.Application.Features.Auth.Dtos;

namespace OnlineStore.Application.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<AuthResponseDto>
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
