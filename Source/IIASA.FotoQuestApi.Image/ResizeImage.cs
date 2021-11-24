using System.Drawing;

namespace IIASA.FotoQuestApi.ImageProcess
{
    public class ResizeImage : ImageDecorator
    {
        private readonly Size size;

        public ResizeImage(IImage image, Size size) : base(image)
        {
            this.size = size;
        }

        public override Image GetImage()
        {
            return new Bitmap(base.GetImage(), size);
        }
    }
}
