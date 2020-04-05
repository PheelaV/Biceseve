using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Biceseve
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1) return;

            var file = new FileInfo(args[0]);

            if (!file.Exists) return;

            using var sourceImage = new Bitmap(file.FullName);
            //var data = ImageToByteArr(image);

            var img = ConvertToRGBArray(sourceImage);

            using var image = new Image<Rgb24>(sourceImage.Width, sourceImage.Height);

            //var frame = new ImageFrame<Rgb24>(new Configuration(), image.Width, image.Height, image.Metadata);

            //var a = new ImageFrame<Rgb24>(;

            Console.WriteLine();
        }

        //public static byte[] ImageToByteArr(SixLabors.ImageSharp.Image img)
        //{
        //    using var ms = new MemoryStream();
        //    img.Save(ms, ImageFormat.Bmp);

        //    return ms.ToArray();
        //}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1814:Prefer jagged arrays over multidimensional", Justification = "<Pending>")]
        public static System.Drawing.Color[][]ConvertToRGBArray(Bitmap bmp)
        {
            //https://stackoverflow.com/a/1563170

            var result = new System.Drawing.Color[bmp.Height][];
            BitmapData bData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);

            /* GetBitsPerPixel just does a switch on the PixelFormat and returns the number */
            byte bitsPerPixel = Convert.ToByte(GetBitsPerPixel(bData.PixelFormat));
            var format = bData.PixelFormat;

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
                result[y - 1][x] = System.Drawing.Color.FromArgb(data[i], data[i + 1], data[i + 2]);
            }

            /* This override copies the data back into the location specified */
            System.Runtime.InteropServices.Marshal.Copy(data, 0, bData.Scan0, data.Length);

            bmp.UnlockBits(bData);

            return result;
        }

        public static int GetBitsPerPixel(PixelFormat format)
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
    }
}
