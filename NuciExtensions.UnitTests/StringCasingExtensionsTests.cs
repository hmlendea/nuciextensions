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
        [TestCase("this is a random sentence", "This is a random sentence")]
        [TestCase("there are two. two sentences", "There are two. Two sentences")]
        [TestCase("the words that are Already Capitalised remain CAPITALISED", "The words that are Already Capitalised remain CAPITALISED")]
        public void GivenAString_WhenConvertingToSentenceCase_ThenEveryWordIsCapitalised(string text, string expectedResult)
            => Assert.That(text.ToSentenceCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("i want This to be in snake case", "i_want_This_to_be_in_snake_case")]
        public void GivenAString_WhenConvertingToSnakeCase_ThenEveryWordIsSeparatedByUnderscore(string text, string expectedResult)
            => Assert.That(text.ToSnakeCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("this is so GonNa be upper snake case", "THIS_IS_SO_GONNA_BE_UPPER_SNAKE_CASE")]
        public void GivenAString_WhenConvertingToUpperSnakeCase_ThenEveryWordIsUppercaseAndSeparatedByUnderscore(string text, string expectedResult)
            => Assert.That(text.ToUpperSnakeCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("lower SNAKE Case please", "lower_snake_case_please")]
        public void GivenAString_WhenConvertingToLowerSnakeCase_ThenEveryWordIsLowercaseAndSeparatedByUnderscore(string text, string expectedResult)
            => Assert.That(text.ToLowerSnakeCase(), Is.EqualTo(expectedResult));
    }
}
