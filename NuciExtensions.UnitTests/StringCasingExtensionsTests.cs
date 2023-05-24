using NUnit.Framework;

namespace NuciExtensions.UnitTests
{
    public class StringCasingExtensionsTests
    {
        [Test]
        [TestCase("my text is in all lower case", "My Text Is In All Lower Case")]
        [TestCase("my-Text-with-Dashes", "My-Text-With-Dashes")]
        [TestCase("my text.with periods", "My Text.With Periods")]
        public void GivenAString_WhenConvertingToTitleCase_ThenEveryWordIsCapitalised(string text, string expectedResult)
            => Assert.That(text.ToTitleCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("this is a random sentance", "This is a random sentance")]
        [TestCase("there are two. two sentances", "There are two. Two sentances")]
        [TestCase("the words that are Already Capitalised remain CAPITALISED", "The words that are Already Capitalised remain CAPITALISED")]
        public void GivenAString_WhenConvertingToSentanceCase_ThenEveryWordIsCapitalised(string text, string expectedResult)
            => Assert.That(text.ToSentanceCase(), Is.EqualTo(expectedResult));
    }
}
