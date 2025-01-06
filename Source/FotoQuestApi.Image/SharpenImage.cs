using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace FotoQuestApi.ImageProcess;
public class SharpenImage : ImageDecorator
{
    private readonly float sharpnessValue;

    public SharpenImage(IImage image, float sharpnessValue) : base(image)
        => this.sharpnessValue = sharpnessValue;

    public override Image GetImage()
    {
        Image image = base.GetImage();
        image.Mutate(i => i.GaussianSharpen(sharpnessValue));
        return image;
    }
}