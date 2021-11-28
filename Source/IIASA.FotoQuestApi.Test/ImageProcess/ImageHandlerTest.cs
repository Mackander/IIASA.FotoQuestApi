using IIASA.FotoQuestApi.Configuration;
using IIASA.FotoQuestApi.ImageProcess;
using NUnit.Framework;
using SixLabors.ImageSharp;
using System.IO;
using Formats = SixLabors.ImageSharp.Formats;

namespace IIASA.FotoQuestApi.Test.ImageProcess
{
    [TestFixture]
    public class ImageHandlerTest
    {
        private ImageHandler imageHandler;

        [Test]
        public void GetEnhanceImage()
        {
            //arrange
            var imageConfigration = new ImageConfigration
            {
                Brightness = 1.2f,
                Contrast = 1.2f,
                Sharpness = 1.2f
            };
            imageHandler = new ImageHandler(imageConfigration);
            var filePath = Path.GetFullPath(@"./TestImages/JpgTestImage.jpg");
            var originalImage = GetJpegTestFile(filePath);
            //act
            var enhancedImage = imageHandler.EnhanceImage(originalImage);
            //assert
            Assert.IsNotNull(enhancedImage);
        }

        private Image GetJpegTestFile(string filePath)
        {
            using (var inputStream = File.OpenRead(filePath))
            {
                return Image.Load(inputStream, new Formats.Jpeg.JpegDecoder());
            }
        }
    }
}
