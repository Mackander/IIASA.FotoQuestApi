using IIASA.FotoQuestApi.Model;
using System.Threading.Tasks;

namespace IIASA.FotoQuestApi.Database
{

    public interface IDatabaseProvider
    {
        public void SaveImageData(IDataRequest data);
        Task<FileData> LoadImageData(IDataRequest dataRequest);
    }
}
