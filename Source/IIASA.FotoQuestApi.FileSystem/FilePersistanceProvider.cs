using IIASA.FotoQuestApi.Configuration;
using IIASA.FotoQuestApi.ImageProcess;
using IIASA.FotoQuestApi.Model;
using IIASA.FotoQuestApi.Model.Exceptions;
using IIASA.FotoQuestApi.Web.Models;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace IIASA.FotoQuestApi.FileSystem
{
    public class FilePersistanceProvider : IFilePersistanceProvider
    {
        private readonly string webRootPath;
        private readonly FilePersistanceConfigration filePersistanceConfigration;
        private readonly IImageHandler imageHandler;

        public FilePersistanceProvider(string webHostEnvironment,
                FilePersistanceConfigration filePersistanceConfigration,
                IImageHandler imageHandler)
        {
            this.webRootPath = webHostEnvironment;
            this.filePersistanceConfigration = filePersistanceConfigration;
            this.imageHandler = imageHandler;
        }

        public async Task<byte[]> GetFileAsync(string fileId, Size size)
        {
            string path = webRootPath + $"\\{filePersistanceConfigration.FolderName}\\";
            string filePath = path + fileId + ".png";

            if (File.Exists(filePath))
            {
                Image resizedImage = imageHandler.GetResizedImage(filePath, size);
                return (byte[])(new ImageConverter()).ConvertTo(resizedImage, typeof(byte[]));
            }
            else
            {
                throw new NotFoundException($"Image not found for provided Id {fileId}");
            }
        }

        public async Task<FileData> SaveFile(FileUpload fileUpload)
        {
            string id = System.Guid.NewGuid().ToString();
            string newFileName = $"{id}" + Path.GetExtension(fileUpload.UploadedFile.FileName);
            string path = webRootPath + $"\\{filePersistanceConfigration.FolderName}\\";
            string filePath = path + newFileName;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                var memoryStream = new MemoryStream();
                await fileUpload.UploadedFile.CopyToAsync(memoryStream);
                Image i = imageHandler.EnhanceImage(Image.FromStream(memoryStream));
                i.Save(stream, ImageFormat.Png);
                memoryStream.Flush();
                stream.Flush();
            }
            var fileMetaData = GetFileMetaData(filePath);

            fileMetaData.Id = id;
            fileMetaData.OriginalName = fileUpload.UploadedFile.FileName;
            fileMetaData.DateOfUpload = System.DateTime.Now;
            fileMetaData.FilePath = filePath;
            fileMetaData.FileName = newFileName;
            return fileMetaData;
           
        }

        private FileData GetFileMetaData(string filePath)
        {
            Image image = Image.FromFile(filePath);
            return new FileData()
            {
                HorizontalResolution = image.HorizontalResolution,
                VerticalResolution = image.VerticalResolution,
                Height = image.Height,
                Width = image.Width,
            };
        }
    }
}