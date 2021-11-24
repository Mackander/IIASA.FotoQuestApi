using IIASA.FotoQuestApi.Model;
using IIASA.FotoQuestApi.Web.Models;
using System.Threading.Tasks;

namespace IIASA.FotoQuestApi.Web
{
    public interface IImageCoordinator
    {
        public Task<FilePersistanceSuccessResponse> PersistImage(FileUpload fileUpload);
        public Task<byte[]> GetImage(string fileId, int imageSize);
    }
}
