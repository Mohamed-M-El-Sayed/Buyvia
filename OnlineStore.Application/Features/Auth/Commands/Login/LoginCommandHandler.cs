using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Contracts.Services.Authentication;
using OnlineStore.Application.Features.Auth.Dtos;

namespace OnlineStore.Application.Features.Auth.Commands.Login;

public sealed class LoginCommandHandler(
    IAuthService authService,
    ILogger<LoginCommandHandler> logger)
    : IRequestHandler<LoginCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Login attempt for user {Email}.",
            request.Email);

        var response = await authService.LoginAsync(
            request.Email,
            request.Password,
            cancellationToken);

        logger.LogInformation(
            "User {Email} logged in successfully.",
            request.Email);

        return response;
    }
}