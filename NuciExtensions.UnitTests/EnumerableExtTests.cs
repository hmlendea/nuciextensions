using System;
using System.Collections.Generic;

using NUnit.Framework;

using NuciExtensions;

namespace NuciExtensions.UnitTests
{
    public class EnumerableExtTests
    {
        [Test]
        public void IsEmpty_CollectionIsEmpty_ReturnsTrue()
        {
            IEnumerable<string> collection = new List<string>();

            bool actual = EnumerableExt.IsEmpty(collection);

            Assert.IsTrue(actual);
        }

        [Test]
        public void IsEmpty_CollectionIsNotEmpty_ReturnsFalse()
        {
            IEnumerable<string> collection = new List<string> { "test" };

            bool actual = EnumerableExt.IsEmpty(collection);

            Assert.IsFalse(actual);
        }

        [Test]
        public void IsNullOrEmpty_CollectionIsNull_ReturnsTrue()
        {
            IEnumerable<string> collection = null;

            bool actual = EnumerableExt.IsNullOrEmpty(collection);

            Assert.IsTrue(actual);
        }
    }
}
