namespace FotoQuestApi.Model;
public record FileData
{
    public string Id { get; init; }
    public string OriginalName { get; init; }
    public DateTime DateOfUpload { get; init; }
    public float HorizontalResolution { get; init; }
    public float VerticalResolution { get; init; }
    public int Height { get; init; }
    public int Width { get; init; }
    public string FilePath { get; init; }
    public string FileName { get; init; }

}