using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OnlineStore.Application.Common.Behaviors;
using OnlineStore.Application.Common.Resolvers;
using OnlineStore.Application.Contracts.Services.BackgroundJobs;
using OnlineStore.Application.Features.Orders.Jobs;
using OnlineStore.Application.Features.ProductVariants.Jobs;

namespace OnlineStore.Application.DependancyInjection
{
    public static class DpendancyInjection
    {
        public static void AddApplicaiton(this IServiceCollection services)
        {

            var applicationAssembly = typeof(DpendancyInjection).Assembly;
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(applicationAssembly));
            services.AddAutoMapper(applicationAssembly);
            services.AddValidatorsFromAssembly(applicationAssembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheInvalidationBehavior<,>));
            // services.AddScoped<IDiscountService, DiscountService>();
            // IServiceCollection serviceCollection = services.AddScoped<IPricingService, PricingService>();
            // services.AddScoped<ICouponService, CouponService>();
            // services.AddScoped<ICheckoutCalculator, CheckoutCalculator>();
            services.AddTransient<ImageUrlConverter>();
            services.AddScoped<ICleanupPendingOrdersJob, CleanupPendingOrdersJob>();
            services.AddScoped<ILowStockCheckJob, LowStockCheckJob>();
        }
    }
}
