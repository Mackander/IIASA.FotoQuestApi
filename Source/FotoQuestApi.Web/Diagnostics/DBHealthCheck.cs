using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FotoQuestApi.Web.Diagnostics;

public class DBHealthCheck : IHealthCheck
{
    private readonly IDatabaseProvider databaseProvider;

    public DBHealthCheck(IDatabaseProvider databaseProvider)
    {
        this.databaseProvider = databaseProvider;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return await databaseProvider.CheckConnection() ? HealthCheckResult.Healthy("Database is Healthy") : HealthCheckResult.Unhealthy("Database is UnHealthy");
    }
}