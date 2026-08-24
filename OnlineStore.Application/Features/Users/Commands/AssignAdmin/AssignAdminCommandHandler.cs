using MediatR;
using OnlineStore.Application.Contracts.Services.Authentication;
using OnlineStore.Application.Features.Auth.Dtos;

namespace OnlineStore.Application.Features.Users.Commands.AssignAdmin;

public class AssignAdminCommandHandler(IUserService userService)
    : IRequestHandler<AssignAdminCommand, MessageResponseDto>
{
    public async Task<MessageResponseDto> Handle(
        AssignAdminCommand request,
        CancellationToken cancellationToken)
    {
        var message = await userService.AssignAdminAsync(request.UserId, cancellationToken);
        return new MessageResponseDto { Message = message };
    }
}