using FotoQuestApi.Model;
using FotoQuestApi.Web.Models;
using SixLabors.ImageSharp;

namespace FotoQuestApi.FileSystem;
public interface IFilePersistanceProvider
{
    public Task<FileData> SaveFile(FileUpload fileUpload);
    public Task<byte[]> GetFileAsync(FileData fileData, Size size);
}