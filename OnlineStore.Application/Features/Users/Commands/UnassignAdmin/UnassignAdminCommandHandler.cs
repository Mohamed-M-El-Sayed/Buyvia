using MediatR;
using OnlineStore.Application.Contracts.Services.Authentication;
using OnlineStore.Application.Features.Auth.Dtos;

namespace OnlineStore.Application.Features.Users.Commands.UnassignAdmin;

public class UnassignAdminCommandHandler(IUserService userService)
    : IRequestHandler<UnassignAdminCommand, MessageResponseDto>
{
    public async Task<MessageResponseDto> Handle(
        UnassignAdminCommand request,
        CancellationToken cancellationToken)
    {
        var message = await userService.UnassignAdminAsync(request.UserId, cancellationToken);
        return new MessageResponseDto { Message = message };
    }
}