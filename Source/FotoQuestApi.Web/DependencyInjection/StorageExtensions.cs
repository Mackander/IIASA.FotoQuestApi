namespace FotoQuestApi.Web.DependencyInjection;

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
