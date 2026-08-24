using System.Threading.RateLimiting;

namespace OnlineStore.API.Extensions;

public static class RateLimitingExtensions
{
    public const string AuthStrict = "auth-strict";
    public const string AuthModerate = "auth-moderate";
    public const string Upload = "upload";
    public const string Otp = "otp";

    public static IServiceCollection AddCustomRateLimiting(
        this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.ContentType = "application/json";

                await context.HttpContext.Response.WriteAsync(
                    "{\"message\":\"Too many requests. Please try again later.\"}",
                    cancellationToken);
            };

            // Global limiter
            options.GlobalLimiter =
                PartitionedRateLimiter.Create<HttpContext, string>(
                    httpContext =>
                        RateLimitPartition.GetFixedWindowLimiter(
                            partitionKey: GetPartitionKey(httpContext),
                            factory: _ => new FixedWindowRateLimiterOptions
                            {
                                PermitLimit = 100,
                                Window = TimeSpan.FromMinutes(1),
                                QueueLimit = 0
                            }));

            // Strict authentication endpoints
            // Login, Register, Forgot Password
            options.AddPolicy(
                AuthStrict,
                httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: GetIpPartitionKey(httpContext),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0
                        }));

            // Moderate authentication endpoints
            // Refresh Token, Confirm Email, Reset Password
            options.AddPolicy(
                AuthModerate,
                httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: GetPartitionKey(httpContext),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 20,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0
                        }));

            // File uploads
            options.AddPolicy(
                Upload,
                httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: GetPartitionKey(httpContext),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0
                        }));

            // Phone OTP
            options.AddPolicy(
                Otp,
                httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: GetPartitionKey(httpContext),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 3,
                            Window = TimeSpan.FromMinutes(10),
                            QueueLimit = 0
                        }));
        });

        return services;
    }

    private static string GetPartitionKey(HttpContext httpContext)
    {
        if (httpContext.User.Identity?.IsAuthenticated == true)
        {
            var userId = httpContext.User.FindFirst("sub")?.Value;

            return $"user:{userId ?? "unknown"}";
        }

        return GetIpPartitionKey(httpContext);
    }

    private static string GetIpPartitionKey(HttpContext httpContext)
    {
        return $"ip:{httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"}";
    }
}