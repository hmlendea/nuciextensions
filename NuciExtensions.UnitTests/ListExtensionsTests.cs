using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace NuciExtensions.UnitTests
{
    public class ListExtensionsTests
    {
        [Test]
        public void Shuffle_OrderIsNotTheSame()
        {
            IList<int> collection = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];

            IList<int> actual = collection.Shuffle();

            Assert.That(
                actual[0].NotEquals(0) ||
                actual[1].NotEquals(1) ||
                actual[2].NotEquals(2) ||
                actual[3].NotEquals(3) ||
                actual[4].NotEquals(4) ||
                actual[5].NotEquals(5) ||
                actual[6].NotEquals(6) ||
                actual[7].NotEquals(7) ||
                actual[8].NotEquals(8) ||
                actual[9].NotEquals(9) ||
                actual[10].NotEquals(10) ||
                actual[11].NotEquals(11) ||
                actual[12].NotEquals(12) ||
                actual[13].NotEquals(13) ||
                actual[14].NotEquals(14) ||
                actual[15].NotEquals(15));
        }

        [Test]
        public void Shuffle_CountIsTheSame()
        {
            IList<int> collection = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];

            IList<int> actual = collection.Shuffle();

            Assert.That(actual.Count, Is.EqualTo(collection.Count));
        }

        [Test]
        public void Pop_ListIsPopulated_LastElementIsRemoved()
        {
            IList<int> collection = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];

            IList<int> actualCollection = collection.ToList();
            actualCollection.Pop();

            Assert.That(actualCollection.Count, Is.EqualTo(collection.Count -1 ));
        }

        [Test]
        public void Pop_ListIsPopulated_LastElementIsReturned()
        {
            IList<int> collection = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];

            int actual = collection.ToList().Pop();

            Assert.That(actual, Is.EqualTo(collection.Last()));
        }

        [Test]
        public void Pop_ListIsEmpty_ThrowsIndexOutOfRangeException()
            => Assert.Throws<IndexOutOfRangeException>(() => new List<int>().Pop());
    }
}
