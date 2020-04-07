using System;
using System.IO;

namespace Biceseve.Lib
{
    public static class IoHelpers
    {
        public static FileInfo GetFile(string filePath)
        {
            var file = new FileInfo(filePath);
            if (!file.Exists) throw new ArgumentException("Provided file does not exist");

            return file;
        }
    }
}
