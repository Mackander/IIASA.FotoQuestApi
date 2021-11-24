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
}