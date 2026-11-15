using System;
using System.Collections;
using System.Collections.Generic;

namespace NuciExtensions.UnitTests.Helpers
{
    /// <summary>
    /// An indexable sequence with a very large element count that is never materialised, so that
    /// indexed access can be exercised on a sequence which cannot realistically be enumerated.
    /// </summary>
    internal sealed class UnboundedReadOnlyList<T> : IReadOnlyList<T>
    {
        private readonly Func<int, T> elementFactory;
        private readonly int count;

        internal UnboundedReadOnlyList(int count, Func<int, T> elementFactory)
        {
            ArgumentNullException.ThrowIfNull(elementFactory);
            ArgumentOutOfRangeException.ThrowIfNegative(count);

            this.count = count;
            this.elementFactory = elementFactory;
        }

        public int Count => count;

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return elementFactory(index);
            }
        }

        public IEnumerator<T> GetEnumerator() => throw new InvalidOperationException(
            "This sequence must be accessed by index and cannot be enumerated.");

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}