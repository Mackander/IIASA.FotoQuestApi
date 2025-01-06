using FotoQuestApi.Configuration;
using SixLabors.ImageSharp;

namespace FotoQuestApi.ImageProcess;
public class ImageHandler : IImageHandler
{
    private readonly ImageConfigration imageConfigration;

    public ImageHandler(ImageConfigration imageConfigration)
    {
        this.imageConfigration = imageConfigration;
    }

    public Image GetResizedImage(Image image, Size size)
    {
        var originalImage = new BaseImage(image);
        var resizeImage = new ResizeImage(originalImage, size);
        return resizeImage.GetImage();
    }

    public Image EnhanceImage(Image image)
    {
        var baseImage = new BaseImage(image);
        var contrastImage = new ContrastImage(baseImage, imageConfigration.Contrast);
        var brightenImage = new BrightenImage(contrastImage, imageConfigration.Brightness);
        var sharpendImage = new SharpenImage(brightenImage, imageConfigration.Sharpness);
        return sharpendImage.GetImage();
    }
}