using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace RealEstate.API.HealthChecks
{
    public class DiskStorageHealthCheck : IHealthCheck
    {
        private readonly IWebHostEnvironment _env;
        private const long MinFreeBytes = 500 * 1024 * 1024; // 500MB minimum

        public DiskStorageHealthCheck(IWebHostEnvironment env)
            => _env = env;

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken ct = default)
        {
            try
            {
                var uploadsPath = Path.Combine(
                    _env.WebRootPath ??
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
                    "uploads");

                // ── Get drive info for uploads path ───────
                var drive = new DriveInfo(
                    Path.GetPathRoot(uploadsPath)
                    ?? Directory.GetCurrentDirectory());

                var freeBytes = drive.AvailableFreeSpace;
                var totalBytes = drive.TotalSize;
                var freePercent = (double)freeBytes / totalBytes * 100;

                var data = new Dictionary<string, object>
                {
                    { "drive",        drive.Name },
                    { "freeSpaceGB",  Math.Round(freeBytes / 1024.0 / 1024.0 / 1024.0, 2) },
                    { "totalSpaceGB", Math.Round(totalBytes / 1024.0 / 1024.0 / 1024.0, 2) },
                    { "freePercent",  Math.Round(freePercent, 1) }
                };

                // ── Less than 500MB free → Unhealthy ──────
                if (freeBytes < MinFreeBytes)
                    return Task.FromResult(HealthCheckResult.Unhealthy(
                        $"مساحة التخزين منخفضة جداً: {Math.Round(freeBytes / 1024.0 / 1024.0, 0)}MB متبقية",
                        data: data));

                // ── Less than 10% free → Degraded ─────────
                if (freePercent < 10)
                    return Task.FromResult(HealthCheckResult.Degraded(
                        $"مساحة التخزين منخفضة: {Math.Round(freePercent, 1)}% متبقية",
                        data: data));

                return Task.FromResult(HealthCheckResult.Healthy(
                    "مساحة التخزين كافية",
                    data: data));
            }
            catch (Exception ex)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy(
                    "فشل في فحص مساحة التخزين",
                    ex));
            }
        }
    }
}