using System;
using System.Collections;
using System.Collections.Generic;

namespace NuciExtensions.UnitTests.Helpers
{
    /// <summary>
    /// An enumerable that throws when it is enumerated more than once, so that a caller which
    /// re-reads the sequence fails loudly instead of silently observing a different set of elements.
    /// </summary>
    internal sealed class SingleUseEnumerable<T> : IEnumerable<T>
    {
        private readonly IReadOnlyList<T> items;
        private bool hasBeenEnumerated;

        internal SingleUseEnumerable(IReadOnlyList<T> items)
        {
            ArgumentNullException.ThrowIfNull(items);

            this.items = items;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (hasBeenEnumerated)
            {
                throw new InvalidOperationException("The sequence can only be enumerated once.");
            }

            hasBeenEnumerated = true;

            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
