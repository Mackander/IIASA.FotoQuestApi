using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace FotoQuestApi.Web.Diagnostics;

public static class HealthCheckBuilderExtensions
{
    public static IHealthChecksBuilder AddDatabaseHealthOptions<T>(this IHealthChecksBuilder builder) where T : class, IConfigureOptions<HealthCheckServiceOptions>
    {
        builder.Services.AddTransient<IConfigureOptions<HealthCheckServiceOptions>, T>();
        return builder;
    }
}