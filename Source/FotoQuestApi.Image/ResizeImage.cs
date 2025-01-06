using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace FotoQuestApi.ImageProcess;
public class ResizeImage : ImageDecorator
{
    private readonly Size size;

    public ResizeImage(IImage image, Size size) : base(image)
        => this.size = size;

    public override Image GetImage()
    {
        Image image = base.GetImage();
        image.Mutate(x => x.Resize(size.Width, size.Height));
        return image;
    }
}