using System.Collections.Generic;

using NUnit.Framework;

namespace NuciExtensions.UnitTests
{
    public class EnumerableExtTests
    {
        [Test]
        public void IsEmpty_CollectionIsEmpty_ReturnsTrue()
            => Assert.That(EnumerableExt.IsEmpty(new List<string>()));

        [Test]
        public void IsEmpty_CollectionIsNotEmpty_ReturnsFalse()
            => Assert.That(
                EnumerableExt.IsEmpty(["test"]),
                Is.False);

        [Test]
        public void IsNullOrEmpty_CollectionIsNull_ReturnsTrue()
            => Assert.That(EnumerableExt.IsNullOrEmpty((IEnumerable<string>)null));
    }
}
