using Biceseve.Lib;
using Biceseve.Lib.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Biceseve.ConsoleApp
{
    public static class XyzHelper
    {
        public const int MagnitudeMin = 0;
        public const int MagnitudeMax = 0;
        internal static void WriteXYZFormat(string filePath)
        {
            var file = IoHelpers.GetFile(filePath);

            using var sourceImage = new Bitmap(file.FullName);

            var rgbArray = sourceImage.ConvertToRGBArray(Lib.Enums.MagnitudeRgbConversionMode.monochromatic);

            rgbArray.SaveRgbArrayAsJpgImage(Path.Combine(file.DirectoryName, "output.jpg"));
            rgbArray.SaveRGBArrayAsXYZ(Path.Combine(file.DirectoryName, $"{file.Name}-output.xyz"));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>")]
        internal static Dictionary<double, List<Tuple<double, double>>> ReadXYZFormat(string filePath)
        {
            var file = IoHelpers.GetFile(filePath);

            var data = new Dictionary<double, List<Tuple<double, double>>>();

            using var sr = new StreamReader(file.FullName);
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine()?.Split('\t');

                (double latitude, double longitude, double altitude) = (double.Parse(line[0]), double.Parse(line[1]), double.Parse(line[2]));

                if (data.ContainsKey(latitude))
                {
                    data[latitude] = new List<Tuple<double, double>>() { new Tuple<double, double>(longitude, altitude) };
                }
            }

            return data;

        }

        public static RgbArray ConvertToRgbArray(Dictionary<double, List<Tuple<double, double>>> xyzData, MagnitudeRgbConversionMode conversionMode = MagnitudeRgbConversionMode.monochromatic)
        {
            EnsureRectangleSymmetry(xyzData);
            var dataEnum = xyzData.GetEnumerator();

            dataEnum.MoveNext();
            var row = dataEnum.Current;
            var height = xyzData.Count;
            var width = row.Value.Count;

            var rgbArray = new Color[height][];


            int count = 0;
            do
            {
                var y = count % width;
                rgbArray[y] = new Color[width];

                for (int x = 0; x < width; x++)
                {
                    var scaledMagnitude = (int)CS152Helpers.ScaleValue(row.Value[x].Item2);
                    rgbArray[y][x] = Bitmapper.GetColor(scaledMagnitude, conversionMode);
                }

                count++;

            } while (dataEnum.MoveNext());

            return new RgbArray(rgbArray);
        }

        private static void EnsureRectangleSymmetry(Dictionary<double, List<Tuple<double, double>>> data)
        {
            _ = data ?? throw new ArgumentNullException("The value provided was null");

            var dataEnum = data.GetEnumerator();

            string symmetryErrorMessage = "Data provided is not square-symmetric";
            if (!dataEnum.MoveNext()) throw new Exception(symmetryErrorMessage);

            var width = dataEnum.Current.Value.Count;

            while (dataEnum.MoveNext())
            {
                if (width != dataEnum.Current.Value.Count) throw new Exception(symmetryErrorMessage);
            }
        }
    }
}
