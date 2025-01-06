using SixLabors.ImageSharp;

namespace FotoQuestApi.ImageProcess;
public abstract class ImageDecorator : IImage
{
    private readonly IImage image;

    public ImageDecorator(IImage image)
        => this.image = image;


    public virtual Image GetImage()
    {
        return this.image.GetImage();
    }
}