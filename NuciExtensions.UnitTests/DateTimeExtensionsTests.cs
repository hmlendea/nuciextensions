using System;

using NUnit.Framework;

namespace NuciExtensions.UnitTests
{
    public class DateTimeExtensionsTests
    {
        [Test]
        public void GetElapsedUnixTime_DateIsValidUnixDate_CorrectValueReturned()
        {
            DateTime validDate = new DateTime(2001, 09, 11);

            TimeSpan expected = new TimeSpan(10001664000000000);
            TimeSpan actual = DateTimeExtensions.GetElapsedUnixTime(validDate);

            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void GetElapsedUnixTime_DateIsNotValidUnixDate_ThrowsArgumentOutOfRangeException()
        {
            DateTime invalidDate = new DateTime(1917, 03, 08);

            Assert.Throws<ArgumentOutOfRangeException>(() => DateTimeExtensions.GetElapsedUnixTime(invalidDate));
        }
        
        [Test]
        public void FromUnixTime_CalledWithString_ParameterIsValid_ReturnsCorrectValue()
        {
            string timestamp = "613873";
            DateTime expected = new DateTime(1970, 1, 8, 2, 31, 13);
            DateTime actual = DateTimeExtensions.FromUnixTime(timestamp);

            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void FromUnixTime_CalledWithString_ParameterIsNotValid_ThrowsArgumentException()
        {
            string timestamp = "bs";

            Assert.Throws<ArgumentException>(() => DateTimeExtensions.FromUnixTime(timestamp));
        }
        
        [Test]
        public void FromUnixTime_CalledWithDouble_ReturnsCorrectValue()
        {
            double time = 8897112231;
            DateTime expected = new DateTime(2251, 12, 9, 20, 3, 51);
            DateTime actual = DateTimeExtensions.FromUnixTime(time);

            Assert.AreEqual(expected, actual);
        }
    }
}
