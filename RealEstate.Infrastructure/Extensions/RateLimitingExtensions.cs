using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Settings;
using System.Net;
using System.Text.Json;
using System.Threading.RateLimiting;
using RealEstate.Shared.Common;

namespace RealEstate.Infrastructure.Extensions
{
    public static class RateLimitingExtensions
    {
        public static IServiceCollection AddRateLimiting(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var settings = configuration
                .GetSection("RateLimiting")
                .Get<RateLimitingSettings>()
                ?? new RateLimitingSettings();

            services.AddRateLimiter(options =>
            {
                // ── Global rejection handler ──────────────
                // Called when any limit is exceeded
                options.OnRejected = async (context, ct) =>
                {
                    context.HttpContext.Response.StatusCode =
                        (int)HttpStatusCode.TooManyRequests;

                    context.HttpContext.Response.ContentType =
                        "application/json";

                    // ── Add Retry-After header ────────────
                    if (context.Lease.TryGetMetadata(
                        MetadataName.RetryAfter,
                        out var retryAfter))
                    {
                        context.HttpContext.Response.Headers
                            .RetryAfter = ((int)retryAfter
                                .TotalSeconds)
                                .ToString();
                    }

                    var response = ApiResponse<object>.Fail(
                        "لقد تجاوزت الحد المسموح به من الطلبات، يرجى المحاولة لاحقاً");

                    await context.HttpContext.Response.WriteAsync(
                        JsonSerializer.Serialize(response,
                            new JsonSerializerOptions
                            {
                                PropertyNamingPolicy =
                                    JsonNamingPolicy.CamelCase
                            }), ct);
                };

                // ── Login — strict, by IP ─────────────────
                // Protects against brute force password attacks
                // 5 attempts per minute — after that locked out
                options.AddFixedWindowLimiter(
                    RateLimitPolicies.Login,
                    opt =>
                    {
                        opt.PermitLimit = settings.Auth.Login.PermitLimit;
                        opt.Window = TimeSpan.FromSeconds(
                            settings.Auth.Login.WindowSeconds);
                        opt.QueueProcessingOrder =
                            QueueProcessingOrder.OldestFirst;
                        opt.QueueLimit = 0; // no queue — reject immediately
                        opt.AutoReplenishment = true;
                    });

                // ── Reset Password — very strict, by IP ───
                // 3 attempts per 15 minutes
                // Prevents email enumeration + token flooding
                options.AddFixedWindowLimiter(
                    RateLimitPolicies.ResetPassword,
                    opt =>
                    {
                        opt.PermitLimit = settings.Auth.ResetPassword.PermitLimit;
                        opt.Window = TimeSpan.FromSeconds(
                            settings.Auth.ResetPassword.WindowSeconds);
                        opt.QueueProcessingOrder =
                            QueueProcessingOrder.OldestFirst;
                        opt.QueueLimit = 0;
                        opt.AutoReplenishment = true;
                    });

                // ── Change Password — strict, by IP ───────
                options.AddFixedWindowLimiter(
                    RateLimitPolicies.ChangePassword,
                    opt =>
                    {
                        opt.PermitLimit = settings.Auth.ChangePassword.PermitLimit;
                        opt.Window = TimeSpan.FromSeconds(
                            settings.Auth.ChangePassword.WindowSeconds);
                        opt.QueueProcessingOrder =
                            QueueProcessingOrder.OldestFirst;
                        opt.QueueLimit = 0;
                        opt.AutoReplenishment = true;
                    });

                // ── Accept Invitation — moderate, by IP ───
                options.AddFixedWindowLimiter(
                    RateLimitPolicies.AcceptInvitation,
                    opt =>
                    {
                        opt.PermitLimit = settings.Auth.AcceptInvitation.PermitLimit;
                        opt.Window = TimeSpan.FromSeconds(
                            settings.Auth.AcceptInvitation.WindowSeconds);
                        opt.QueueProcessingOrder =
                            QueueProcessingOrder.OldestFirst;
                        opt.QueueLimit = 0;
                        opt.AutoReplenishment = true;
                    });

                // ── General API — token bucket by CompanyId ──
                // Authenticated endpoints rate limited per company
                // not per IP — fairer for companies behind shared IPs
                // Token bucket = smoother than fixed window
                // allows short bursts then throttles
                options.AddPolicy(
                    RateLimitPolicies.General,
                    httpContext =>
                    {
                        // ── Use CompanyId if authenticated ─
                        var companyId = httpContext.User
                            .FindFirst("CompanyId")?.Value;

                        // ── Fall back to IP if not authenticated
                        var partitionKey = !string.IsNullOrEmpty(companyId)
                            ? $"company_{companyId}"
                            : $"ip_{httpContext.Connection.RemoteIpAddress}";

                        return RateLimitPartition.GetTokenBucketLimiter(
                            partitionKey,
                            _ => new TokenBucketRateLimiterOptions
                            {
                                TokenLimit = settings.General.PermitLimit,
                                ReplenishmentPeriod = TimeSpan.FromSeconds(
                                    settings.General.WindowSeconds),
                                TokensPerPeriod = settings.General.PermitLimit,
                                AutoReplenishment = true,
                                QueueProcessingOrder =
                                    QueueProcessingOrder.OldestFirst,
                                QueueLimit = 0
                            });
                    });
            });

            return services;
        }
    }
}