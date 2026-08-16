using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace NuciExtensions.UnitTests
{
    [TestFixture]
    public sealed class ListExtensionsTests
    {
        [Test]
        public void GivenAPopulatedList_WhenShuffling_ThenTheSameElementsAreReturned()
        {
            IList<int> collection = [4, 8, 16, 32, 42, 48, 64, 96];

            IList<int> shuffledCollection = collection.Shuffle();

            Assert.That(shuffledCollection, Is.EquivalentTo(collection));
            Assert.That(shuffledCollection, Is.Not.SameAs(collection));
        }

        [Test]
        public void GivenAPopulatedList_WhenShuffling_ThenTheCountIsPreserved()
        {
            IList<int> collection = [4, 8, 16, 32, 42, 48, 64, 96];

            IList<int> shuffledCollection = collection.Shuffle();

            Assert.That(shuffledCollection, Has.Count.EqualTo(collection.Count));
        }

        [Test]
        public void GivenANullList_WhenShuffling_ThenANullReferenceExceptionIsThrown()
        {
            IList<int> collection = null!;

            Assert.That(
                () => collection.Shuffle(),
                Throws.TypeOf<NullReferenceException>());
        }

        [Test]
        public void GivenAnEmptyList_WhenShuffling_ThenTheOriginalListIsReturned()
        {
            IList<int> collection = [];

            Assert.That(collection.Shuffle(), Is.SameAs(collection));
        }

        [Test]
        public void GivenASingleElementList_WhenShuffling_ThenTheElementIsReturned()
        {
            IList<int> collection = [42];

            Assert.That(collection.Shuffle(), Is.EqualTo(collection));
        }

        [Test]
        public void GivenAPopulatedList_WhenPopping_ThenTheLastElementIsRemoved()
        {
            IList<int> collection = [4, 8, 16, 42];
            IEnumerable<int> expectedCollection = [4, 8, 16];

            collection.Pop();

            Assert.That(collection, Is.EqualTo(expectedCollection));
        }

        [Test]
        public void GivenAPopulatedList_WhenPopping_ThenTheLastElementIsReturned()
        {
            IList<int> collection = [4, 8, 16, 42];

            int poppedElement = collection.Pop();

            Assert.That(poppedElement, Is.EqualTo(42));
        }

        [Test]
        public void GivenAnEmptyList_WhenPopping_ThenAnIndexOutOfRangeExceptionIsThrown()
        {
            IList<int> collection = [];

            Assert.That(
                () => collection.Pop(),
                Throws.TypeOf<IndexOutOfRangeException>());
        }

        [Test]
        public void GivenANullList_WhenPopping_ThenANullReferenceExceptionIsThrown()
        {
            IList<int> collection = null!;

            Assert.That(
                () => collection.Pop(),
                Throws.TypeOf<NullReferenceException>());
        }
    }
}
