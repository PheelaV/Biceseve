using Biceseve.Lib;
using Biceseve.Lib.Enums;
using System;

namespace Biceseve.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var paramsLengthValid = args.Length == 2 || args.Length == 3;
            if(args[0] == "-r" || args[0] == "--read" && paramsLengthValid)
            {
                var colorMode = 
                      args.Length != 3 || args[2] == "3" ? MagnitudeRgbConversionMode.monochromatic 
                    : args[2] == "1"? MagnitudeRgbConversionMode.scaledHue 
                    : MagnitudeRgbConversionMode.realistic;
                XyzHelper.ReadXYZFormat(args[1], colorMode);
            } 
            else if (args[0] == "-w" || args[0] == "--write" && paramsLengthValid)
            {
                XyzHelper.WriteXYZFormat(args[1]);
            } 
            else
            {
                throw new ArgumentException("Invalid program params");
            }
        }
    }
}
