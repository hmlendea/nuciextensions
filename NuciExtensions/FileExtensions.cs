using System;
using System.IO;

namespace NuciExtensions
{
    /// <summary>
    /// <see cref="File"/> extensions.
    /// </summary>
    public static class FileExtensions
    {
        public static bool ExistsInPathVariable(string fileName)
        {
            if (File.Exists(fileName))
            {
                return true;
            }

            var values = Environment.GetEnvironmentVariable("PATH");

            foreach (var path in values.Split(Path.PathSeparator))
            {
                var fullPath = Path.Combine(path, fileName);

                if (File.Exists(fullPath))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
