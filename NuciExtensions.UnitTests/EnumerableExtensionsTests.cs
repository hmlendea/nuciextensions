using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using NuciExtensions.UnitTests.Helpers;

namespace NuciExtensions.UnitTests
{
    [TestFixture]
    public sealed class EnumerableExtensionsTests
    {
        private static readonly string[] GameCollection = ["Dark Souls III", "Minecraft", "Terraria"];

        [Test]
        public void GivenAPopulatedCollection_WhenGettingARandomElement_ThenAnElementFromTheCollectionIsReturned()
        {
            IEnumerable<string> collection = ["Dark Souls III", "Minecraft", "Terraria"];
            string randomElement = collection.GetRandomElement();

            Assert.That(collection, Does.Contain(randomElement));
        }

        [Test]
        [TestCase(new[] { "a", "b", "c" }, 42)]
        [TestCase(new[] { "x", "y" }, 123)]
        [TestCase(new[] { "single" }, 999)]
        [TestCase(new[] { "unicode café", "test" }, 456)]
        public void GivenAPopulatedCollectionAndSeededRandom_WhenGettingARandomElement_ThenAnElementFromTheCollectionIsReturned_Parameterized(
            string[] collection, int seed)
        {
            var random = new Random(seed);
            string randomElement = collection.GetRandomElement(random);
            Assert.That(collection, Does.Contain(randomElement));
        }

        [Test]
        public void GivenAPopulatedCollectionAndSeededRandom_WhenGettingARandomElement_ThenTheExpectedElementIsReturned()
        {
            IEnumerable<string> collection = ["Dark Souls III", "Minecraft", "Terraria"];

            Assert.That(
                collection.GetRandomElement(new Random(613)),
                Is.EqualTo("Terraria"));
        }

