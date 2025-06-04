using System;

using NUnit.Framework;

namespace NuciExtensions.UnitTests
{
    public class DateTimeExtensionsTests
    {
        [Test]
        public void GetElapsedUnixTime_DateIsValidUnixDate_CorrectValueReturned()
        {
            TimeSpan expected = new(10001664000000000);
            TimeSpan actual = DateTimeExtensions.GetElapsedUnixTime(new(2001, 09, 11));

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void GetElapsedUnixTime_DateIsNotValidUnixDate_ThrowsArgumentOutOfRangeException()
            => Assert.Throws<ArgumentOutOfRangeException>(() => DateTimeExtensions.GetElapsedUnixTime(new(1917, 03, 08)));

        [Test]
        public void FromUnixTime_CalledWithString_ParameterIsValid_ReturnsCorrectValue()
        {
            DateTime expected = new(1970, 1, 8, 2, 31, 13);
            DateTime actual = DateTimeExtensions.FromUnixTime("613873");

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void FromUnixTime_CalledWithString_ParameterIsNotValid_ThrowsArgumentException()
            => Assert.Throws<ArgumentException>(() => DateTimeExtensions.FromUnixTime("bs"));

        [Test]
        public void FromUnixTime_CalledWithDouble_ReturnsCorrectValue()
        {
            DateTime expected = new(2251, 12, 9, 20, 3, 51);
            DateTime actual = DateTimeExtensions.FromUnixTime(8897112231);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
