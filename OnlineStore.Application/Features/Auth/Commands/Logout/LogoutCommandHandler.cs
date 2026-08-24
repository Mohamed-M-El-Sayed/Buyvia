using MediatR;
using OnlineStore.Application.Contracts.Services.Authentication;

namespace OnlineStore.Application.Features.Auth.Commands.Logout
{
    public class LogoutCommandHandler(IAuthService authService) : IRequestHandler<LogoutCommand, Unit>
    {
        public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            await authService.LogoutAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
