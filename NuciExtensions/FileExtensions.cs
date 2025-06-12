using System;
using System.IO;

namespace NuciExtensions
{
    /// <summary>
    /// Provides extension methods for file operations.
    /// </summary>
    public static class FileExtensions
    {
        /// <summary>
        /// Checks if a file exists in the system's PATH environment variable.
        /// </summary>
        /// /// <param name="fileName">The name of the file to check.</param>
        /// <returns>True if the file exists in PATH, otherwise false.</returns>
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
