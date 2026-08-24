using MediatR;
using OnlineStore.Application.Common.Models;
using OnlineStore.Application.Contracts.Services.Authentication;
using OnlineStore.Application.Features.Auth.Dtos;

namespace OnlineStore.Application.Features.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler(IUserService userService)
     : IRequestHandler<GetAllUsersQuery, PageResult<UserDto>>
    {
        public async Task<PageResult<UserDto>> Handle(
            GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var (users, totalCount) = await userService.GetUsersAsync(
                request.SearchTerm,
                request.Role,
                request.PageNumber,
                request.PageSize,
                cancellationToken);

            var items = new List<UserDto>();
            foreach (var user in users)
            {
                var roles = await userService.GetRolesAsync(user);

                items.Add(new UserDto
                {
                    Id = user.Id.ToString(),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email!,
                    ProfilePictureUrl = user.ProfilePictureUrl ?? "default.png",
                    Roles = roles
                });
            }

            return new PageResult<UserDto>(items, request.PageNumber, request.PageSize, totalCount);

        }
    }
}
