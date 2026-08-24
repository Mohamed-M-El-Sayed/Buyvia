using Hangfire;
using OnlineStore.API.Extensions;
using OnlineStore.API.Middlewares;
using OnlineStore.Application.Contracts.Services.BackgroundJobs;
using OnlineStore.Application.DependancyInjection;
using OnlineStore.Infrastructure.Persistence.Seed;
using OnlineStore.Infrastructure.Services.BackgroundJobs;
using Serilog;
namespace OnlineStore.Infrastructure.DependencyInjection;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddPresentation();

        builder.Services.AddApplicaiton();

        builder.Services.AddInfrastructure(
            builder.Configuration,
            builder.Environment.WebRootPath);

        var app = builder.Build();


        // Database Seeding
        using (var scope = app.Services.CreateScope())
        {
            var databaseSeeder =
                scope.ServiceProvider.GetRequiredService<IDatabaseSeeder>();

            await databaseSeeder.SeedAsync();
        }


        // Swagger
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }


        // Hangfire
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = new[]
            {
                new HangfireAuthorizationFilter()
            }
        });
        using (var scope = app.Services.CreateScope())
        {
            var backgroundJobService =
                scope.ServiceProvider.GetRequiredService<IBackgroundJobService>();

            backgroundJobService.AddOrUpdate<ICleanupPendingOrdersJob>(
                "cleanup-pending-orders",
                job => job.ExecuteAsync(),
                "*/5 * * * *");
        }


        // HTTP Request Pipeline
        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseSerilogRequestLogging();

        app.UseStaticFiles();

        app.UseHttpsRedirection();


        app.UseRateLimiter();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseOutputCache();

        app.MapControllers();

        await app.RunAsync();
    }
}
