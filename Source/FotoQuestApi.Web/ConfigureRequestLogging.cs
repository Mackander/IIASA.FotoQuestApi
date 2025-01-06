namespace FotoQuestApi.Web;


public static class ConfigureRequestLogging
{
    public static IApplicationBuilder UseCustomRequestLogging(this IApplicationBuilder app)
    {
        return app.UseSerilogRequestLogging(options => {

            //options.GetLevel = LogEventLevel.Warning
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) => {

                diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"]);
            };
        });
    }
}
