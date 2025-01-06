using FotoQuestApi.Web.DependencyInjection;
using FotoQuestApi.Web.Diagnostics;
using FotoQuestApi.Web.Filters;
using Microsoft.OpenApi.Models;

namespace FotoQuestApi.Web;

public class Startup
{
    private readonly IWebHostEnvironment webHostEnvironment;

    public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        Configuration = configuration;
        this.webHostEnvironment = webHostEnvironment;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers(option => { option.Filters.Add(typeof(ExceptionHandlerFilter)); });

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "FotoQuestApi",
            });
        });
        services.AddHttpContextAccessor();

        services.AddDependencies(Configuration, webHostEnvironment);

        services.AddHealthChecks()
                .AddDatabaseHealthOptions<DbHealthCheckConfigureOptions>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "FotoQuestApi.xml");
        });
        app.UseRouting();

        app.UseAuthorization();

        app.UseCustomRequestLogging();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("health", new XmlHealthCheckOptions());
        });
    }
}