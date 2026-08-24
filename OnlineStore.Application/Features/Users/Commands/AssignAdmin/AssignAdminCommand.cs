using MediatR;
using OnlineStore.Application.Features.Auth.Dtos;

namespace OnlineStore.Application.Features.Users.Commands.AssignAdmin
{
    public class AssignAdminCommand : IRequest<MessageResponseDto>
    {
        public Guid UserId { get; set; }

    }
}
