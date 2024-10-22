using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Net.Mime;

namespace HealthCheck.Server
{
    public class CustomHealthCheckOptions : HealthCheckOptions
    {
        public CustomHealthCheckOptions() : base()
        {
            var jsonSerializerOptions = new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            };

            ResponseWriter = async (c, r) =>
            {
                c.Response.ContentType = MediaTypeNames.Application.Json;

                // this status code could be improved using the total status of the checks
                c.Response.StatusCode = StatusCodes.Status200OK;

                var result = System.Text.Json.JsonSerializer.Serialize(new
                {
                    checks = r.Entries.Select(e => new
                    {
                        name = e.Key,
                        responseTime = e.Value.Duration.TotalMilliseconds,
                        status = e.Value.Status.ToString(),
                        description = e.Value.Description,
                    }),
                    totalStatus = r.Status,
                    totalResponseTime = r.TotalDuration.TotalMilliseconds,
                }, jsonSerializerOptions);
                await c.Response.WriteAsync(result);
            };
        }
    }
}
