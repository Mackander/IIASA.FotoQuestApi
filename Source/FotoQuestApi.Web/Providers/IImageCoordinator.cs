namespace FotoQuestApi.Web;

public interface IImageCoordinator
{
    public Task<FilePersistanceSuccessResponse> PersistImage(FileUpload fileUpload);
    public Task<byte[]> GetImage(string fileId, int imageSize);
}