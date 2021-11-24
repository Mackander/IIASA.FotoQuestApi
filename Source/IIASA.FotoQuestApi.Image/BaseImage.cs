using System.Drawing;

namespace IIASA.FotoQuestApi.ImageProcess
{
    public class BaseImage : IImage
    {
        private readonly Image image;

        public BaseImage(Image image)
        {
            this.image = image;
        }

        public Image GetImage()
        {
            return this.image;
        }
    }
}
