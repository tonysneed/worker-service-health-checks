# Worker Service Health Checks

Example of using a worker service with health checks

1. Create a .NET Core Web API
   - Remove **Controllers** folder
   - Remove `services.AddControllers()` from `Startup.ConfigureServices`.
2. Add `Worker` class.
    ```csharp
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
    ```
3. Update `Startup.ConfigureServices`.
   - Add `services.AddHostedService<Worker>()`.
4. Update `Startup.Configure`.
   - Remove all code from the method.
5. Add `HealthMonitor`.
    ```csharp
    public static class HealthMonitor
    {
        public static HealthCheckResult HealthCheckResult { get; set; }
    }
    ```
6. Update `Worker` to set health check status.
   - After 5 iterations set to `Degraded`.
   - After 10 iterations set to `Unhealthy`.
7. Add health check to `Startup.ConfigureServices`.
    ```csharp
    services.AddHealthChecks()
        .AddCheck<WorkerHealthCheck>("health_check");
    ```
8. Add health check endpoint to `Startup.Configure`.
    ```csharp
    app.UseRouting();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapHealthChecks("/health");
    });
    ```
9. Run and test.
  - `dotnet run`
  - http://localhost:5000/health