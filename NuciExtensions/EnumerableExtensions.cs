using System;
using System.Collections.Generic;
using System.Linq;

namespace NuciExtensions
{
    /// <summary>
    /// Provides extension methods for enumerable collections.
    /// </summary>
    public static class EnumerableExtensions
    {
        static readonly Lazy<Random> lazyRandom = new(() => new Random());

        /// <summary>
        /// Gets a random element.
        /// </summary>
        /// <returns>The element.</returns>
        /// <param name="enumerable">Enumerable.</param>
        /// <exception cref="ArgumentNullException">Thrown if the enumerable is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the enumerable is empty.</exception>
        public static T GetRandomElement<T>(this IEnumerable<T> enumerable)
        {
            ArgumentNullException.ThrowIfNull(enumerable);

            if (!enumerable.Any())
            {
                throw new InvalidOperationException("Cannot get a random element from an empty collection.");
            }

            Random random = lazyRandom.Value;

            return enumerable.ElementAt(random.Next(enumerable.Count()));
        }

        /// <summary>
        /// Gets a random element.
        /// </summary>
        /// <returns>The element.</returns>
        /// <param name="enumerable">Enumerable.</param>
        /// <param name="random">Random object to use.</param>
        /// <exception cref="ArgumentNullException">Thrown if the enumerable or random is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the enumerable is empty.</exception>
        public static T GetRandomElement<T>(this IEnumerable<T> enumerable, Random random)
        {
            ArgumentNullException.ThrowIfNull(enumerable);
            ArgumentNullException.ThrowIfNull(random);

            if (!enumerable.Any())
            {
                throw new InvalidOperationException("Cannot get a random element from an empty collection.");
            }

            return enumerable.ElementAt(random.Next(enumerable.Count()));
        }

        /// <summary>
        /// Gets the duplicated elements.
        /// </summary>
        /// <param name="source">The collection.</param>
        /// <returns>The duplicated elements.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the source is null.</exception>
        public static IEnumerable<T> GetDuplicates<T>(this IEnumerable<T> source)
        {
            ArgumentNullException.ThrowIfNull(source);

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
        /// Checks whether the collection is empty.
        /// </summary>
        /// <param name="enumerable">The collection.</param>
        /// <returns>True if the collection is empty, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the enumerable is null.</exception>
        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            ArgumentNullException.ThrowIfNull(enumerable);

            return !enumerable.Any();
        }
    }
}
