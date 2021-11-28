using SixLabors.ImageSharp;
using System.IO;
using Formats = SixLabors.ImageSharp.Formats;

namespace IIASA.FotoQuestApi.Test
{
    public static class Common
    {
        public static Image GetJpegTestFile(string filePath)
        {
            using (var inputStream = File.OpenRead(filePath))
            {
                return Image.Load(inputStream, new Formats.Jpeg.JpegDecoder());
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
}
