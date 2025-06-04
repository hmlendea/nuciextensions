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
            IList<string> collection = [ "test1", "test2", "test3" ];
            var randomElement = collection.GetRandomElement();

            Assert.That(collection.Any(x => x.Equals(randomElement)));
        }

        [Test]
        public void GetRandomElement_CalledWithRandomParameter_ReturnsTheExpectedElement()
        {
            IList<string> collection = [ "test1", "test2", "test3" ];

            Assert.That(
                collection.GetRandomElement(new Random(613)),
                Is.EqualTo(collection[2]));
        }
    }
}
