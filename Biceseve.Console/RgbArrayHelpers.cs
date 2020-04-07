using Biceseve.Lib;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;

namespace Biceseve.ConsoleApp
{
    public static class RgbArrayHelpers
    {
        private static void SaveRGBArrayAsImage(RgbArray rgbArray, DirectoryInfo outputDir)
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

            using var stream = new FileStream(Path.Combine(outputDir.FullName, "output.jpg"), FileMode.OpenOrCreate, FileAccess.ReadWrite);
            image.SaveAsJpeg(stream);
        }
    }
}
