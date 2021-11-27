using IIASA.FotoQuestApi.Configuration;
using IIASA.FotoQuestApi.FileSystem;
using IIASA.FotoQuestApi.ImageProcess;
using IIASA.FotoQuestApi.Model;
using IIASA.FotoQuestApi.Model.Exceptions;
using Moq;
using NUnit.Framework;
using SixLabors.ImageSharp;
using System.IO;
using System.Threading.Tasks;
using Formats = SixLabors.ImageSharp.Formats;

namespace IIASA.FotoQuestApi.Test.FileSystem
{
    [TestFixture]
    public class FilePersistanceTests
    {
        private FilePersistanceProvider filePersistanceProvider;
        Mock<FilePersistanceConfigration> filePersistanceConfigration;
        Mock<IImageHandler> imageHandler;


        [SetUp]
        public void Setup()
        {
            filePersistanceConfigration = new Mock<FilePersistanceConfigration>();
            imageHandler = new Mock<IImageHandler>();
        }


        [Test]
        public async Task FilePersistanceTests_GetFileAsyncTest()
        {
            //arrange
            var size = new Size(1000, 1000);
            var fileData = GetFileData();

            Image img = GetJpegTestFile(fileData.FilePath);
            imageHandler.SetReturnsDefault(img);
            filePersistanceProvider = new FilePersistanceProvider(fileData.FilePath, filePersistanceConfigration.Object, imageHandler.Object);

            //act
            var response = await filePersistanceProvider.GetFileAsync(fileData, size);

            //assert
            Assert.IsNotNull(response);
        }

        [Test]
        public void FilePersistanceTests_GetFileAsync_WithoutFileTest()
        {
            //arrange
            var size = new Size(1000, 1000);
            var id = System.Guid.NewGuid().ToString();
            var fileData = new FileData
            {
                Id = id
            };
            filePersistanceProvider = new FilePersistanceProvider(fileData.FilePath, filePersistanceConfigration.Object, imageHandler.Object);

            //act
            NotFoundException ex = Assert.ThrowsAsync<NotFoundException>(async () => await filePersistanceProvider.GetFileAsync(fileData, size));

            //Assert
            Assert.That(ex.Message, Is.EqualTo($"Image not found for provided Id : {fileData.Id}"));
        }

        private Image GetJpegTestFile(string filePath)
        {
            using (var inputStream = File.OpenRead(filePath))
            {
                return Image.Load(inputStream, new Formats.Jpeg.JpegDecoder());
            }
        }

        private FileData GetFileData()
        {
            return new FileData
            {
                FilePath = Path.GetFullPath(@"./TestImages/JpgTestImage.jpg"),
                FileName = "JpgTestImage.jpg"
            };
        }
    }
}