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
        private static readonly Lazy<Random> lazyRandom = new(() => new Random());

        /// <summary>
        /// Gets a random element.
        /// </summary>
        /// <returns>The element.</returns>
        /// <param name="enumerable">Enumerable.</param>
        /// <remarks>
        /// The sequence is enumerated at most once, so single-pass, stateful and side-effecting
        /// sequences are never re-read and cannot produce inconsistent results between passes.
        /// A sequence that provides indexed access is indexed directly and is never enumerated;
        /// any other sequence is sampled in a single pass while it is enumerated, so no element
        /// is buffered. An unbounded sequence that is not indexable does not terminate.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if the enumerable is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the enumerable is empty.</exception>
        public static T GetRandomElement<T>(this IEnumerable<T> enumerable)
        {
            ArgumentNullException.ThrowIfNull(enumerable);

            return enumerable.GetRandomElement(lazyRandom.Value);
        }

        /// <summary>
        /// Gets a random element.
        /// </summary>
        /// <returns>The element.</returns>
        /// <param name="enumerable">Enumerable.</param>
        /// <param name="random">Random object to use.</param>
        /// <remarks>
        /// The sequence is enumerated at most once, so single-pass, stateful and side-effecting
        /// sequences are never re-read and cannot produce inconsistent results between passes.
        /// A sequence that provides indexed access is indexed directly and is never enumerated;
        /// any other sequence is sampled in a single pass while it is enumerated, so no element
        /// is buffered. An unbounded sequence that is not indexable does not terminate. Results
        /// are reproducible only for the same sequence and the same <paramref name="random"/>
        /// state; the supplied instance is not synchronised, so concurrent callers must provide a
        /// thread-safe instance.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if the enumerable or random is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the enumerable is empty.</exception>
        public static T GetRandomElement<T>(this IEnumerable<T> enumerable, Random random)
        {
            ArgumentNullException.ThrowIfNull(enumerable);
            ArgumentNullException.ThrowIfNull(random);

            if (enumerable is IReadOnlyList<T> readOnlyList)
            {
                if (readOnlyList.Count == 0)
                {
                    throw new InvalidOperationException("Cannot get a random element from an empty collection.");
                }

                return readOnlyList[random.Next(readOnlyList.Count)];
            }

            return SampleRandomElement(enumerable, random);
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

        private static T SampleRandomElement<T>(IEnumerable<T> enumerable, Random random)
        {
            using (IEnumerator<T> enumerator = enumerable.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    throw new InvalidOperationException("Cannot get a random element from an empty collection.");
                }

                T selectedElement = enumerator.Current;
                int elementCount = 1;

                while (enumerator.MoveNext())
                {
                    elementCount += 1;

                    if (random.Next(elementCount) == 0)
                    {
                        selectedElement = enumerator.Current;
                    }
                }

                return selectedElement;
            }
        }
    }
}
