namespace FotoQuestApi.Web.DependencyInjection;

internal static class ConfigurationExtensions
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services,
                                                           IConfiguration configuration)
    {
        var imageConfigration = configuration.GetSection(ImageConfigration.ImageOptions).Get<ImageConfigration>();
        var filePersistanceConfigration = configuration.GetSection(FilePersistanceConfigration.FilePersistance).Get<FilePersistanceConfigration>();
        services.AddSingleton(filePersistanceConfigration)
                .AddSingleton(imageConfigration);
        return services;
    }
}
