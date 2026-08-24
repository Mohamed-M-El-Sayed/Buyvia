using MediatR;
using OnlineStore.Application.Contracts.Services.Authentication;
using OnlineStore.Application.Features.Auth.Dtos;

namespace OnlineStore.Application.Features.Auth.Commands.ResendConfirmationEmail;

public class ResendConfirmationEmailCommandHandler(IAuthService authService)
    : IRequestHandler<ResendConfirmationEmailCommand, MessageResponseDto>
{
    public async Task<MessageResponseDto> Handle(
        ResendConfirmationEmailCommand request,
        CancellationToken cancellationToken)
    {
        var message = await authService.ResendConfirmationEmailAsync(request.Email, cancellationToken);
        return new MessageResponseDto { Message = message };
    }
}