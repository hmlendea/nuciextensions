using System;
using System.Collections.Generic;
using System.Linq;

namespace NuciExtensions
{
    /// <summary>
    /// Enumerable extensions.
    /// </summary>
    public static class EnumerableExtensions
    {
        static Random random;

        /// <summary>
        /// Gets a random element.
        /// </summary>
        /// <returns>The element.</returns>
        /// <param name="enumerable">Enumerable.</param>
        public static T GetRandomElement<T>(this IEnumerable<T> enumerable)
        {
            if (EnumerableExt.IsNullOrEmpty(enumerable))
            {
                throw new NullReferenceException();
            }

            if (random == null)
            {
                random = new Random();
            }

            return enumerable.ElementAt(random.Next(enumerable.Count()));
        }
    }
}
