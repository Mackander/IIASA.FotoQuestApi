using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace IIASA.FotoQuestApi.ImageProcess
{

    public class ModificationDetails
    {
        public Size Size { get; set; }
        public float Contrast { get; set; }
        public float Brightness { get; set; }
        public float Sharpness { get; set; }
    }

    public interface IImage
    {
        public Image GetImage();
    }

    public class BaseImage : IImage
    {
        private readonly Image image;

        public BaseImage(Image image)
        {
            this.image = image;
        }

        public Image GetImage()
        {
            return this.image;
        }
    }

    public abstract class ImageDecorator : IImage
    {
        private readonly IImage image;

        public ImageDecorator(IImage image)
        {
            this.image = image;
        }

        public virtual Image GetImage()
        {
            return this.image.GetImage();
        }
    }

    public class ResizeImage : ImageDecorator
    {
        private readonly Size size;

        public ResizeImage(IImage image, Size size) : base(image)
        {
            this.size = size;
        }

        public override Image GetImage()
        {
            return new Bitmap(base.GetImage(), size);
        }
    }

    public class ContrastImage : ImageDecorator
    {
        private readonly float contrastValue;

        public ContrastImage(IImage image, float contrastValue = 1.0f) : base(image)
        {
            this.contrastValue = contrastValue;
        }

        public override Image GetImage()
        {
            return EnhanceImage();
        }

        private Image EnhanceImage()
        {
            Image image = base.GetImage();

            Bitmap originalImage = new Bitmap(image);
            Bitmap adjustedImage = new Bitmap(image);

            float brightness = 1.0f; // no change in brightness
            float contrast = contrastValue; // twice the contrast
            float gamma = 1.0f; // no change in gamma

            float adjustedBrightness = brightness - 1.0f;
            // create matrix that will brighten and contrast the image
            float[][] ptsArray = {
                                    new float[] {contrast, 0, 0, 0, 0}, // scale red
                                    new float[] {0, contrast, 0, 0, 0}, // scale green
                                    new float[] {0, 0, contrast, 0, 0}, // scale blue
                                    new float[] {0, 0, 0, 1.0f, 0}, // don't scale alpha
                                    new float[] {adjustedBrightness, adjustedBrightness, adjustedBrightness, 0, 1}};

            ImageAttributes imageAttributes = new ImageAttributes();
            imageAttributes.ClearColorMatrix();
            imageAttributes.SetColorMatrix(new ColorMatrix(ptsArray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            imageAttributes.SetGamma(gamma, ColorAdjustType.Bitmap);
            Graphics g = Graphics.FromImage(adjustedImage);
            g.DrawImage(originalImage, new Rectangle(0, 0, adjustedImage.Width, adjustedImage.Height), 0, 0, originalImage.Width, originalImage.Height, GraphicsUnit.Pixel, imageAttributes);
            return (System.Drawing.Image)adjustedImage;
        }
    }

    public class BrightenImage : ImageDecorator
    {
        public BrightenImage(IImage image, float brightnessValue = 1.0f) : base(image) { }

        public override Image GetImage()
        {
            return EnhanceImage();
        }

        private Image EnhanceImage()
        {
            Image image = base.GetImage();

            Bitmap originalImage = new Bitmap(image);
            Bitmap adjustedImage = new Bitmap(image);

            float brightness = 1.0f; // no change in brightness
            float contrast = 1.0f; // twice the contrast
            float gamma = 1.0f; // no change in gamma

            float adjustedBrightness = brightness - 1.0f;
            // create matrix that will brighten and contrast the image
            float[][] ptsArray = {
                                    new float[] {contrast, 0, 0, 0, 0}, // scale red
                                    new float[] {0, contrast, 0, 0, 0}, // scale green
                                    new float[] {0, 0, contrast, 0, 0}, // scale blue
                                    new float[] {0, 0, 0, 1.0f, 0}, // don't scale alpha
                                    new float[] {adjustedBrightness, adjustedBrightness, adjustedBrightness, 0, 1}};

            ImageAttributes imageAttributes = new ImageAttributes();
            imageAttributes.ClearColorMatrix();
            imageAttributes.SetColorMatrix(new ColorMatrix(ptsArray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            imageAttributes.SetGamma(gamma, ColorAdjustType.Bitmap);
            Graphics g = Graphics.FromImage(adjustedImage);
            g.DrawImage(originalImage, new Rectangle(0, 0, adjustedImage.Width, adjustedImage.Height), 0, 0, originalImage.Width, originalImage.Height, GraphicsUnit.Pixel, imageAttributes);
            return (System.Drawing.Image)adjustedImage;
        }
    }

    public class SharpenImage : ImageDecorator
    {
        public SharpenImage(IImage image) : base(image) { }

        public override Image GetImage()
        {
            //return SharpenImage();
            return base.GetImage();
        }

        private Bitmap SharpenImage1() //TODO
        {
            Bitmap image = new Bitmap(base.GetImage());
            Bitmap sharpenImage = (Bitmap)image.Clone();

            int filterWidth = 3;
            int filterHeight = 3;
            int width = image.Width;
            int height = image.Height;

            // Create sharpening filter.
            double[,] filter = new double[filterWidth, filterHeight];
            filter[0, 0] = filter[0, 1] = filter[0, 2] = filter[1, 0] = filter[1, 2] = filter[2, 0] = filter[2, 1] = filter[2, 2] = -1;
            filter[1, 1] = 9;

            double factor = 1.0;
            double bias = 0.0;

            Color[,] result = new Color[image.Width, image.Height];

            // Lock image bits for read/write.
            BitmapData pbits = sharpenImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            // Declare an array to hold the bytes of the bitmap.
            int bytes = pbits.Stride * height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(pbits.Scan0, rgbValues, 0, bytes);

            int rgb;
            // Fill the color array with the new sharpened color values.
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    double red = 0.0, green = 0.0, blue = 0.0;

                    for (int filterX = 0; filterX < filterWidth; filterX++)
                    {
                        for (int filterY = 0; filterY < filterHeight; filterY++)
                        {
                            int imageX = (x - filterWidth / 2 + filterX + width) % width;
                            int imageY = (y - filterHeight / 2 + filterY + height) % height;

                            rgb = imageY * pbits.Stride + 3 * imageX;

                            red += rgbValues[rgb + 2] * filter[filterX, filterY];
                            green += rgbValues[rgb + 1] * filter[filterX, filterY];
                            blue += rgbValues[rgb + 0] * filter[filterX, filterY];
                        }
                        int r = Math.Min(Math.Max((int)(factor * red + bias), 0), 255);
                        int g = Math.Min(Math.Max((int)(factor * green + bias), 0), 255);
                        int b = Math.Min(Math.Max((int)(factor * blue + bias), 0), 255);

                        result[x, y] = Color.FromArgb(r, g, b);
                    }
                }
            }

            // Update the image with the sharpened pixels.
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    rgb = y * pbits.Stride + 3 * x;

                    rgbValues[rgb + 2] = result[x, y].R;
                    rgbValues[rgb + 1] = result[x, y].G;
                    rgbValues[rgb + 0] = result[x, y].B;
                }
            }

            // Copy the RGB values back to the bitmap.
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, pbits.Scan0, bytes);
            // Release image bits.
            sharpenImage.UnlockBits(pbits);

            return sharpenImage;
        }
    }
}
