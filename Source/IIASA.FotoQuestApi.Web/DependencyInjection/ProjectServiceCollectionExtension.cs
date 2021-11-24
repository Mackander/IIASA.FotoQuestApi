using IIASA.FotoQuestApi.Configuration;
using IIASA.FotoQuestApi.Database;
using IIASA.FotoQuestApi.FileSystem;
using IIASA.FotoQuestApi.ImageProcess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IIASA.FotoQuestApi.Web.DependencyInjection
{
    internal static class ProjectServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services,
                                                              IConfiguration configuration,
                                                              IWebHostEnvironment webHostEnvironment)
            => services.AddConfiguration(configuration)
                       .AddImageProcess()
                       .AddStorage(webHostEnvironment)
                       .AddSingleton<IImageCoordinator, ImageCoordinator>();
    }

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

    internal static class ImageExtensions
    {
        public static IServiceCollection AddImageProcess(this IServiceCollection services)
            => services.AddSingleton<IImageHandler, ImageHandler>();
    }

    internal static class StorageExtensions
    {
        public static IServiceCollection AddStorage(this IServiceCollection services, IWebHostEnvironment webHostEnvironment)
            => services.AddSingleton<IDatabaseProvider, DatabaseProvider>()
                        .AddSingleton<IDbPersistanceProvider, DbPersistanceProvider>()
                        .AddSingleton<IFilePersistanceProvider>(provider => ActivatorUtilities.CreateInstance<FilePersistanceProvider>(provider,
                                                                                                                                        webHostEnvironment.WebRootPath,
                                                                                                                                        provider.GetService<FilePersistanceConfigration>(),
                                                                                                                                        provider.GetService<IImageHandler>()));
    }
}