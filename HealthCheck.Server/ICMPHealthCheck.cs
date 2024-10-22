using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.NetworkInformation;

namespace HealthCheck.Server
{
    public class ICMPHealthCheck : IHealthCheck
    {
        private readonly string Host;
        private readonly int HealthyRoundtripTime;

        public ICMPHealthCheck(string host, int healthyRountripTime)
        {
            Host = host;
            HealthyRoundtripTime = healthyRountripTime;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using var ping = new Ping();
                var reply = await ping.SendPingAsync(Host);
                switch (reply.Status)
                {
                    case IPStatus.Success:
                        var msg = $"ICMP to {Host} took {reply.RoundtripTime} ms.";
                        return (reply.RoundtripTime <= HealthyRoundtripTime) 
                            ? HealthCheckResult.Healthy(msg) 
                            : HealthCheckResult.Degraded(msg);
                    default:
                        return HealthCheckResult.Unhealthy();
                }
            }
            catch(Exception ex)
            {
                var err = $"ICMP failed: {ex.Message}";
                return HealthCheckResult.Unhealthy(err);
            }
        }
    }
}
