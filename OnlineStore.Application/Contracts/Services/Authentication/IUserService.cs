
using OnlineStore.Domain.Entities.Identity;

namespace OnlineStore.Application.Contracts.Services.Authentication
{
    public interface IUserService
    {
        Task<ApplicationUser?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
        Task UpdateAsync(ApplicationUser user);
        Task<IList<ApplicationUser>> GetUsersInRoleAsync(string role);
        Task<(IList<ApplicationUser> Users, int TotalCount)> GetUsersAsync(string? searchTerm,
            string? role,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task<string> AssignAdminAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<string> UnassignAdminAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
