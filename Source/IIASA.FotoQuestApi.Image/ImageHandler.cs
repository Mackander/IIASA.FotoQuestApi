using IIASA.FotoQuestApi.Configuration;
using System.Drawing;

namespace IIASA.FotoQuestApi.ImageProcess
{
    public class ImageHandler : IImageHandler
    {
        private readonly ImageConfigration imageConfigration;

        public ImageHandler(ImageConfigration imageConfigration)
        {
            this.imageConfigration = imageConfigration;
        }

        public Image GetResizedImage(string filePath, Size size)
        {
            var originalImage = new BaseImage(Image.FromFile(filePath));
            var resizeImage = new ResizeImage(originalImage, size);
            return resizeImage.GetImage();
        }

        public Image EnhanceImage(Image image)
        {
            var baseImage = new BaseImage(image);
            var contrastImage = new ContrastImage(baseImage, imageConfigration.Contrast);
            var brightenImage = new BrightenImage(contrastImage, imageConfigration.Brightness);
            return brightenImage.GetImage();
        }
    }
}