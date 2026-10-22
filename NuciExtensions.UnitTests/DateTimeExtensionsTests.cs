using System;
using System.Globalization;

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

        [TestCase("613873", 1970, 1, 8, 2, 31, 13)]
        [TestCase("1.5", 1970, 1, 1, 0, 0, 1, 500)]
        [TestCase("0", 1970, 1, 1, 0, 0, 0)]
        [TestCase("  1  ", 1970, 1, 1, 0, 0, 1)]
        [TestCase("1e0", 1970, 1, 1, 0, 0, 1)]
        [TestCase("0.123", 1970, 1, 1, 0, 0, 0, 123)]
        [TestCase("0.456", 1970, 1, 1, 0, 0, 0, 456)]
        [TestCase("253402300799", 9999, 12, 31, 23, 59, 59)]
        [TestCase("86400", 1970, 1, 2, 0, 0, 0)]
        [TestCase("3600", 1970, 1, 1, 1, 0, 0)]
        [TestCase("60", 1970, 1, 1, 0, 1, 0)]
        [TestCase("1000000000", 2001, 9, 9, 1, 46, 40)]
        [TestCase("1609459200", 2021, 1, 1, 0, 0, 0)]
        [TestCase("1704067200", 2024, 1, 1, 0, 0, 0)]
        [TestCase("1.5e2", 1970, 1, 1, 0, 2, 30)]
        [TestCase("1E3", 1970, 1, 1, 0, 16, 40)]
        public void GivenValidUnixTimeString_WhenConvertingFromUnixTime_ThenReturnsCorrectDate(
            string input, int year, int month, int day, int hour, int minute, int second, int millisecond = 0)
        {
            DateTime expected = new(year, month, day, hour, minute, second, millisecond, DateTimeKind.Utc);
            DateTime actual = DateTimeExtensions.FromUnixTime(input);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase("Praise the Sun!")]
        [TestCase("")]
        [TestCase("   ")]
        public void GivenInvalidUnixTimeString_WhenConvertingFromUnixTime_ThenThrowsArgumentException(string input)
            => Assert.That(
                () => DateTimeExtensions.FromUnixTime(input),
                Throws.TypeOf<ArgumentException>());

        [Test]
        public void GivenANullUnixTimeString_WhenConvertingFromUnixTime_ThenAnArgumentNullExceptionIsThrown()
            => Assert.That(
                () => DateTimeExtensions.FromUnixTime(null),
                Throws.TypeOf<ArgumentNullException>());

        [TestCase(8897112231, 2251, 12, 9, 20, 3, 51)]
        [TestCase(0, 1970, 1, 1, 0, 0, 0)]
        [TestCase(0.123, 1970, 1, 1, 0, 0, 0, 123)]
        [TestCase(0.456, 1970, 1, 1, 0, 0, 0, 456)]
        [TestCase(253402300799, 9999, 12, 31, 23, 59, 59)]
        [TestCase(86400, 1970, 1, 2, 0, 0, 0)]
        [TestCase(3600, 1970, 1, 1, 1, 0, 0)]
        [TestCase(60, 1970, 1, 1, 0, 1, 0)]
        [TestCase(1000000000, 2001, 9, 9, 1, 46, 40)]
        [TestCase(1609459200, 2021, 1, 1, 0, 0, 0)]
        [TestCase(1704067200, 2024, 1, 1, 0, 0, 0)]
        [TestCase(1.5e2, 1970, 1, 1, 0, 2, 30)]
        [TestCase(1E3, 1970, 1, 1, 0, 16, 40)]
        [TestCase(0.001, 1970, 1, 1, 0, 0, 0, 1)]
        [TestCase(0.999, 1970, 1, 1, 0, 0, 0, 999)]
        public void GivenValidUnixTimeNumber_WhenConvertingFromUnixTime_ThenReturnsCorrectDate(
            double input, int year, int month, int day, int hour, int minute, int second, int millisecond = 0)
        {
            DateTime expected = new(year, month, day, hour, minute, second, millisecond, DateTimeKind.Utc);
            DateTime actual = DateTimeExtensions.FromUnixTime(input);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase(-1)]
        [TestCase(-0.001)]
        [TestCase(-86400)]
        [TestCase(-3600)]
        [TestCase(-60)]
        [TestCase(-1000000000)]
        [TestCase(-1.5)]
        [TestCase(-0.0001)]
        public void GivenNegativeUnixTimeNumber_WhenConvertingFromUnixTime_ThenThrowsArgumentOutOfRangeException(double input)
            => Assert.That(
                () => DateTimeExtensions.FromUnixTime(input),
                Throws.TypeOf<ArgumentOutOfRangeException>());

        [TestCase("-1")]
        [TestCase("-0.001")]
        [TestCase("-86400")]
        [TestCase("-3600")]
        [TestCase("-60")]
        [TestCase("-1000000000")]
        [TestCase("-1.5")]
        [TestCase("-0.0001")]
        public void GivenNegativeUnixTimeString_WhenConvertingFromUnixTime_ThenThrowsArgumentOutOfRangeException(string input)
            => Assert.That(
                () => DateTimeExtensions.FromUnixTime(input),
                Throws.TypeOf<ArgumentOutOfRangeException>());

        [Test]
        public void GivenUnixTimeStringWithScientificNotation_WhenConvertingFromUnixTime_ThenReturnsCorrectDate()
        {
            DateTime expected = new(1970, 1, 1, 0, 0, 1);
            DateTime actual = DateTimeExtensions.FromUnixTime("1e0");

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GivenLargeUnixTime_WhenConvertingFromUnixTime_ThenReturnsCorrectDate()
        {
            DateTime expected = new(9999, 12, 31, 23, 59, 59);
            DateTime actual = DateTimeExtensions.FromUnixTime(253402300799);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GivenUnixTimeWithMilliseconds_WhenConvertingFromUnixTime_ThenPreservesMilliseconds()
        {
            DateTime expected = new(1970, 1, 1, 0, 0, 0, 123, DateTimeKind.Utc);
            DateTime actual = DateTimeExtensions.FromUnixTime(0.123);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GivenUnixTimeStringWithMilliseconds_WhenConvertingFromUnixTime_ThenPreservesMilliseconds()
        {
            DateTime expected = new(1970, 1, 1, 0, 0, 0, 456, DateTimeKind.Utc);
            DateTime actual = DateTimeExtensions.FromUnixTime("0.456");

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GivenDateTimeWithUtcKind_WhenGettingElapsedUnixTime_ThenReturnsCorrectDuration()
        {
            DateTime utcTime = new(2001, 09, 11, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan expected = new(10001664000000000);
            TimeSpan actual = DateTimeExtensions.GetElapsedUnixTime(utcTime);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GivenDateTimeWithUnspecifiedKind_WhenGettingElapsedUnixTime_ThenTreatsAsUtc()
        {
            DateTime unspecifiedTime = new(2001, 09, 11, 0, 0, 0, DateTimeKind.Unspecified);
            TimeSpan expected = new(10001664000000000);
            TimeSpan actual = DateTimeExtensions.GetElapsedUnixTime(unspecifiedTime);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GivenDateTimeWithLocalKind_WhenGettingElapsedUnixTime_ThenTreatsAsUtc()
        {
            DateTime localTime = new(2001, 09, 11, 0, 0, 0, DateTimeKind.Local);
            TimeSpan expected = new(10001664000000000);
            TimeSpan actual = DateTimeExtensions.GetElapsedUnixTime(localTime);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GivenDateTimeWithMilliseconds_WhenGettingElapsedUnixTime_ThenIncludesMilliseconds()
        {
            DateTime time = new(1970, 1, 1, 0, 0, 0, 500, DateTimeKind.Utc);
            TimeSpan expected = TimeSpan.FromMilliseconds(500);
            TimeSpan actual = DateTimeExtensions.GetElapsedUnixTime(time);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GivenDateTimeExactlyAtEpoch_WhenGettingElapsedUnixTime_ThenReturnsZero()
        {
            DateTime epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan actual = DateTimeExtensions.GetElapsedUnixTime(epoch);

            Assert.That(actual, Is.EqualTo(TimeSpan.Zero));
        }

        [Test]
        public void GivenEmptyString_WhenConvertingFromUnixTime_ThenThrowsArgumentException()
            => Assert.That(
                () => DateTimeExtensions.FromUnixTime(""),
                Throws.TypeOf<ArgumentException>());

        [Test]
        public void GivenWhitespaceOnlyString_WhenConvertingFromUnixTime_ThenThrowsArgumentException()
            => Assert.That(
                () => DateTimeExtensions.FromUnixTime("   "),
                Throws.TypeOf<ArgumentException>());
    }
}
