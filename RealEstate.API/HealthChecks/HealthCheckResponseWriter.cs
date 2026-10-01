using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

namespace RealEstate.API.HealthChecks
{
    public static class HealthCheckResponseWriter
    {
        public static async Task WriteResponse(
            HttpContext context,
            HealthReport report)
        {
            context.Response.ContentType = "application/json";

            var response = new
            {
                status = report.Status.ToString(),
                success = report.Status == HealthStatus.Healthy,
                duration = report.TotalDuration.TotalMilliseconds,
                checks = report.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    message = e.Value.Description,
                    duration = e.Value.Duration.TotalMilliseconds,
                    data = e.Value.Data,
                    error = e.Value.Exception?.Message
                })
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true,
                        DefaultIgnoreCondition =
                            System.Text.Json.Serialization
                                .JsonIgnoreCondition.WhenWritingNull
                    }));
        }
    }
}