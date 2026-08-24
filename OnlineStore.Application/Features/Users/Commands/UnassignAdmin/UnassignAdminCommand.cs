using MediatR;
using OnlineStore.Application.Features.Auth.Dtos;

namespace OnlineStore.Application.Features.Users.Commands.UnassignAdmin;

public class UnassignAdminCommand : IRequest<MessageResponseDto>
{
    public Guid UserId { get; set; }
}