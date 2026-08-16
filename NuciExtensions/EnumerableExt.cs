using System;
using System.Collections.Generic;

namespace NuciExtensions
{
    /// <summary>
    /// Provides extension methods for checking enumerable collection states.
    /// </summary>
    public static class EnumerableExt
    {
        /// <summary>
        /// Checks whether the collection is null or empty.
        /// </summary>
        /// <param name="enumerable">The collection.</param>
        /// <returns>True if the collection is null or empty, false otherwise.</returns>
        public static bool IsNullOrEmpty<T>(IEnumerable<T> enumerable)
            => enumerable is null || enumerable.IsEmpty();

        /// <summary>
        /// Checks whether the collection is empty.
        /// </summary>
        /// <param name="enumerable">The collection.</param>
        /// <returns>True if the collection is empty, false otherwise.</returns>
        public static bool IsEmpty<T>(IEnumerable<T> enumerable)
        {
            ArgumentNullException.ThrowIfNull(enumerable);
            return enumerable.IsEmpty();
        }
    }
}
