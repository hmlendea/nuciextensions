using System;
using System.Collections.Generic;
using System.Linq;

namespace NuciExtensions
{
    /// <summary>
    /// Enumerable extensions.
    /// </summary>
    public static class EnumerableExt
    {
        static Random random;

        /// <summary>
        /// Checks wether the collection is empty.
        /// </summary>
        /// <param name="enumerable">The collection.</param>
        /// <returns>True if the collection is empty, false otherwise.</returns>
        public static bool IsNullOrEmpty<T>(IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return true;
            }

            return enumerable.Count() < 1;
        }
    }
}
