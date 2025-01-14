﻿using IIASA.FotoQuestApi.Database;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace IIASA.FotoQuestApi.Web.Diagnostics
{
    public class DbHealthCheckConfigureOptions : IConfigureOptions<HealthCheckServiceOptions>
    {
        private readonly IDatabaseProvider databaseProvider;

        public DbHealthCheckConfigureOptions(IDatabaseProvider databaseProvider)
        {
            this.databaseProvider = databaseProvider;
        }

        public void Configure(HealthCheckServiceOptions options)
        {
            options.Registrations.Add(new HealthCheckRegistration(
                                                                  "Database",
                                                                  new DBHealthCheck(databaseProvider),
                                                                  HealthStatus.Unhealthy,
                                                                  null));
        }
    }
}
