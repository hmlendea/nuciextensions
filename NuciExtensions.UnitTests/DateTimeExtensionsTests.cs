using System;

using NUnit.Framework;

using NuciExtensions;

namespace NuciExtensions.UnitTests
{
    public class DateTimeExtensionsTests
    {
        [Test]
        public void GetElapsedUnixTime_DateIsValidUnixDate_CorrectValueReturned()
        {
            DateTime validDate = new DateTime(2001, 09, 11);

            TimeSpan expected = new TimeSpan(10001556000000000);
            TimeSpan actual = DateTimeExtensions.GetElapsedUnixTime(validDate);

            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void GetElapsedUnixTime_DateIsNotValidUnixDate_ThrowsArgumentOutOfRangeException()
        {
            DateTime invalidDate = new DateTime(1917, 03, 08);

            Assert.Throws<ArgumentOutOfRangeException>(() => DateTimeExtensions.GetElapsedUnixTime(invalidDate));
        }
    }
}
