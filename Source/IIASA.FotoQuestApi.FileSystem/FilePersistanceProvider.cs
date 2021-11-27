using IIASA.FotoQuestApi.Configuration;
using IIASA.FotoQuestApi.ImageProcess;
using IIASA.FotoQuestApi.Model;
using IIASA.FotoQuestApi.Model.Exceptions;
using IIASA.FotoQuestApi.Web.Models;
using SixLabors.ImageSharp;
using Formats = SixLabors.ImageSharp.Formats;
using System.IO;
using System.Threading.Tasks;

namespace IIASA.FotoQuestApi.FileSystem
{
    public class FilePersistanceProvider : IFilePersistanceProvider
    {
        private readonly string webRootPath;
        private readonly FilePersistanceConfigration filePersistanceConfigration;
        private readonly IImageHandler imageHandler;

        public FilePersistanceProvider(string webRootPath,
                FilePersistanceConfigration filePersistanceConfigration,
                IImageHandler imageHandler)
        {
            this.webRootPath = webRootPath;
            this.filePersistanceConfigration = filePersistanceConfigration;
            this.imageHandler = imageHandler;
        }

        public async Task<byte[]> GetFileAsync(FileData fileData, Size size)
        {
            string filePath = fileData.FilePath;
            if (File.Exists(filePath))
            {
                string fileExtension = Path.GetExtension(fileData.FileName)[1..].ToLower();
                using (var inputStream = File.OpenRead(filePath))
                {
                    using (var image = await Image.LoadAsync(inputStream, GetImageDecoder(fileExtension)))
                    {
                        Image resizedImage = imageHandler.GetResizedImage(image, size);
                        using (var stream = new MemoryStream())
                        {
                            await resizedImage.SaveAsync(stream, GetImageEncoder(fileExtension));
                            return stream.ToArray();
                        }
                    }
                }
            }
            else
            {
                throw new NotFoundException($"Image not found for provided Id : {fileData.Id}");
            }
        }

        public async Task<FileData> SaveFile(FileUpload fileUpload)
        {
            string id = System.Guid.NewGuid().ToString();
            string fileExtension = Path.GetExtension(fileUpload.UploadedFile.FileName);
            string newFileName = $"{id}" + fileExtension;
            string path = webRootPath + $"\\{filePersistanceConfigration.FolderName}\\";
            string filePath = path + newFileName;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                using (var mst = fileUpload.UploadedFile.OpenReadStream())
                {
                    var img = await Image.LoadAsync(mst, GetImageDecoder(fileExtension[1..]));
                    Image enhancedImage = imageHandler.EnhanceImage(img);
                    await enhancedImage.SaveAsync(stream, GetImageEncoder(fileExtension[1..]));
                }
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
            Image image = Image.Load(filePath);
            return new FileData()
            {
                HorizontalResolution = (float)image.Metadata.HorizontalResolution,
                VerticalResolution = (float)image.Metadata.VerticalResolution,
                Height = image.Height,
                Width = image.Width,
            };
        }

        private Formats.IImageDecoder GetImageDecoder(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case "png":
                    return new Formats.Png.PngDecoder();
                case "jpeg":
                case "jpg":
                    return new Formats.Jpeg.JpegDecoder();
                case "bmp":
                    return new Formats.Bmp.BmpDecoder();
                case "gif":
                    return new Formats.Gif.GifDecoder();
                case "tga":
                    return new Formats.Tga.TgaDecoder();
                default:
                    throw new System.Exception($"Decoder not defined for extension : {fileExtension}");
            }
        }

        private Formats.IImageEncoder GetImageEncoder(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case "png":
                    return new Formats.Png.PngEncoder();
                case "jpeg":
                case "jpg":
                    return new Formats.Jpeg.JpegEncoder();
                case "bmp":
                    return new Formats.Bmp.BmpEncoder();
                case "gif":
                    return new Formats.Gif.GifEncoder();
                case "tga":
                    return new Formats.Tga.TgaEncoder();
                default:
                    throw new System.Exception($"Encoder not defined for extension : {fileExtension}");
            }
        }
    }
}