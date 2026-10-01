using Hangfire;
using Hangfire.Storage;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace RealEstate.API.HealthChecks
{
    public class HangfireHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken ct = default)
        {
            try
            {
                var storage = JobStorage.Current;
                var monitoring = storage.GetMonitoringApi();

                var servers = monitoring.Servers();
                var stats = monitoring.GetStatistics();

                var data = new Dictionary<string, object>
                {
                    { "servers",         servers.Count },
                    { "processingJobs",  stats.Processing },
                    { "failedJobs",      stats.Failed },
                    { "scheduledJobs",   stats.Scheduled },
                    { "succeededJobs",   stats.Succeeded }
                };

                // ── No servers running → Unhealthy ────────
                if (servers.Count == 0)
                    return Task.FromResult(HealthCheckResult.Unhealthy(
                        "لا يوجد Hangfire server يعمل حالياً",
                        data: data));

                // ── Too many failed jobs → Degraded ───────
                if (stats.Failed > 10)
                    return Task.FromResult(HealthCheckResult.Degraded(
                        $"يوجد {stats.Failed} مهمة فاشلة في Hangfire",
                        data: data));

                return Task.FromResult(HealthCheckResult.Healthy(
                    $"Hangfire يعمل بشكل طبيعي — {servers.Count} server نشط",
                    data: data));
            }
            catch (Exception ex)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy(
                    "فشل في فحص Hangfire",
                    ex));
            }
        }
    }
}