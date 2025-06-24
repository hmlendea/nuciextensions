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
                !actual[0].Equals(0) ||
                !actual[1].Equals(1) ||
                !actual[2].Equals(2) ||
                !actual[3].Equals(3) ||
                !actual[4].Equals(4) ||
                !actual[5].Equals(5) ||
                !actual[6].Equals(6) ||
                !actual[7].Equals(7) ||
                !actual[8].Equals(8) ||
                !actual[9].Equals(9) ||
                !actual[10].Equals(10) ||
                !actual[11].Equals(11) ||
                !actual[12].Equals(12) ||
                !actual[13].Equals(13) ||
                !actual[14].Equals(14) ||
                !actual[15].Equals(15));
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
