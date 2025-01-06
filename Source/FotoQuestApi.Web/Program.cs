namespace FotoQuestApi.Web;

public class Program
{
    public static int Main(string[] args)
    {
        var name = typeof(Program).Assembly.GetName().Name;

        Log.Logger = new LoggerConfiguration()
             .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
             .Enrich.FromLogContext()
             .Enrich.WithMachineName() // This will be containerId for Dockerised application
             .Enrich.WithProperty("Assembly", name)
             .WriteTo.Console()
             .CreateLogger();

        try
        {
            Log.Information("Starting web host");
            CreateHostBuilder(args).Build().Run();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog() // use Serilog Generic Host 
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}