using SixLabors.ImageSharp;
using System.IO;

namespace FotoQuestApi.Test;
public static class Common
{
    public static readonly string fileId = "639de010-5a33-408f-b549-33711eb15dd0";
    public static readonly string jpegFilePath = Path.GetFullPath($".//TestImages//{fileId}.jpg");


    public static Image GetJpegTestFile(string filePath)
    {
        using (var inputStream = File.OpenRead(filePath))
        {
            return Image.Load(inputStream);
        }
    }

    public static byte[] GetImageAsByteArray(Image image)
    {
        using (var stream = new MemoryStream())
        {
            image.SaveAsJpeg(stream);
            return stream.ToArray();
        }
    }
}
