using MediatR;
using Microsoft.Extensions.Logging;
using OnlineStore.Application.Contracts.Services.Authentication;

namespace OnlineStore.Application.Features.Auth.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandHandler(ILogger<ConfirmEmailCommandHandler> logger,
        IAuthService authService) : IRequestHandler<ConfirmEmailCommand, Unit>
    {
        public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Confirming email for user {UserId}.",
                request.UserId);
            await authService.ConfirmEmailAsync(
                      request.UserId,
                      request.Token,
                      cancellationToken);

            logger.LogInformation(
                "Email confirmed successfully for user {UserId}.",
                request.UserId);
            return Unit.Value;
        }
    }
}
