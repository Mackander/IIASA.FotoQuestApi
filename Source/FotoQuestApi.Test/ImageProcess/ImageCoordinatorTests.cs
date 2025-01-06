using FotoQuestApi.Configuration;
using FotoQuestApi.Database;
using FotoQuestApi.FileSystem;
using FotoQuestApi.Model;
using FotoQuestApi.Model.Exceptions;
using FotoQuestApi.Web;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using SixLabors.ImageSharp;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FotoQuestApi.Test.ImageProcess
{
    [TestFixture]
    public class ImageCoordinatorTests
    {
        private Mock<IDbPersistanceProvider> dbPersistanceProvider;
        private Mock<IFilePersistanceProvider> filePersistanceProvider;
        private string fileId = string.Empty;
        private string jpegFilePath = string.Empty;
        private int minAllowedSize = 30;
        private int maxAllowedSize = 3000;

        [SetUp]
        public void Setup()
        {
            fileId = "639de010-5a33-408f-b549-33711eb15dd0";
            jpegFilePath = Path.GetFullPath($".//TestImages//{fileId}.jpg");
            dbPersistanceProvider = new Mock<IDbPersistanceProvider>();
            filePersistanceProvider = new Mock<IFilePersistanceProvider>();
        }

        [Test]
        public async Task GetImageTest()
        {
            //arrange
            int small = 512;
            var fileData = new Model.FileData()
            {
                Id = fileId
            };

            var originalImage = Common.GetJpegTestFile(jpegFilePath);

            dbPersistanceProvider.Setup(x => x.LoadImageData(fileId)).Returns(Task.Run(() => fileData));
            filePersistanceProvider.Setup(x => x.GetFileAsync(fileData, new Size(small, small))).Returns(Task.Run(() => Common.GetImageAsByteArray(originalImage)));

            var imageConfigration = new ImageConfigration() { Small = small, MinAllowedSize = minAllowedSize, MaxAllowedSize = maxAllowedSize };

            var imageCoordinator = new ImageCoordinator(dbPersistanceProvider.Object, filePersistanceProvider.Object, imageConfigration);
            //act
            var response = await imageCoordinator.GetImage(fileId, small);
            //assert
            Assert.That(response.Length > 0);
        }

        [Test]
        public void GetImageTest_FileNotProvided()
        {
            //arrange
            int small = 512;
            var imageConfigration = new ImageConfigration() { Small = small, MinAllowedSize = minAllowedSize, MaxAllowedSize = maxAllowedSize };
            var imageCoordinator = new ImageCoordinator(dbPersistanceProvider.Object, filePersistanceProvider.Object, imageConfigration);
            //act
            BadRequestException badRequestException = Assert.ThrowsAsync<BadRequestException>(async () => await imageCoordinator.GetImage(string.Empty, small));

            //Assert
            Assert.That(badRequestException.Message, Is.EqualTo("FileId not provided"));
        }

        [Test]
        public void GetImageTest_BadRequest_SmallerThanRange()
        {
            //arrange
            int imageSize = 29;
            var imageConfigration = new ImageConfigration() { MinAllowedSize = minAllowedSize, MaxAllowedSize = maxAllowedSize };
            var imageCoordinator = new ImageCoordinator(dbPersistanceProvider.Object, filePersistanceProvider.Object, imageConfigration);
            //act
            BadRequestException badRequestException = Assert.ThrowsAsync<BadRequestException>(async () => await imageCoordinator.GetImage(fileId, imageSize));

            //Assert
            Assert.That(badRequestException.Message, Is.EqualTo($"Provided Image Size '{imageSize}' should be in Range of {imageConfigration.MinAllowedSize} to {imageConfigration.MaxAllowedSize}"));
        }

        [Test]
        public void GetImageTest_BadRequest_GreaterThanRange()
        {
            //arrange
            int imageSize = 3001;
            var imageConfigration = new ImageConfigration() { MinAllowedSize = minAllowedSize, MaxAllowedSize = maxAllowedSize };
            var imageCoordinator = new ImageCoordinator(dbPersistanceProvider.Object, filePersistanceProvider.Object, imageConfigration);
            //act
            BadRequestException badRequestException = Assert.ThrowsAsync<BadRequestException>(async () => await imageCoordinator.GetImage(fileId, imageSize));

            //Assert
            Assert.That(badRequestException.Message, Is.EqualTo($"Provided Image Size '{imageSize}' should be in Range of {imageConfigration.MinAllowedSize} to {imageConfigration.MaxAllowedSize}"));
        }


        [Test]
        public void PersistImageTest_BadRequest_FileNotProvided()
        {
            //arrange
            var imageCoordinator = new ImageCoordinator(dbPersistanceProvider.Object, filePersistanceProvider.Object, new Configuration.ImageConfigration());

            //act
            BadRequestException badRequestException = Assert.ThrowsAsync<BadRequestException>(async () => await imageCoordinator.PersistImage(new Web.Models.FileUpload()));

            //Assert
            Assert.That(badRequestException.Message, Is.EqualTo("File not provided"));
        }

        [Test]
        public void PersistImageTest_BadRequest_FileTypeNotAllowed()
        {
            //arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(_ => _.FileName).Returns("test.pdf");

            var fileUpload = new Web.Models.FileUpload() { UploadedFile = fileMock.Object };
            var imageConfigration = new ImageConfigration() { ValidImageExtensions = new List<string> { "png", "jpeg", "jpg", "bmp", "gif" } };
            var imageCoordinator = new ImageCoordinator(dbPersistanceProvider.Object, filePersistanceProvider.Object, imageConfigration);

            //act
            BadRequestException badRequestException = Assert.ThrowsAsync<BadRequestException>(async () => await imageCoordinator.PersistImage(fileUpload));

            //Assert
            Assert.That(badRequestException.Message, Is.EqualTo($"Allowed file extensions are : {string.Join(", ", imageConfigration.ValidImageExtensions)}"));
        }

        [Test]
        public async Task PersistImageTest()
        {
            //arrange
            var fileMock = new Mock<IFormFile>();
            var fileData = new Model.FileData { Id = fileId };
            fileMock.Setup(_ => _.FileName).Returns("test.png");
            var fileUpload = new Web.Models.FileUpload() { UploadedFile = fileMock.Object };
            filePersistanceProvider.Setup(x => x.SaveFile(fileUpload)).Returns(Task.Run(() => fileData));

            var imageConfigration = new ImageConfigration() { ValidImageExtensions = new List<string> { "png", "jpeg", "jpg", "bmp", "gif" } };
            var imageCoordinator = new ImageCoordinator(dbPersistanceProvider.Object, filePersistanceProvider.Object, imageConfigration);

            //act
            var response = await imageCoordinator.PersistImage(fileUpload);

            //Assert
            Assert.That(fileId, Is.EqualTo(response.Id));

        }

    }
}
