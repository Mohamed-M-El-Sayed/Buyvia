using Microsoft.AspNetCore.Identity;
using OnlineStore.Application.Common.Exceptions;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Services.Authentication;
using OnlineStore.Domain.Constants;
using OnlineStore.Domain.Entities.Identity;

namespace OnlineStore.Infrastructure.Services.Authentication
{
    public class UserService(UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService) : IUserService
    {
        public async Task<ApplicationUser?> FindByIdAsync(
       Guid id,
       CancellationToken cancellationToken)
        {
            return await userManager.FindByIdAsync(id.ToString());
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(
                        ", ",
                        result.Errors.Select(e => e.Description)));
            }
        }
        public async Task<IList<ApplicationUser>> GetUsersInRoleAsync(string role)
        {
            IList<ApplicationUser>? users = await userManager.GetUsersInRoleAsync(role);
            return users;
        }
        public async Task<(IList<ApplicationUser> Users, int TotalCount)> GetUsersAsync(
            string? searchTerm,
            string? role,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            IQueryable<ApplicationUser> query;

            if (!string.IsNullOrWhiteSpace(role))
            {
                var usersInRole = await userManager.GetUsersInRoleAsync(role);
                query = usersInRole.AsQueryable();
            }
            else
            {
                query = userManager.Users;
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(u =>
                    u.Email!.Contains(searchTerm) ||
                    u.FirstName.Contains(searchTerm) ||
                    u.LastName.Contains(searchTerm));
            }

            var totalCount = query.Count();

            var users = query
                .OrderBy(u => u.Email)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (users, totalCount);
        }
        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return await userManager.GetRolesAsync(user);
        }
        public async Task<string> AssignAdminAsync(
            Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByIdAsync(userId.ToString())
                ?? throw new NotFoundException("User not found.");

            if (await userManager.IsInRoleAsync(user, Roles.Admin))
                throw new BadRequestException("User is already an admin.");

            var result = await userManager.AddToRoleAsync(user, Roles.Admin);
            if (!result.Succeeded)
                throw new BadRequestException(
                    string.Join(", ", result.Errors.Select(e => e.Description)));

            return "Admin role assigned successfully.";
        }
        public async Task<string> UnassignAdminAsync(
            Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByIdAsync(userId.ToString())
                ?? throw new NotFoundException("User not found.");

            if (!await userManager.IsInRoleAsync(user, Roles.Admin))
                throw new BadRequestException("User is not an admin.");

            if (currentUserService.UserId == userId)
                throw new BadRequestException("You cannot remove your own admin role.");

            var result = await userManager.RemoveFromRoleAsync(user, Roles.Admin);
            if (!result.Succeeded)
                throw new BadRequestException(
                    string.Join(", ", result.Errors.Select(e => e.Description)));

            return "Admin role removed successfully.";
        }
    }
}
