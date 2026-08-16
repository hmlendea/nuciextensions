using System;

using NUnit.Framework;

using NuciExtensions.UnitTests.Helpers;

namespace NuciExtensions.UnitTests
{
    [TestFixture]
    public sealed class EnumExtensionsTests
    {
        [Test]
        public void GivenAnEnumerationValueWithADisplayName_WhenGettingTheDisplayName_ThenTheConfiguredNameIsReturned()
            => Assert.That(
                DummyDisplayEnum.ValueWithDisplayName.GetDisplayName(),
                Is.EqualTo("Praise the Sun!"));

        [Test]
        public void GivenAnEnumerationValueWithoutADisplayName_WhenGettingTheDisplayName_ThenTheValueNameIsReturned()
            => Assert.That(
                DummyDisplayEnum.ValueWithoutDisplayName.GetDisplayName(),
                Is.EqualTo(nameof(DummyDisplayEnum.ValueWithoutDisplayName)));

        [Test]
        public void GivenANullEnumerationValue_WhenGettingTheDisplayName_ThenANullReferenceExceptionIsThrown()
        {
            Enum value = null!;

            Assert.That(
                () => value.GetDisplayName(),
                Throws.TypeOf<NullReferenceException>());
        }

        [Test]
        public void GivenAnUndefinedEnumerationValue_WhenGettingTheDisplayName_ThenAnArgumentNullExceptionIsThrown()
        {
            DummyDisplayEnum value = (DummyDisplayEnum)613;

            Assert.That(
                () => value.GetDisplayName(),
                Throws.TypeOf<ArgumentNullException>());
        }
    }
}