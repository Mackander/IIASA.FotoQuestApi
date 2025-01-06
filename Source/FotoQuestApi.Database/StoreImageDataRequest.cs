using System.Data;

namespace FotoQuestApi.Database;
public class StoreImageDataRequest : IDataRequest
{
    public string Id { get; set; }
    public string OriginalName { get; set; }
    public DateTime DateOfUpload { get; set; }
    public float HorizontalResolution { get; set; }
    public float VerticalResolution { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public string FilePath { get; set; }
    public string FileName { get; set; }
    public string Command => "uspStoreImageData";
    public CommandType CommandType => CommandType.StoredProcedure;
}