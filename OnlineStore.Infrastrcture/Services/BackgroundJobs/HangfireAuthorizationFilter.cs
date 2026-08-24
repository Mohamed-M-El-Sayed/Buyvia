using Hangfire.Dashboard;
using OnlineStore.Domain.Constants;

namespace OnlineStore.Infrastructure.Services.BackgroundJobs
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            return httpContext.User.Identity?.IsAuthenticated == true
                && httpContext.User.IsInRole(Roles.Admin);
        }
    }
}
