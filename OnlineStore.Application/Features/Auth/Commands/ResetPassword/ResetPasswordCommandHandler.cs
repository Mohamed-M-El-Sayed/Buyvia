using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Contracts.Services.Authentication;
using OnlineStore.Application.Features.Auth.Dtos;

namespace OnlineStore.Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler(
        IAuthService authService,
        ILogger<ResetPasswordCommandHandler> logger) : IRequestHandler<ResetPasswordCommand, MessageResponseDto>
    {
        public async Task<MessageResponseDto> Handle(
            ResetPasswordCommand request,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("Resetting password for email: {Email}", request.Email);

            var message = await authService.ResetPasswordAsync(
                request.Email,
                request.Token,
                request.NewPassword,
                cancellationToken);

            logger.LogInformation("Password reset successfully for email: {Email}", request.Email);
            return new MessageResponseDto
            {
                Message = message
            };
        }
    }
}
