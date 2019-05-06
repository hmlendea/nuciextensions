using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using NuciExtensions;

namespace NuciExtensions.UnitTests
{
    public class StringExtensionsTests
    {
        [Test]
        public void RemoveDiacritics_ReturnsCorrectValue()
        {
            string input = "Horațiu says héllo";
            string actual = input.RemoveDiacritics();
            string expected = "Horatiu says hello";

            Assert.AreEqual(expected, actual);
        }
    }
}
