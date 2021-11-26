using IIASA.FotoQuestApi.Model;
using IIASA.FotoQuestApi.Web.Models;
using SixLabors.ImageSharp;
using System.Threading.Tasks;

namespace IIASA.FotoQuestApi.FileSystem
{
    public interface IFilePersistanceProvider
    {
        public Task<FileData> SaveFile(FileUpload fileUpload);
        public Task<byte[]> GetFileAsync(FileData fileData, Size size);
    }
}
