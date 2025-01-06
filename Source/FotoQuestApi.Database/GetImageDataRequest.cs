using System.Data;

namespace FotoQuestApi.Database;

public class GetImageDataRequest : IDataRequest
{
    public string Id { get; set; }
    public string Command => "uspFetchImageData";
    public CommandType CommandType => CommandType.StoredProcedure;
}