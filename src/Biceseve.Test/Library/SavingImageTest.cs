using Biceseve.Lib;
using SixLabors.ImageSharp.ColorSpaces;
using System;
using System.Drawing;
using Xunit;

namespace Biceseve.Test
{
    public class SavingImageTest
    {

        private RgbArray GetTestRgbArray(int x = 256, int y = 256)
        {
            var rgbArray = new RgbArray(new Color[x][]);

            for (int i = 0; i < x; i++)
            {
                rgbArray.data[i] = new Color[y];
                for (int j = 0; j < y; j++)
                {
                    var magnitude = (int)((i / (double)x + j / (double)y) * 128);
                    rgbArray.data[i][j] = Color.FromArgb(magnitude, magnitude, magnitude);
                }
            }
            return rgbArray;
        }

        [Fact]
        public void SaveRgbArrayAsImage()
        {
            var rgbArray = GetTestRgbArray(3,3);

            rgbArray.SaveRgbArrayAsBmpImage("C:\\Users\\filip\\source\\repos\\Biceseve\\data\\testData.bmp");
        }

        [Fact]
        public void SaveRgbArrayAsXyz()
        {
            var rgbArray = GetTestRgbArray(3,3);

            rgbArray.SaveRGBArrayAsXYZ("C:\\Users\\filip\\source\\repos\\Biceseve\\data\\tesatData.xyz");
        }
    }
}
