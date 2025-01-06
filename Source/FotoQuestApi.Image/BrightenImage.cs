using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace FotoQuestApi.ImageProcess;
public class BrightenImage : ImageDecorator
{
    private readonly float brightnessValue;

    public BrightenImage(IImage image, float brightnessValue = 1.1f) : base(image)
        => this.brightnessValue = brightnessValue;


    public override Image GetImage()
        => EnhanceImage();

    private Image EnhanceImage()
    {
        Image image = base.GetImage();
        image.Mutate(i => i.Brightness(brightnessValue));
        return image;
    }
}