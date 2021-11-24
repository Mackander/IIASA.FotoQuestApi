using System.Drawing;

namespace IIASA.FotoQuestApi.ImageProcess
{
    public interface IImageHandler
    {
        public Image GetResizedImage(string filePath, Size size);

        public Image EnhanceImage(Image img);
    }
}
