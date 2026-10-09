using System;
using System.Collections.Generic;
using System.Linq;

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
        public static bool IsNullOrEmpty<T>(IEnumerable<T>? enumerable)
            => enumerable is null || !enumerable.Any();

        /// <summary>
        /// Checks whether the collection is empty.
        /// </summary>
        /// <param name="enumerable">The collection.</param>
        /// <returns>True if the collection is empty, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the enumerable is null.</exception>
        public static bool IsEmpty<T>(IEnumerable<T> enumerable)
        {
            ArgumentNullException.ThrowIfNull(enumerable);

            return !enumerable.Any();
        }
    }
}
