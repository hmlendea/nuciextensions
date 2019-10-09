using System;
using System.Collections.Generic;

using NUnit.Framework;

using NuciExtensions;

namespace NuciExtensions.UnitTests
{
    public class ListExtensionsTests
    {
        [Test]
        public void Shuffle_OrderIsNotTheSame()
        {
            IList<int> collection = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

            IList<int> actual = collection.Shuffle();

            Assert.IsTrue(
                actual[0] != 0 ||
                actual[1] != 1 ||
                actual[2] != 2 ||
                actual[3] != 3 ||
                actual[4] != 4 ||
                actual[5] != 5 ||
                actual[6] != 6 ||
                actual[7] != 7 ||
                actual[8] != 8 ||
                actual[9] != 9 ||
                actual[10] != 10 ||
                actual[11] != 11 ||
                actual[12] != 12 ||
                actual[13] != 13 ||
                actual[14] != 14 ||
                actual[15] != 15);
        }

        [Test]
        public void Shuffle_CountIsTheSame()
        {
            IList<int> collection = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

            IList<int> actual = collection.Shuffle();
            
            Assert.AreEqual(collection.Count, actual.Count);
        }
    }
}
