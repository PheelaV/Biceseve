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
            var utf8WithoutBOM = new UTF8Encoding(false);
            using var writer = new StreamWriter(filePath, append: false, utf8WithoutBOM);

            for (int x = 0; x < rgbArray.Width; x++)
            {
                for (int y = 0; y < rgbArray.Height; y++)
                {
                    writer.WriteLine(ScaleCoordinate(x) + delimeter + ScaleCoordinate(y) + delimeter + ScaleValue(GetMagnitude(rgbArray.data[rgbArray.Height - y - 1][x])));
                }
            }
        }

        public static float ScaleValue(float value, float min = 0, float max = 255, float minScale = -11000, float maxScale = 8500)
        {
            return minScale + (value - min) / (max - min) * (maxScale - minScale);
        }

        public static float GetMagnitude(Color color)
        {
            return GetMagnitude(color.R, color.G, color.B);
        }

        public static float GetMagnitude(int r, int g, int b)
        {
            return (r + g + b) / 3f;
        }

        private static float ScaleCoordinate(int coordinate, CoordinationSteps steps = CoordinationSteps.OneSixth)
        {
            switch (steps)
            {
                case CoordinationSteps.OneSixth:
                    return coordinate * (1 / 6f);
                case CoordinationSteps.OneFifteenth:
                    return coordinate * (1 / 15f);
                default: throw new ArgumentNullException("ScaleCoordinate step argument null");
            }
        }
    }
}
