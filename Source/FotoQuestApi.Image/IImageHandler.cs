using SixLabors.ImageSharp;

namespace FotoQuestApi.ImageProcess;
public interface IImageHandler
{
    public Image GetResizedImage(Image image, Size size);

    public Image EnhanceImage(Image img);
}