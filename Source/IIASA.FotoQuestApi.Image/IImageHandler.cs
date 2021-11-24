using System.Drawing;

namespace IIASA.FotoQuestApi.ImageProcess
{
    public interface IImageHandler
    {
        public Image GetResizedImage(string filePath, Size size);

        public Image EnhanceImage(Image img);
    }

    public class ImageHandler : IImageHandler
    {
        public Image GetResizedImage(string filePath, Size size)
        {
            Image image = Image.FromFile(filePath);
            return ResizeImage(image, size);
        }

        public Image EnhanceImage(Image image)
        {
            var baseImage = new BaseImage(image);
            var contrastImage = new ContrastImage(baseImage);
            var brightenImage = new BrightenImage(contrastImage);
            return brightenImage.GetImage();
        }

        private Image ResizeImage(Image imgToResize, Size size) => new Bitmap(imgToResize, size);
    }
}
