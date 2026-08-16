using NUnit.Framework;

namespace NuciExtensions.UnitTests
{
    [TestFixture]
    public sealed class StringCasingExtensionsTests
    {
        [Test]
        [TestCase("", "")]
        [TestCase("Praise the Sun!", "Praise The Sun!")]
        [TestCase("Are you, too, looking for your own sun?", "Are You, Too, Looking For Your Own Sun?")]
        [TestCase("One Ring to rule them all. One Ring to find them", "One Ring To Rule Them All. One Ring To Find Them")]
        public void GivenAString_WhenConvertingToTitleCase_ThenEveryWordIsCapitalised(string text, string expectedResult)
            => Assert.That(text.ToTitleCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("", "")]
        [TestCase("this one surely passes", "This one surely passes")]
        [TestCase("not today. praise the Sun!", "Not today. Praise the Sun!")]
        [TestCase("Praise the Sun!", "Praise the Sun!")]
        public void GivenAString_WhenConvertingToSentenceCase_ThenEverySentenceIsCapitalised(
            string text,
            string expectedResult)
            => Assert.That(text.ToSentenceCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("", "")]
        [TestCase("Praise the Sun!", "Praise_the_Sun")]
        [TestCase("Praise   the Sun!", "Praise_the_Sun")]
        public void GivenAString_WhenConvertingToSnakeCase_ThenEveryWordIsSeparatedByUnderscore(string text, string expectedResult)
            => Assert.That(text.ToSnakeCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("Praise the Sun!", "PRAISE_THE_SUN")]
        public void GivenAString_WhenConvertingToUpperSnakeCase_ThenEveryWordIsUppercaseAndSeparatedByUnderscore(string text, string expectedResult)
            => Assert.That(text.ToUpperSnakeCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("Praise the Sun!", "praise_the_sun")]
        public void GivenAString_WhenConvertingToLowerSnakeCase_ThenEveryWordIsLowercaseAndSeparatedByUnderscore(string text, string expectedResult)
            => Assert.That(text.ToLowerSnakeCase(), Is.EqualTo(expectedResult));
    }
}
