using MediatR;
using OnlineStore.Application.Features.Auth.Dtos;

namespace OnlineStore.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<AuthResponseDto>
    {
        public string AccessToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
    }
}
