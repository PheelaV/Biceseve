using Biceseve.Lib.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Biceseve.Lib
{
    public static class XyzHelper
    {
        public const int MagnitudeMin = 0;
        public const int MagnitudeMax = 0;
        public static void WriteXYZFormat(string filePath)
        {
            var file = IoHelpers.GetFile(filePath);

            using var sourceImage = new Bitmap(file.FullName);

            var rgbArray = sourceImage.ConvertToRGBArray(MagnitudeRgbConversionMode.monochromatic);

            //rgbArray.SaveRgbArrayAsJpgImage(Path.Combine(file.DirectoryName, "output-write.jpg"));
            rgbArray.SaveRGBArrayAsXYZ(Path.Combine(file.DirectoryName, $"{Path.GetFileNameWithoutExtension(file.Name)}-write_output.xyz"));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "<Pending>")]
        public static void ReadXYZFormat(string filePath)
        {
            var file = IoHelpers.GetFile(filePath);

            var data = GetXyzData(filePath);

            var rgbArray = ConvertToRgbArray(data);

            //for (int i = 0; i < rgbArray.data.Length; i++)
            //{
            //    rgbArray.data[i][500] = Color.White;
            //}
            rgbArray.SaveRgbArrayAsBmpImage(Path.Combine(file.DirectoryName, $"{Path.GetFileNameWithoutExtension(file.Name)}-read_output.bmp"));
        }

        public static Dictionary<double, Dictionary<double, double>> GetXyzData(string filePath)
        {
            var file = IoHelpers.GetFile(filePath);
            var data = new Dictionary<double, Dictionary<double, double>>();

            using var sr = new StreamReader(file.FullName);
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine()?.Split('\t');

                (double longitude, double latitude, double altitude) = (double.Parse(line[0]), double.Parse(line[1]), double.Parse(line[2]));

                if (!data.ContainsKey(latitude))
                {
                    data[latitude] = new Dictionary<double, double>();
                }
                data[latitude][longitude] = altitude;
            }

            return data;
        }

        public static RgbArray ConvertToRgbArray(Dictionary<double, Dictionary<double, double>> xyzData, MagnitudeRgbConversionMode conversionMode = MagnitudeRgbConversionMode.monochromatic)
        {
            EnsureRectangleSymmetry(xyzData);

            var xyzDataEnum = xyzData.GetEnumerator();
            var firstRow = xyzData.First();

            var height = xyzData.Count;
            var width = firstRow.Value.Count;
            var rgbArray = new Color[height][];
            int count = 0;

            while (xyzDataEnum.MoveNext())
            {
                var row = xyzDataEnum.Current;
                var y = count / width;
                rgbArray[y] = new Color[width];

                var rowEnum = row.Value.GetEnumerator();

                while (rowEnum.MoveNext())
                {
                    var x = count % width;
                    var pixel = rowEnum.Current;
                    var scaledMagnitude = (int)CS152Helpers.ScaleValue(pixel.Value, -11000, 8500, 0, 255);
                    rgbArray[y][x] = Bitmapper.GetColor(scaledMagnitude, conversionMode);
                    count++;
                }
            }

            return new RgbArray(rgbArray);
        }

        private static void EnsureRectangleSymmetry(Dictionary<double, Dictionary<double, double>> xyzData)
        {
            _ = xyzData ?? throw new ArgumentNullException("The value provided was null");

            var dataEnum = xyzData.GetEnumerator();

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
