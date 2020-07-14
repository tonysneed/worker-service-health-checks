using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WorkerServiceHealth
{
    public static class HealthMonitor
    {
        public static HealthCheckResult HealthCheckResult { get; set; }
    }
}
