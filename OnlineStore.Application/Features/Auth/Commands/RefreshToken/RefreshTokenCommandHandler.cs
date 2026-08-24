using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Contracts.Services.Authentication;
using OnlineStore.Application.Features.Auth.Dtos;

namespace OnlineStore.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler(
    IAuthService authService,
    ILogger<RefreshTokenCommandHandler> logger)
    : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
    {

        public async Task<AuthResponseDto> Handle(
            RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Processing refresh token request.");

            var result = await authService.RefreshTokenAsync(
                request.AccessToken,
                request.RefreshToken,
                cancellationToken);
            return result;
        }
    }
}
