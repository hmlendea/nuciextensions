using System;
using System.Collections;
using System.Collections.Generic;

namespace NuciExtensions.UnitTests.Helpers
{
    /// <summary>
    /// A read-only list that records how many times its indexer has been accessed and how many times
    /// it has been enumerated, so that indexed access can be distinguished from a full enumeration.
    /// </summary>
    internal sealed class TrackingReadOnlyList<T> : IReadOnlyList<T>
    {
        private readonly IReadOnlyList<T> items;

        internal int IndexerAccessCount { get; private set; }

        internal int EnumerationCount { get; private set; }

        internal TrackingReadOnlyList(IReadOnlyList<T> items)
        {
            ArgumentNullException.ThrowIfNull(items);

            this.items = items;
        }

        public int Count => items.Count;

        public T this[int index]
        {
            get
            {
                IndexerAccessCount += 1;

                return items[index];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            EnumerationCount += 1;

            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
