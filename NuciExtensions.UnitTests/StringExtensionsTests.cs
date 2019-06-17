using NUnit.Framework;

namespace NuciExtensions.UnitTests
{
    public class StringExtensionsTests
    {
        [Test]
        public void RemoveDiacritics_ReturnsCorrectValue()
        {
            string input = "Horațiu says héllo";
            string expected = "Horatiu says hello";
            string actual = input.RemoveDiacritics();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RemovePunctuation_ReturnsCorrectValue()
        {
            string input = "Hello world! Am I greeting the world? Yes, I'm indeed greeting the world.";
            string expected = "Hello world Am I greeting the world Yes Im indeed greeting the world";
            string actual = input.RemovePunctuation();

            Assert.AreEqual(expected, actual);
        }
    }
}
