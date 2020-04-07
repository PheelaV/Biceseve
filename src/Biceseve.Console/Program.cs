using System;

namespace Biceseve.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var paramsLengthValid = args.Length == 2;
            if(args[0] == "-r" || args[0] == "--read" && paramsLengthValid)
            {
                XyzHelper.ReadXYZFormat(args[1]);
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
