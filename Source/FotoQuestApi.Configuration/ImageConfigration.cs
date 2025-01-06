namespace FotoQuestApi.Configuration;

public class ImageConfigration
{
    public const string ImageOptions = "ImageOptions";
    public int Thumbnail { get; set; }
    public int Small { get; set; }
    public int Large { get; set; }
    public int MinAllowedSize { get; set; }
    public int MaxAllowedSize { get; set; }
    public List<string> ValidImageExtensions { get; set; }
    public float Brightness { get; set; }
    public float Contrast { get; set; }
    public float Sharpness { get; set; }
}