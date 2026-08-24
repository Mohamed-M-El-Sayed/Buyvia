using MediatR;
using OnlineStore.Application.Common.Models;
using OnlineStore.Application.Features.Auth.Dtos;

namespace OnlineStore.Application.Features.Users.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<PageResult<UserDto>>
    {
        public string? SearchTerm { get; set; }
        public string? Role { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
}
