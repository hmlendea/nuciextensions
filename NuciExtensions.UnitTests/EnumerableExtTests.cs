using System.Collections.Generic;

using NUnit.Framework;

namespace NuciExtensions.UnitTests
{
    public class EnumerableExtTests
    {
        [Test]
        public void IsEmpty_CollectionIsEmpty_ReturnsTrue()
        {
            IEnumerable<string> collection = new List<string>();

            bool actual = EnumerableExt.IsEmpty(collection);

            Assert.That(actual);
        }

        [Test]
        public void IsEmpty_CollectionIsNotEmpty_ReturnsFalse()
        {
            IEnumerable<string> collection = new List<string> { "test" };

            bool actual = EnumerableExt.IsEmpty(collection);

            Assert.That(actual, Is.False);
        }

        [Test]
        public void IsNullOrEmpty_CollectionIsNull_ReturnsTrue()
        {
            IEnumerable<string> collection = null;

            bool actual = EnumerableExt.IsNullOrEmpty(collection);

            Assert.That(actual);
        }
    }
}
