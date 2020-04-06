using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Biceseve.Lib
{
    public class Bitmapper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1814:Prefer jagged arrays over multidimensional", Justification = "<Pending>")]
        // CURRENTLY ONLY TESTED WITH 24bit bmp
        public static Color[][] ConvertToRGBArray(Bitmap bmp)
        {
            //https://stackoverflow.com/a/1563170

            var result = new Color[bmp.Height][];
            BitmapData bData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);

            /* GetBitsPerPixel just does a switch on the PixelFormat and returns the number */
            byte bitsPerPixel = Convert.ToByte(GetBitsPerPixel(bData.PixelFormat));

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
                result[y - 1][x] = Color.FromArgb(data[i + 2], data[i + 1], data[i]);
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
