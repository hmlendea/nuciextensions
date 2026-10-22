using System;
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

        [TestCaseSource(nameof(IsEmptyTestCases))]
        public void GivenACollection_WhenCheckingWhetherItIsEmpty_ThenReturnsExpectedResult_Parameterized(
            IEnumerable<string> collection, bool expected)
        {
            Assert.That(EnumerableExt.IsEmpty(collection), Is.EqualTo(expected));
        }

        private static IEnumerable<TestCaseData> IsEmptyTestCases()
        {
            yield return new TestCaseData(Array.Empty<string>(), true).SetName("EmptyArray");
            yield return new TestCaseData(new[] { "a" }, false).SetName("SingleElement");
            yield return new TestCaseData(new[] { "a", "b", "c" }, false).SetName("MultipleElements");
            yield return new TestCaseData(new[] { "unicode café" }, false).SetName("UnicodeElement");
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
        public void GivenANullCollection_WhenCheckingWhetherItIsEmpty_ThenAnArgumentNullExceptionIsThrown()
        {
            IEnumerable<string> collection = null;

            Assert.That(
                () => EnumerableExt.IsEmpty(collection),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GivenANullCollection_WhenCheckingWhetherItIsNullOrEmpty_ThenTrueIsReturned()
        {
            IEnumerable<string> collection = null;

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

        [TestCaseSource(nameof(IsNullOrEmptyTestCases))]
        public void GivenACollection_WhenCheckingWhetherItIsNullOrEmpty_ThenReturnsExpectedResult_Parameterized(
            IEnumerable<string> collection, bool expected)
        {
            Assert.That(EnumerableExt.IsNullOrEmpty(collection), Is.EqualTo(expected));
        }

        private static IEnumerable<TestCaseData> IsNullOrEmptyTestCases()
        {
            yield return new TestCaseData(null, true).SetName("NullCollection");
            yield return new TestCaseData(Array.Empty<string>(), true).SetName("EmptyArray");
            yield return new TestCaseData(new[] { "a" }, false).SetName("SingleElement");
            yield return new TestCaseData(new[] { "a", "b" }, false).SetName("TwoElements");
            yield return new TestCaseData(new[] { "unicode café" }, false).SetName("UnicodeElement");
        }
    }
}
