using Biceseve.Lib;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Drawing;
using System.IO;

namespace Biceseve.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1) return;

            var file = new FileInfo(args[0]);

            if (!file.Exists) return;

            using var sourceImage = new Bitmap(file.FullName);


            var img = Bitmapper.ConvertToRGBArray(sourceImage);

            using var image = new Image<Rgb24>(sourceImage.Width, sourceImage.Height);

            for (var x = 0; x < sourceImage.Width; x++)
            {
                for(var y = 0; y < sourceImage.Height; y++)
                {
                    var pixel = img[y][x];
                    image[x, y] = new Rgb24(pixel.R, pixel.G, pixel.B);
                }
            }

            using var stream = new FileStream(Path.Combine(file.DirectoryName, "output.jpg"), FileMode.OpenOrCreate, FileAccess.ReadWrite);
            image.SaveAsJpeg(stream);

        }
        
    }
}
