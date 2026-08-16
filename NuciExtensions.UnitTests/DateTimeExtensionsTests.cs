using System;

using NUnit.Framework;

namespace NuciExtensions.UnitTests
{
    [TestFixture]
    public sealed class DateTimeExtensionsTests
    {
        [Test]
        public void GivenAValidUnixDate_WhenGettingElapsedUnixTime_ThenTheCorrectDurationIsReturned()
        {
            TimeSpan expected = new(10001664000000000);
            TimeSpan actual = DateTimeExtensions.GetElapsedUnixTime(new(2001, 09, 11));

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GivenADateBeforeTheUnixEpoch_WhenGettingElapsedUnixTime_ThenAnArgumentOutOfRangeExceptionIsThrown()
            => Assert.That(
                () => DateTimeExtensions.GetElapsedUnixTime(new(1917, 03, 08)),
                Throws.TypeOf<ArgumentOutOfRangeException>());

        [Test]
        public void GivenAValidUnixTimeString_WhenConvertingFromUnixTime_ThenTheCorrectDateIsReturned()
        {
            DateTime expected = new(1970, 1, 8, 2, 31, 13);
            DateTime actual = DateTimeExtensions.FromUnixTime("613873");

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GivenAnInvalidUnixTimeString_WhenConvertingFromUnixTime_ThenAnArgumentExceptionIsThrown()
            => Assert.That(
                () => DateTimeExtensions.FromUnixTime("Praise the Sun!"),
                Throws.TypeOf<ArgumentException>());

        [Test]
        public void GivenAValidUnixTimeNumber_WhenConvertingFromUnixTime_ThenTheCorrectDateIsReturned()
        {
            DateTime expected = new(2251, 12, 9, 20, 3, 51);
            DateTime actual = DateTimeExtensions.FromUnixTime(8897112231);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
