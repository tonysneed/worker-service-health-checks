using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerServiceHealth
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int counter = 0;
            int degraded = 10;
            int unhealthy = 20;
            while (!stoppingToken.IsCancellationRequested)
            {
                // Set health status
                counter++;
                if (counter < degraded)
                    HealthMonitor.HealthCheckResult = HealthCheckResult.Healthy("healthy");
                if (counter == degraded)
                    HealthMonitor.HealthCheckResult = HealthCheckResult.Degraded("degraded");
                if (counter == unhealthy)
                    HealthMonitor.HealthCheckResult = HealthCheckResult.Unhealthy("unhealthy");

                // Log time and pause
                _logger.LogInformation("Worker running at: {time} {health}",
                    DateTimeOffset.Now, HealthMonitor.HealthCheckResult.Status);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
