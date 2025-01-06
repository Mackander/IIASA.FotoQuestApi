using FotoQuestApi.Configuration;
using FotoQuestApi.ImageProcess;
using NUnit.Framework;
using SixLabors.ImageSharp;
using System.IO;
using Formats = SixLabors.ImageSharp.Formats;

namespace FotoQuestApi.Test.ImageProcess
{
    [TestFixture]
    public class ImageHandlerTest
    {
        private ImageHandler imageHandler;
        private string fileId = string.Empty;
        private string jpegFilePath = string.Empty;

        [SetUp]
        public void Setup()
        {
            fileId = Common.fileId;
            jpegFilePath = Common.jpegFilePath;
        }

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
            var originalImage = Common.GetJpegTestFile(jpegFilePath);
            //act
            var enhancedImage = imageHandler.EnhanceImage(originalImage);
            //assert
            Assert.That(enhancedImage is not null);
        }

    }
}
