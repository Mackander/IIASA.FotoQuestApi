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
            var originalImage = new BaseImage(Image.FromFile(filePath));
            var resizeImage = new ResizeImage(originalImage, size);
            return resizeImage.GetImage();
        }

        public Image EnhanceImage(Image image)
        {
            var baseImage = new BaseImage(image);
            var contrastImage = new ContrastImage(baseImage, 1.5f);
            var brightenImage = new BrightenImage(contrastImage,1.5f);
            return brightenImage.GetImage();
        }

    }
}
