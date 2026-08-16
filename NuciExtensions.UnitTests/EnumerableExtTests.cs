using System.Collections.Generic;

using NUnit.Framework;

namespace NuciExtensions.UnitTests
{
    [TestFixture]
    public sealed class EnumerableExtTests
    {
        [Test]
        public void GivenAnEmptyCollection_WhenCheckingWhetherItIsEmpty_ThenTrueIsReturned()
        {
            IEnumerable<string> collection = [];

            Assert.That(EnumerableExt.IsEmpty(collection));
        }

        [Test]
        public void GivenAPopulatedCollection_WhenCheckingWhetherItIsEmpty_ThenFalseIsReturned()
        {
            IEnumerable<string> collection = ["Minecraft"];

            Assert.That(
                EnumerableExt.IsEmpty(collection),
                Is.False);
        }

        [Test]
        public void GivenANullCollection_WhenCheckingWhetherItIsNullOrEmpty_ThenTrueIsReturned()
        {
            IEnumerable<string> collection = null!;

            Assert.That(EnumerableExt.IsNullOrEmpty(collection));
        }

        [Test]
        public void GivenAnEmptyCollection_WhenCheckingWhetherItIsNullOrEmpty_ThenTrueIsReturned()
        {
            IEnumerable<string> collection = [];

            Assert.That(EnumerableExt.IsNullOrEmpty(collection));
        }

        [Test]
        public void GivenAPopulatedCollection_WhenCheckingWhetherItIsNullOrEmpty_ThenFalseIsReturned()
        {
            IEnumerable<string> collection = ["Minecraft"];

            Assert.That(
                EnumerableExt.IsNullOrEmpty(collection),
                Is.False);
        }
    }
}
