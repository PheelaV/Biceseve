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
            if (args.Length != 1) throw new ArgumentException("Please provide file to convert");
            var file = new FileInfo(args[0]);
            if (!file.Exists) throw new ArgumentException("Provided file does not exist");

            using var sourceImage = new Bitmap(file.FullName);

            var rgbArray = sourceImage.ConvertToRGBArray(Lib.Enums.RgbArrayColorMode.monochromatic);

            rgbArray.SaveRgbArrayAsJpgImage(Path.Combine(file.DirectoryName, "output.jpg"));
            rgbArray.SaveRGBArrayAsXYZ(Path.Combine(file.DirectoryName, "output.xyz"));

        }
    }
}