        [Test]
        public void GivenANullCollection_WhenGettingARandomElement_ThenAnArgumentNullExceptionIsThrown()
        {
            IEnumerable<string> collection = null;

            Assert.That(
                () => collection.GetRandomElement(),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GivenAnEmptyCollection_WhenGettingARandomElement_ThenAnInvalidOperationExceptionIsThrown()
        {
            IEnumerable<string> collection = [];

            Assert.That(
                () => collection.GetRandomElement(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void GivenANullCollectionAndRandom_WhenGettingARandomElement_ThenAnArgumentNullExceptionIsThrown()
        {
            IEnumerable<string> collection = null;
            Random random = new(613);

            Assert.That(
                () => collection.GetRandomElement(random),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GivenAnEmptyCollectionAndRandom_WhenGettingARandomElement_ThenAnInvalidOperationExceptionIsThrown()
        {
            IEnumerable<string> collection = [];
            Random random = new(613);

            Assert.That(
                () => collection.GetRandomElement(random),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void GivenANullRandom_WhenGettingARandomElement_ThenAnArgumentNullExceptionIsThrown()
        {
            IEnumerable<string> collection = ["Dark Souls III"];
            Random random = null;

            Assert.That(
                () => collection.GetRandomElement(random),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GivenASingleUseSequence_WhenGettingARandomElement_ThenTheSequenceIsEnumeratedOnce()
        {
            SingleUseEnumerable<string> collection = new(GameCollection);

            string randomElement = collection.GetRandomElement(new Random(42));

            Assert.That(GameCollection, Does.Contain(randomElement));
        }

        [Test]
        public void GivenATrackedSequence_WhenGettingARandomElement_ThenTheSequenceIsEnumeratedExactlyOnce()
        {
            TrackingEnumerable<string> collection = new(GameCollection);

            collection.GetRandomElement(new Random(42));

            Assert.That(collection.EnumerationCount, Is.EqualTo(1));
        }

        [Test]
        public void GivenATrackedSequence_WhenGettingARandomElement_ThenEveryElementIsYieldedExactlyOnce()
        {
            TrackingEnumerable<string> collection = new(GameCollection);

            collection.GetRandomElement(new Random(42));

            Assert.That(collection.YieldCount, Is.EqualTo(GameCollection.Length));
        }

        [Test]
        public void GivenATrackedSequence_WhenGettingARandomElement_ThenTheEnumeratorIsDisposed()
        {
            TrackingEnumerable<string> collection = new(GameCollection);

            collection.GetRandomElement(new Random(42));

            Assert.That(collection.DisposeCount, Is.EqualTo(1));
        }

        [Test]
        public void GivenATrackedSequence_WhenGettingARandomElement_ThenAnElementFromTheSequenceIsReturned()
        {
            TrackingEnumerable<string> collection = new(GameCollection);

            string randomElement = collection.GetRandomElement(new Random(42));

            Assert.That(GameCollection, Does.Contain(randomElement));
        }

        [Test]
        public void GivenATrackedSequenceAndNoRandom_WhenGettingARandomElement_ThenTheSequenceIsEnumeratedExactlyOnce()
        {
            TrackingEnumerable<string> collection = new(GameCollection);

            collection.GetRandomElement();

            Assert.That(collection.EnumerationCount, Is.EqualTo(1));
        }

        [Test]
        [TestCase(42, "Terraria")]
        [TestCase(123, "Dark Souls III")]
        [TestCase(613, "Dark Souls III")]
        [TestCase(999, "Dark Souls III")]
        [TestCase(456, "Dark Souls III")]
        [TestCase(7, "Minecraft")]
        [TestCase(2024, "Minecraft")]
        public void GivenASequenceAndASeed_WhenGettingARandomElement_ThenTheResultIsDeterministic(
            int seed,
            string expectedElement)
        {
            SingleUseEnumerable<string> collection = new(GameCollection);

            Assert.That(collection.GetRandomElement(new Random(seed)), Is.EqualTo(expectedElement));
        }

        [Test]
        public void GivenASequenceAndTheSameSeed_WhenGettingARandomElementTwice_ThenTheResultsAreEqual()
        {
            SingleUseEnumerable<string> first = new(GameCollection);
            SingleUseEnumerable<string> second = new(GameCollection);

            string firstElement = first.GetRandomElement(new Random(613));
            string secondElement = second.GetRandomElement(new Random(613));

            Assert.That(firstElement, Is.EqualTo(secondElement));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void GivenASequenceOfOneElementAndSeeds_WhenGettingARandomElement_ThenTheOnlyElementIsReturned(int seed)
        {
            SingleUseEnumerable<string> collection = new(["Minecraft"]);

            Assert.That(collection.GetRandomElement(new Random(seed)), Is.EqualTo("Minecraft"));
        }

        [Test]
        public void GivenAnIndexableSequence_WhenGettingARandomElement_ThenTheIndexerIsUsedAndTheSequenceIsNotEnumerated()
        {
            TrackingReadOnlyList<string> collection = new(GameCollection);

            string randomElement = collection.GetRandomElement(new Random(42));

            Assert.That(collection.IndexerAccessCount, Is.EqualTo(1));
            Assert.That(collection.EnumerationCount, Is.Zero);
            Assert.That(randomElement, Is.EqualTo("Terraria"));
        }

        [Test]
        public void GivenAnIndexableSequenceWithNoRandom_WhenGettingARandomElement_ThenNoEnumerationOccurs()
        {
            TrackingReadOnlyList<string> collection = new(GameCollection);

            collection.GetRandomElement();

            Assert.That(collection.EnumerationCount, Is.Zero);
        }

        [Test]
        public void GivenAHugeIndexableSequence_WhenGettingARandomElement_ThenTheIndexerIsUsedInsteadOfEnumeration()
        {
            UnboundedReadOnlyList<int> collection = new(int.MaxValue, index => index);

            int randomElement = collection.GetRandomElement(new Random(613));

            Assert.That(randomElement, Is.InRange(0, int.MaxValue - 1));
        }

        [Test]
        public void GivenAHugeRangeSequence_WhenGettingARandomElement_ThenAnElementIsReturnedWithoutEnumeration()
        {
            IEnumerable<int> collection = Enumerable.Range(0, int.MaxValue);

            int randomElement = collection.GetRandomElement(new Random(613));

            Assert.That(randomElement, Is.InRange(0, int.MaxValue - 1));
        }

        [Test]
        public void GivenAHugeRepeatedSequence_WhenGettingARandomElement_ThenTheCallReturnsWithoutEnumeration()
        {
            IEnumerable<string> collection = Enumerable.Repeat("Minecraft", int.MaxValue);

            Assert.That(collection.GetRandomElement(new Random(613)), Is.EqualTo("Minecraft"));
        }

        [Test]
        public void GivenAnEmptySingleUseSequence_WhenGettingARandomElement_ThenAnInvalidOperationExceptionIsThrown()
        {
            SingleUseEnumerable<string> collection = new([]);

            Assert.That(
                () => collection.GetRandomElement(new Random(613)),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void GivenAnEmptyTrackedSequence_WhenGettingARandomElement_ThenTheSequenceIsEnumeratedOnce()
        {
            TrackingEnumerable<string> collection = new([]);

            Assert.That(
                () => collection.GetRandomElement(new Random(613)),
                Throws.TypeOf<InvalidOperationException>());

            Assert.That(collection.EnumerationCount, Is.EqualTo(1));
        }

        [Test]
        public void GivenAnEmptyIndexableSequence_WhenGettingARandomElement_ThenTheSequenceIsNotEnumerated()
        {
            TrackingReadOnlyList<string> collection = new([]);

            Assert.That(
                () => collection.GetRandomElement(new Random(613)),
                Throws.TypeOf<InvalidOperationException>());

            Assert.That(collection.EnumerationCount, Is.Zero);
        }

        [Test]
        [TestCase(42)]
        [TestCase(123)]
        [TestCase(613)]
        public void GivenANonIndexableSequence_WhenGettingARandomElement_ThenEveryElementIsEligible(int seed)
        {
            HashSet<string> observed = [];

            for (int iteration = 0; iteration < 60; iteration++)
            {
                SingleUseEnumerable<string> collection = new(GameCollection);

                observed.Add(collection.GetRandomElement(new Random(seed + iteration)));
            }

            Assert.That(observed, Is.EquivalentTo(GameCollection));
        }

        [Test]
        public void GivenASequenceOfManyElements_WhenGettingARandomElement_ThenEveryElementIsEligible()
        {
            string[] collection =
            [
                "Dark Souls III",
                "Minecraft",
                "Terraria",
                "Hollow Knight",
                "Celeste",
                "Stardew Valley",
                "Hades",
                "Factorio",
            ];

            HashSet<string> observed = [];

            for (int seed = 0; seed < 200; seed++)
            {
                observed.Add(((IEnumerable<string>)collection).GetRandomElement(new Random(seed)));
            }

            Assert.That(observed, Is.EquivalentTo(collection));
        }

        [Test]
        public void GivenACollectionWithoutDuplicates_WhenGettingDuplicates_ThenAnEmptyCollectionIsReturned()
        {
            IEnumerable<string> collection = ["Dark Souls III", "Minecraft", "Terraria"];

            Assert.That(collection.GetDuplicates(), Is.Empty);
        }

        [Test]
        public void GivenACollectionWithDuplicates_WhenGettingDuplicates_ThenEachDuplicateIsReturnedOnceInOrder()
        {
            IEnumerable<string> collection =
            [
                "Dark Souls III",
                "Minecraft",
                "Dark Souls III",
                "Terraria",
                "Minecraft",
                "Dark Souls III",
            ];

            Assert.That(
                collection.GetDuplicates(),
                Is.EqualTo(["Dark Souls III", "Minecraft"]));
        }

        [Test]
        [TestCase(new[] { "a", "b", "a", "c", "b" }, new[] { "a", "b" })]
        [TestCase(new[] { "x", "x", "x" }, new[] { "x" })]
        [TestCase(new[] { "a", "b", "c" }, new string[] { })]
        [TestCase(new[] { "unicode café", "test", "unicode café" }, new[] { "unicode café" })]
        [TestCase(new string[] { }, new string[] { })]
        public void GivenACollectionWithDuplicates_WhenGettingDuplicates_ThenEachDuplicateIsReturnedOnceInOrder_Parameterized(
            string[] collection, string[] expected)
        {
            Assert.That(collection.GetDuplicates(), Is.EqualTo(expected));
        }

        [Test]
        public void GivenANullCollection_WhenGettingDuplicates_ThenAnArgumentNullExceptionIsThrown()
        {
            IEnumerable<string> collection = null;

            Assert.That(
                () => collection.GetDuplicates().ToArray(),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GivenAnEmptyCollection_WhenCheckingWhetherItIsEmpty_ThenTrueIsReturned()
            => Assert.That(Array.Empty<string>().IsEmpty());

        [Test]
        public void GivenAPopulatedCollection_WhenCheckingWhetherItIsEmpty_ThenFalseIsReturned()
            => Assert.That(
                new[] { "Minecraft" }.IsEmpty(),
                Is.False);

        [Test]
        public void GivenANullCollection_WhenCheckingWhetherItIsEmpty_ThenAnArgumentNullExceptionIsThrown()
        {
            IEnumerable<string> collection = null;

            Assert.That(
                () => collection.IsEmpty(),
                Throws.TypeOf<ArgumentNullException>());
        }
    }
}
