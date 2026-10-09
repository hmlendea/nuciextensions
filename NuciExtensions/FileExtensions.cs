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
        /// <param name="fileName">The name of the file to check.</param>
        /// <returns>True if the file exists in PATH, otherwise false.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the fileName is null.</exception>
        public static bool ExistsInPathVariable(string fileName)
        {
            ArgumentNullException.ThrowIfNull(fileName);

            if (File.Exists(fileName))
            {
                return true;
            }

            string values = Environment.GetEnvironmentVariable("PATH");

            if (values is null)
            {
                return false;
            }

            foreach (var path in values.Split(Path.PathSeparator))
            {
                string fullPath = Path.Combine(path, fileName);

                if (File.Exists(fullPath))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
