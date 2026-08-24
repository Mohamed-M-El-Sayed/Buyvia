using MediatR;
using OnlineStore.Application.Features.Auth.Dtos;

namespace OnlineStore.Application.Features.Auth.Queries.GetCurrentUser
{
    public class GetCurrentUserQuery : IRequest<UserDto>
    {
    }
}
