using FotoQuestApi.Model;

namespace FotoQuestApi.Database;

public interface IDatabaseProvider
{
    public Task<int> SaveImageData(IDataRequest data);
    Task<FileData> LoadImageData(IDataRequest dataRequest);
    public Task<bool> CheckConnection();
}