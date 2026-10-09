using System;
using System.Collections.Generic;
using System.Linq;

namespace NuciExtensions
{
    /// <summary>
    /// Provides extension methods for working with lists.
    /// </summary>
    public static class ListExtensions
    {
        static readonly Lazy<Random> lazyRandom = new(() => new Random());

        /// <summary>
        /// Shuffles the elements of the specified list.
        /// </summary>
        /// <param name="list">The list to shuffle.</param>
        /// <returns>A new list containing the shuffled elements.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the list is null.</exception>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            ArgumentNullException.ThrowIfNull(list);

            if (list.Count == 0)
            {
                return list;
            }

            Random random = lazyRandom.Value;

            List<T> clone = [.. list];
            List<T> result = [];

            while (clone.Count > 0)
            {
                int randomIndex = random.Next(clone.Count);
                T obj = clone.ElementAt(randomIndex);

                clone.RemoveAt(randomIndex);
                result.Add(obj);
            }

            return result;
        }

        /// <summary>
        /// Removes and returns the last element of the specified list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="source">The list from which to remove the last element.</param>
        /// <returns>The last element of the list.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the source list is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the list is empty.</exception>
        public static T Pop<T>(this IList<T> source)
        {
            ArgumentNullException.ThrowIfNull(source);

            if (source.Count == 0)
            {
                throw new InvalidOperationException("There are no elements in the list");
            }

            int index = source.Count - 1;

            T element = source.ElementAt(index);
            source.RemoveAt(index);

            return element;
        }
    }
}
