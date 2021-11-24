using IIASA.FotoQuestApi.Configuration;
using IIASA.FotoQuestApi.Database;
using IIASA.FotoQuestApi.FileSystem;
using IIASA.FotoQuestApi.ImageProcess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace IIASA.FotoQuestApi.Web.DependencyInjection
{
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