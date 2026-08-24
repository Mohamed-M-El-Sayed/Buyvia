using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Contracts.Services.Authentication;
using OnlineStore.Application.Features.Auth.Dtos;

namespace OnlineStore.Application.Features.Auth.Commands.Register;

public sealed class RegisterCommandHandler(
    IAuthService authService,
    ILogger<RegisterCommandHandler> logger)
    : IRequestHandler<RegisterCommand, RegisterResponseDto>
{
    public async Task<RegisterResponseDto> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Registering user with email {Email}.",
            request.Email);

        var response = await authService.RegisterAsync(
            new RegisterRequestDto
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = request.Password,
            },
            cancellationToken);

        logger.LogInformation(
            "User {Email} registered successfully.",
            request.Email);

        return response;
    }
}