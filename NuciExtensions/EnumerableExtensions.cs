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

            random ??= new Random();

            return enumerable.ElementAt(random.Next(enumerable.Count()));
        }

        /// <summary>
        /// Gets a random element.
        /// </summary>
        /// <returns>The element.</returns>
        /// <param name="enumerable">Enumerable.</param>
        /// <param name="random">Random object to use.</param>
        public static T GetRandomElement<T>(this IEnumerable<T> enumerable, Random random)
        {
            if (EnumerableExt.IsNullOrEmpty(enumerable))
            {
                throw new NullReferenceException();
            }

            return enumerable.ElementAt(random.Next(enumerable.Count()));
        }

        /// <summary>
        /// Gets the duplicated elements.
        /// </summary>
        /// <param name="source">The collection.</param>
        /// <returns>The duplicated elements.</returns>
        public static IEnumerable<T> GetDuplicates<T>(this IEnumerable<T> source)
        {
            HashSet<T> itemsSeen = [];
            HashSet<T> itemsYielded = [];

            foreach (T item in source)
            {
                if (!itemsSeen.Add(item) && itemsYielded.Add(item))
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Checks wether the collection is empty.
        /// </summary>
        /// <param name="enumerable">The collection.</param>
        /// <returns>True if the collection is empty, false otherwise.</returns>
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
            => !enumerable.Any();
    }
}
