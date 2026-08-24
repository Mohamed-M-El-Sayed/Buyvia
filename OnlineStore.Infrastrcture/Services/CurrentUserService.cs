using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OnlineStore.Application.Contracts;
using OnlineStore.Domain.Constants;
using OnlineStore.Domain.Entities.Identity;

namespace OnlineStore.Infrastructure.Services
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor,
        UserManager<ApplicationUser> userManager) : ICurrentUserService
    {


        public Guid? UserId
        {
            get
            {
                var value = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return Guid.TryParse(value, out var guid) ? guid : null;
            }
        }
        public bool IsAdmin => httpContextAccessor?.HttpContext?.User.IsInRole(Roles.Admin) ?? false;



        public async Task<string> GetFullNameAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            return user is null ? "Unknown" : $"{user.FirstName} {user.LastName}".Trim();
        }
        public string FullName
        {
            get
            {
                var firstName = httpContextAccessor.HttpContext?.User?.FindFirst("firstName")?.Value;
                var lastName = httpContextAccessor.HttpContext?.User?.FindFirst("lastName")?.Value;
                return $"{firstName} {lastName}".Trim();
            }
        }
    }
}
