using MediatR;
using OnlineStore.Application.Contracts.Services.Authentication;
using OnlineStore.Application.Features.Auth.Dtos;

namespace OnlineStore.Application.Features.Auth.Commands.ForgotPassword
{
    public class ForgotPasswordCommandHandler(
        IAuthService authService)
        : IRequestHandler<ForgotPasswordCommand, MessageResponseDto>
    {
        public async Task<MessageResponseDto> Handle(
            ForgotPasswordCommand request,
            CancellationToken cancellationToken)
        {
            var message = await authService.ForgotPasswordAsync(
                request.Email,
                cancellationToken);

            return new MessageResponseDto
            {
                Message = message
            };
        }
    }
}