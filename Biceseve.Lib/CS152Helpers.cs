using Biceseve.Lib.Enums;
using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace Biceseve.Lib
{
    public static class CS152Helpers
    {
        public static void SaveRGBArrayAsXYZ(this RgbArray rgbArray, string filePath)
        {
            var delimeter = "\t";
            using var writer = new StreamWriter(filePath, append: false, Encoding.UTF8);

            for (int x = 0; x < rgbArray.Width; x++)
            {
                for (int y = 0; y < rgbArray.Height; y++)
                {
                    writer.WriteLine(ScaleCoordinate(x) + delimeter + ScaleCoordinate(y) + delimeter + ScaleValue(GetMagnitude(rgbArray.data[y][x])));
                }
            }
        }

        public static double ScaleValue(double value, int min = 0, int max = 765, int minScale = -11000, int maxScale = 8500)
        {
            return minScale + (value - min) / (max - min) * (maxScale - minScale);
        }

        public static double GetMagnitude(Color color)
        {
            return GetMagnitude(color.R, color.G, color.B);
        }

        public static double GetMagnitude(int r, int g, int b)
        {
            return (r + g + b) / 3d;
        }

        private static double ScaleCoordinate(int coordinate, CoordinationSteps steps = CoordinationSteps.OneSixth)
        {
            switch (steps)
            {
                case CoordinationSteps.OneSixth:
                    return coordinate * (1 / 6d);
                case CoordinationSteps.OneFifteenth:
                    return coordinate * (1 / 15d);
                default: throw new ArgumentNullException("ScaleCoordinate step argument null");
            }
        }
    }
}
