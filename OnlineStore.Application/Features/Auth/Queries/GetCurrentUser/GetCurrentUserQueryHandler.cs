using MediatR;
using OnlineStore.Application.Contracts.Services.Authentication;
using OnlineStore.Application.Features.Auth.Dtos;

namespace OnlineStore.Application.Features.Auth.Queries.GetCurrentUser
{
    public class GetCurrentUserQueryHandler(IAuthService authService) : IRequestHandler<GetCurrentUserQuery, UserDto>
    {
        public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            return await authService.GetCurrentUserAsync(cancellationToken);
        }
    }
}
