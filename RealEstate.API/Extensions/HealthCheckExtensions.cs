using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RealEstate.API.HealthChecks;

namespace RealEstate.API.Extensions
{
    public static class HealthCheckExtensions
    {
        public static IServiceCollection AddHealthChecks(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddHealthChecks()

                // ── SQL Server ────────────────────────────
                // Checks DB is reachable with a simple SELECT 1
                .AddSqlServer(
                    connectionString: configuration
                        .GetConnectionString("DefaultConnection")!,
                    name: "sql-server",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: ["ready", "db"])

                // ── Disk Storage ──────────────────────────
                .AddCheck<DiskStorageHealthCheck>(
                    name: "disk-storage",
                    failureStatus: HealthStatus.Degraded,
                    tags: ["ready", "storage"])

                // ── Hangfire ──────────────────────────────
                .AddCheck<HangfireHealthCheck>(
                    name: "hangfire",
                    failureStatus: HealthStatus.Degraded,
                    tags: ["ready", "jobs"]);

            return services;
        }
    }
}