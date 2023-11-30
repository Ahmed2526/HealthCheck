using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.VisualBasic;
using System.Net.Mime;
using System.Text.Json;

namespace HealthCheckAPI
{
    public class CustomHealthCheckOptions : HealthCheckOptions
    {
        public CustomHealthCheckOptions() : base()
        {
            var jsonSerializerOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            ResponseWriter = async (context, healthReport) =>
            {
                context.Response.ContentType =
                MediaTypeNames.Application.Json;
                context.Response.StatusCode = StatusCodes.Status200OK;
                var result = JsonSerializer.Serialize(new
                {
                    checks = healthReport.Entries.Select(e => new
                    {
                        name = e.Key,
                        responseTime =e.Value.Duration.TotalMilliseconds,
                        status = e.Value.Status.ToString(),
                        description = e.Value.Description
                    }),
                    totalStatus = healthReport.Status,
                    totalResponseTime = healthReport.TotalDuration.TotalMilliseconds,
                }, jsonSerializerOptions);
                await context.Response.WriteAsync(result);
            };
        }
    }
}
