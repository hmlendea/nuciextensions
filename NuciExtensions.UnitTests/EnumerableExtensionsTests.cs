using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace NuciExtensions.UnitTests
{
    [TestFixture]
    public class EnumerableExtensionsTests
    {
        [Test]
        public void GivenAPopulatedCollection_WhenGettingARandomElement_ThenAnElementFromTheCollectionIsReturned()
        {
            IEnumerable<string> collection = ["Dark Souls III", "Minecraft", "Terraria"];
            string randomElement = collection.GetRandomElement();

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
        public void GivenANullCollection_WhenGettingARandomElement_ThenANullReferenceExceptionIsThrown()
        {
            IEnumerable<string> collection = null!;

            Assert.That(
                () => collection.GetRandomElement(),
                Throws.TypeOf<NullReferenceException>());
        }

        [Test]
        public void GivenAnEmptyCollection_WhenGettingARandomElement_ThenANullReferenceExceptionIsThrown()
        {
            IEnumerable<string> collection = [];

            Assert.That(
                () => collection.GetRandomElement(),
                Throws.TypeOf<NullReferenceException>());
        }

        [Test]
        public void GivenANullCollectionAndRandom_WhenGettingARandomElement_ThenANullReferenceExceptionIsThrown()
        {
            IEnumerable<string> collection = null!;
            Random random = new(613);

            Assert.That(
                () => collection.GetRandomElement(random),
                Throws.TypeOf<NullReferenceException>());
        }

        [Test]
        public void GivenAnEmptyCollectionAndRandom_WhenGettingARandomElement_ThenANullReferenceExceptionIsThrown()
        {
            IEnumerable<string> collection = [];
            Random random = new(613);

            Assert.That(
                () => collection.GetRandomElement(random),
                Throws.TypeOf<NullReferenceException>());
        }

        [Test]
        public void GivenANullRandom_WhenGettingARandomElement_ThenANullReferenceExceptionIsThrown()
        {
            IEnumerable<string> collection = ["Dark Souls III"];
            Random random = null!;

            Assert.That(
                () => collection.GetRandomElement(random),
                Throws.TypeOf<NullReferenceException>());
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
        public void GivenANullCollection_WhenGettingDuplicates_ThenANullReferenceExceptionIsThrown()
        {
            IEnumerable<string> collection = null!;

            Assert.That(
                () => collection.GetDuplicates().ToArray(),
                Throws.TypeOf<NullReferenceException>());
        }
    }
}
