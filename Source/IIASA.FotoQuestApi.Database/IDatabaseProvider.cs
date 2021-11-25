using IIASA.FotoQuestApi.Model;

namespace IIASA.FotoQuestApi.Database
{

    public interface IDatabaseProvider
    {
        public void SaveImageData(IDataRequest data);
        FileData LoadImageData(IDataRequest dataRequest);
    }
}
