using IIASA.FotoQuestApi.ImageProcess;
using Microsoft.Extensions.DependencyInjection;

namespace IIASA.FotoQuestApi.Web.DependencyInjection
{
    internal static class ImageExtensions
    {
        public static IServiceCollection AddImageProcess(this IServiceCollection services)
            => services.AddSingleton<IImageHandler, ImageHandler>();
    }
}