using SixLabors.ImageSharp;

namespace IIASA.FotoQuestApi.ImageProcess;
public interface IImageHandler
{
    public Image GetResizedImage(Image image, Size size);

    public Image EnhanceImage(Image img);
}