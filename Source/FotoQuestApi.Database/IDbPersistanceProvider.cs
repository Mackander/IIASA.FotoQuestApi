using FotoQuestApi.Model;

namespace FotoQuestApi.Database;
public interface IDbPersistanceProvider
{
    public Task<FileData> LoadImageData(string fileId);
    public Task<int> SaveImageData(FileData data);
}

public class DbPersistanceProvider : IDbPersistanceProvider
{
    private readonly IDatabaseProvider databaseProvider;

    public DbPersistanceProvider(IDatabaseProvider databaseProvider)
        => this.databaseProvider = databaseProvider;

    public async Task<FileData> LoadImageData(string fileId)
        => await databaseProvider.LoadImageData(new GetImageDataRequest { Id = fileId });

    public async Task<int> SaveImageData(FileData data)
    {
        var response = await databaseProvider.SaveImageData(new StoreImageDataRequest
        {
            Id = data.Id,
            OriginalName = data.OriginalName,
            DateOfUpload = data.DateOfUpload,
            HorizontalResolution = data.HorizontalResolution,
            VerticalResolution = data.VerticalResolution,
            Height = data.Height,
            Width = data.Width,
            FilePath = data.FilePath,
            FileName = data.FileName,
        });
        if (response <= 0)
        {
            throw new Exception($"Not able to save Data in Database with id {data.Id}");
        }
        return response;
    }
}