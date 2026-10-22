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
        [TestCase("hello world", "Hello World")]
        [TestCase("HELLO WORLD", "Hello World")]
        [TestCase("hELLO wORLD", "Hello World")]
        [TestCase("  leading spaces", "  Leading Spaces")]
        [TestCase("trailing spaces  ", "Trailing Spaces  ")]
        [TestCase("multiple   spaces", "Multiple   Spaces")]
        [TestCase("UPPERCASE", "Uppercase")]
        [TestCase("lowercase", "Lowercase")]
        [TestCase("MiXeD CaSe", "Mixed Case")]
        public void GivenAString_WhenConvertingToTitleCase_ThenEveryWordIsCapitalised(string text, string expectedResult)
            => Assert.That(text.ToTitleCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("", "")]
        [TestCase("this one surely passes", "This one surely passes")]
        [TestCase("not today. praise the Sun!", "Not today. Praise the Sun!")]
        [TestCase("Praise the Sun!", "Praise the Sun!")]
        [TestCase("hello. world!", "Hello. World!")]
        [TestCase("multiple. sentences. here.", "Multiple. Sentences. Here.")]
        [TestCase("already Capitalized.", "Already Capitalized.")]
        [TestCase("UPPERCASE SENTENCE.", "UPPERCASE SENTENCE.")]
        [TestCase("  leading space.", "  Leading space.")]
        [TestCase("trailing space.  ", "Trailing space.  ")]
        public void GivenAString_WhenConvertingToSentenceCase_ThenEverySentenceIsCapitalised(
            string text,
            string expectedResult)
            => Assert.That(text.ToSentenceCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("", "")]
        [TestCase("Praise the Sun!", "Praise_the_Sun")]
        [TestCase("Praise   the Sun!", "Praise_the_Sun")]
        [TestCase("hello world", "hello_world")]
        [TestCase("HelloWorld", "HelloWorld")]
        [TestCase("helloWorld", "helloWorld")]
        [TestCase("HELLO WORLD", "HELLO_WORLD")]
        [TestCase("UPPERCASE", "UPPERCASE")]
        [TestCase("lowercase", "lowercase")]
        public void GivenAString_WhenConvertingToSnakeCase_ThenEveryWordIsSeparatedByUnderscore(string text, string expectedResult)
            => Assert.That(text.ToSnakeCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("Praise the Sun!", "PRAISE_THE_SUN")]
        [TestCase("hello world", "HELLO_WORLD")]
        [TestCase("HelloWorld", "HELLOWORLD")]
        [TestCase("helloWorld", "HELLOWORLD")]
        [TestCase("UPPERCASE", "UPPERCASE")]
        [TestCase("lowercase", "LOWERCASE")]
        public void GivenAString_WhenConvertingToUpperSnakeCase_ThenEveryWordIsUppercaseAndSeparatedByUnderscore(string text, string expectedResult)
            => Assert.That(text.ToUpperSnakeCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("Praise the Sun!", "praise_the_sun")]
        [TestCase("hello world", "hello_world")]
        [TestCase("HelloWorld", "helloworld")]
        [TestCase("helloWorld", "helloworld")]
        [TestCase("UPPERCASE", "uppercase")]
        [TestCase("lowercase", "lowercase")]
        public void GivenAString_WhenConvertingToLowerSnakeCase_ThenEveryWordIsLowercaseAndSeparatedByUnderscore(string text, string expectedResult)
            => Assert.That(text.ToLowerSnakeCase(), Is.EqualTo(expectedResult));

        [Test]
        public void GivenANullString_WhenConvertingToTitleCase_ThenAnArgumentNullExceptionIsThrown()
            => Assert.That(
                () => ((string)null).ToTitleCase(),
                Throws.TypeOf<System.ArgumentNullException>());

        [Test]
        public void GivenANullString_WhenConvertingToSentenceCase_ThenAnArgumentNullExceptionIsThrown()
            => Assert.That(
                () => ((string)null).ToSentenceCase(),
                Throws.TypeOf<System.ArgumentNullException>());

        [Test]
        public void GivenANullString_WhenConvertingToSnakeCase_ThenAnArgumentNullExceptionIsThrown()
            => Assert.That(
                () => ((string)null).ToSnakeCase(),
                Throws.TypeOf<System.ArgumentNullException>());

        [Test]
        public void GivenANullString_WhenConvertingToUpperSnakeCase_ThenAnArgumentNullExceptionIsThrown()
            => Assert.That(
                () => ((string)null).ToUpperSnakeCase(),
                Throws.TypeOf<System.ArgumentNullException>());

        [Test]
        public void GivenANullString_WhenConvertingToLowerSnakeCase_ThenAnArgumentNullExceptionIsThrown()
            => Assert.That(
                () => ((string)null).ToLowerSnakeCase(),
                Throws.TypeOf<System.ArgumentNullException>());
    }
}
