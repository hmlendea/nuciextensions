using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace NuciExtensions.UnitTests
{
    public class EnumerableExtensionsTests
    {
        [Test]
        public void GetRandomElement_ReturnsElementFromCollection()
        {
            IList<string> collection = new List<string>
            {
                "test1", "test2", "test3"
            };

            string actual = collection.GetRandomElement();

            Assert.IsTrue(collection.Any(x => x.Equals(actual)));
        }

        [Test]
        public void GetRandomElement_CalledWithRandomParameter_ReturnsTheExpectedElement()
        {
            IList<string> collection = new List<string>
            {
                "test1", "test2", "test3"
            };

            Random random = new Random(613);
            string actual = collection.GetRandomElement(random);

            Assert.AreEqual(collection[2], actual);
        }
    }
}
