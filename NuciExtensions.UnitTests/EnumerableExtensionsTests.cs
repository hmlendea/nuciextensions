using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using NuciExtensions;

namespace NuciExtensions.UnitTests
{
    public class EnumerableExtensionsTests
    {
        [Test]
        public void GetRandomElement_ReturnsElementFromCollection()
        {
            IEnumerable<string> collection = new List<string>
            {
                "test1", "test2", "test3"
            };

            string actual = collection.GetRandomElement();

            Assert.IsTrue(collection.Any(x => x.Equals(actual)));
        }
    }
}
