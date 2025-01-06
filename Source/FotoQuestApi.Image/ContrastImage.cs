using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace FotoQuestApi.ImageProcess;
public class ContrastImage : ImageDecorator
{
    private readonly float contrastValue;

    public ContrastImage(IImage image, float contrastValue = 1.1f) : base(image)
        => this.contrastValue = contrastValue;

    public override Image GetImage()
        => EnhanceImage();


    private Image EnhanceImage()
    {
        Image image = base.GetImage();
        image.Mutate(i => i.Contrast(contrastValue));
        return image;
    }
}