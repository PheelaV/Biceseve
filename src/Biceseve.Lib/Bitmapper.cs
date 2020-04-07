using Biceseve.Lib.Enums;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Color = System.Drawing.Color;

namespace Biceseve.Lib
{
    public static partial class Bitmapper
    {
        // CURRENTLY ONLY TESTED WITH 24bit bmp
        public static RgbArray ConvertToRGBArray(this Bitmap bmp, MagnitudeRgbConversionMode colorMode = MagnitudeRgbConversionMode.realistic)
        {
            //https://stackoverflow.com/a/1563170

            var result = new Color[bmp.Height][];
            BitmapData bData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);

            /* GetBitsPerPixel just does a switch on the PixelFormat and returns the number */
            byte bitsPerPixel = Convert.ToByte(GetBitsPerPixel(bData.PixelFormat));

            if(bitsPerPixel != 24)
            {
                Console.WriteLine("WARNING, currently tested only with 24bit bmp, might be unstable");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }

            /*the size of the image in bytes */
            int size = bData.Stride * bData.Height;

            /*Allocate buffer for image*/
            byte[] data = new byte[size];

            /*This overload copies data of /size/ into /data/ from location specified (/Scan0/)*/
            System.Runtime.InteropServices.Marshal.Copy(bData.Scan0, data, 0, size);
            var bytesPerPixel = bitsPerPixel / 8;
            for (int i = 0; i < size; i += bytesPerPixel)
            {
                //double magnitude = 1 / 3d * (data[i] + data[i + 1] + data[i + 2]);
                int x = i / bytesPerPixel % bmp.Width;
                int y = bmp.Height - i / bytesPerPixel / bmp.Width;
                if (x == 0) result[y - 1] = new System.Drawing.Color[bmp.Width];

                // bmp format scans starts at bottom left and goes to top right
                // data[i] is the first of 3 bytes of color
                // bmp stores values as Blue, Green, Red
                result[y - 1][x] = GetColor(data[i + 2], data[i + 1], data[i], colorMode);
            }

            /* This override copies the data back into the location specified */
            System.Runtime.InteropServices.Marshal.Copy(data, 0, bData.Scan0, data.Length);

            bmp.UnlockBits(bData);

            return new RgbArray(result);
        }

        public static void SaveRgbArrayAsJpgImage(this RgbArray rgbArray, string filePath)
        {
            var width = rgbArray.Width;
            var height = rgbArray.Height;
            using var image = new Image<Rgb24>(width, height);

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var pixel = rgbArray.data[y][x];
                    image[x, y] = new Rgb24(pixel.R, pixel.G, pixel.B);
                }
            }

            using var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            image.SaveAsJpeg(stream);
        }

        public static Color GetColor(int magnitude, MagnitudeRgbConversionMode conversionMode)
        {
            switch (conversionMode)
            {
                case MagnitudeRgbConversionMode.monochromatic:
                    return Color.FromArgb(magnitude, magnitude, magnitude);
                case MagnitudeRgbConversionMode.scaledHue:
                    throw new NotImplementedException("Future development");
                case MagnitudeRgbConversionMode.realistic:
                default:
                    throw new NotImplementedException("Future development");
            }
        }

        public static Color GetColor(int r, int g, int b, MagnitudeRgbConversionMode conversionMode)
        {
            switch (conversionMode)
            {
                case MagnitudeRgbConversionMode.monochromatic:
                    var magnitude = (int)CS152Helpers.GetMagnitude(r, g, b);
                    return Color.FromArgb(magnitude, magnitude, magnitude);
                case MagnitudeRgbConversionMode.scaledHue:
                    throw new NotImplementedException("Future development");
                case MagnitudeRgbConversionMode.realistic:
                default:
                    return Color.FromArgb(r, g, b);
            }
        }

        private static int GetBitsPerPixel(PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.Format1bppIndexed:
                    return 1;
                case PixelFormat.Format4bppIndexed:
                    return 4;
                case PixelFormat.Format8bppIndexed:
                    return 8;
                case PixelFormat.Format16bppArgb1555:
                case PixelFormat.Format16bppGrayScale:
                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:
                    return 16;
                case PixelFormat.Format24bppRgb:
                    return 24;
                case PixelFormat.Canonical:
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    return 32;
                case PixelFormat.Format48bppRgb:
                    return 48;
                case PixelFormat.Format64bppArgb:
                case PixelFormat.Format64bppPArgb:
                    return 64;
                default:
                    return 0;
            }
        }
        public static double GetMagnitude(Color color)
        {
            return color.R + color.G + color.B;
        }
    }
}
