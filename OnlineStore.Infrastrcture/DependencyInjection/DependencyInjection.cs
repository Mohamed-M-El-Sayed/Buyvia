using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineStore.Application.Contracts;
using OnlineStore.Application.Contracts.Persistence;
using OnlineStore.Application.Contracts.Services.Authentication;
using OnlineStore.Application.Contracts.Services.Caching;
using OnlineStore.Application.Contracts.Services.Email;
using OnlineStore.Application.Contracts.Services.Payment;
using OnlineStore.Application.Contracts.Services.Verification;
using OnlineStore.Domain.Entities.Identity;
using OnlineStore.Infrastructure.Persistence;
using OnlineStore.Infrastructure.Persistence.Repositories;
using OnlineStore.Infrastructure.Persistence.Seed;
using OnlineStore.Infrastructure.Persistence.Seed.SeedData;
using OnlineStore.Infrastructure.Services;
using OnlineStore.Infrastructure.Services.Authentication;
using OnlineStore.Infrastructure.Services.BackgroundJobs;
using OnlineStore.Infrastructure.Services.Caching;
using OnlineStore.Infrastructure.Services.Email;
using OnlineStore.Infrastructure.Services.Payment;
using OnlineStore.Infrastructure.Services.Verification;

namespace OnlineStore.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration, string wwwRootPath)
        {
            services.AddDbContext<OnlineStoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddHangfire(options =>
            {
                options.UseSqlServerStorage(
                    configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddHangfireServer();
            services.AddIdentityCore<ApplicationUser>()
             .AddRoles<IdentityRole<Guid>>()
             .AddEntityFrameworkStores<OnlineStoreDbContext>()
             .AddDefaultTokenProviders();
            services.AddDataProtection();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IFileService>(_ =>
                new FileService(wwwRootPath));
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.Configure<StripeSettings>(configuration.GetSection("Stripe"));
            services.AddScoped<IPaymentService, StripePaymentService>();
            services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.SectionName));
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
            services.Configure<AuthevoOptions>(configuration.GetSection(AuthevoOptions.SectionName));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IBackgroundJobService, HangfireBackgroundJobService>();
            services.AddHttpClient<
                IPhoneVerificationService,
                AuthevoPhoneVerificationService>(
                client =>
                {
                    client.BaseAddress = new Uri("https://api.authevo.dev");
                });
            services.AddStackExchangeRedisOutputCache(options =>
            {
                options.Configuration =
                     configuration["Redis:ConnectionString"];

                options.InstanceName =
                    configuration["Redis:InstanceName"];
            });
            services.AddScoped<ICacheInvalidationService, CacheInvalidationService>();
            services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();
            services.AddScoped<IdentitySeeder>();
            services.AddScoped<CatalogSeed>();
        }
    }
}
