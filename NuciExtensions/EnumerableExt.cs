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
        /// <summary>
        /// Checks wether the collection is null or empty.
        /// </summary>
        /// <param name="enumerable">The collection.</param>
        /// <returns>True if the collection is null or empty, false otherwise.</returns>
        public static bool IsNullOrEmpty<T>(IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return true;
            }
            
            return IsEmpty(enumerable);
        }

        /// <summary>
        /// Checks wether the collection is empty.
        /// </summary>
        /// <param name="enumerable">The collection.</param>
        /// <returns>True if the collection is empty, false otherwise.</returns>
        public static bool IsEmpty<T>(IEnumerable<T> enumerable)
        {
            return enumerable.IsEmpty();
        }
    }
}
