using FotoQuestApi.Configuration;
using FotoQuestApi.ImageProcess;
using FotoQuestApi.Model;
using FotoQuestApi.Model.Exceptions;
using FotoQuestApi.Web.Models;
using SixLabors.ImageSharp;
using Formats = SixLabors.ImageSharp.Formats;

namespace FotoQuestApi.FileSystem;
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
                using (var image = await Image.LoadAsync(inputStream, default))
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
                var img = await Image.LoadAsync(mst, default);
                Image enhancedImage = imageHandler.EnhanceImage(img);
                await enhancedImage.SaveAsync(stream, GetImageEncoder(fileExtension[1..]));
            }
            stream.Flush();
        }
        return GetFileMetaData(filePath) with
        {
            Id = id,
            OriginalName = fileUpload.UploadedFile.FileName,
            DateOfUpload = System.DateTime.Now,
            FilePath = filePath,
            FileName = newFileName
        };
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

    private Formats.IImageEncoder GetImageEncoder(string fileExtension)
    =>
        fileExtension.ToLower() switch
        {
            "png" => new Formats.Png.PngEncoder(),
            "jpeg" or "jpg" => new Formats.Jpeg.JpegEncoder(),
            "bmp" => new Formats.Bmp.BmpEncoder(),
            "gif" => new Formats.Gif.GifEncoder(),
            "tga" => new Formats.Tga.TgaEncoder(),
            _ => throw new System.Exception($"Encoder not defined for extension : {fileExtension}"),
        };

}