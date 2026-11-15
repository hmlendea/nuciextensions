using System;
using System.Collections;
using System.Collections.Generic;

namespace NuciExtensions.UnitTests.Helpers
{
    /// <summary>
    /// An enumerable that records how many times it has been enumerated, how many elements it has
    /// yielded and how many times its enumerator has been disposed, so that single-pass enumeration
    /// semantics can be asserted. The type deliberately does not implement any indexed interface,
    /// so a caller cannot satisfy itself with indexed access.
    /// </summary>
    internal sealed class TrackingEnumerable<T> : IEnumerable<T>
    {
        private readonly IReadOnlyList<T> items;

        internal int EnumerationCount { get; private set; }

        internal int YieldCount { get; private set; }

        internal int DisposeCount { get; private set; }

        internal TrackingEnumerable(IReadOnlyList<T> items)
        {
            ArgumentNullException.ThrowIfNull(items);

            this.items = items;
        }

        public IEnumerator<T> GetEnumerator()
        {
            EnumerationCount += 1;

            return Enumerate();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerator<T> Enumerate()
        {
            try
            {
                foreach (T item in items)
                {
                    YieldCount += 1;

                    yield return item;
                }
            }
            finally
            {
                DisposeCount += 1;
            }
        }
    }
}
