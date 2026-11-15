using System.Globalization;

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
        [TestCase("already Title Case", "Already Title Case")]
        [TestCase("with-numbers 123", "With-Numbers 123")]
        [TestCase("special_chars!", "Special_Chars!")]
        [TestCase("unicode café", "Unicode Café")]
        [TestCase("tabs\tand\nnewlines", "Tabs\tAnd\nNewlines")]
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
        [TestCase("hello! world?", "Hello! world?")]
        [TestCase("multiple... ellipsis", "Multiple... Ellipsis")]
        [TestCase("japanese。 sentence。", "Japanese。 sentence。")]
        [TestCase("already. Sentence.", "Already. Sentence.")]
        [TestCase("UPPERCASE! LOWERCASE?", "UPPERCASE! LOWERCASE?")]
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
        [TestCase("hello-world", "helloworld")]
        [TestCase("hello.world", "helloworld")]
        [TestCase("  spaces  ", "_spaces_")]
        [TestCase("multiple___underscores", "multipleunderscores")]
        [TestCase("PascalCase", "PascalCase")]
        [TestCase("camelCase", "camelCase")]
        [TestCase("XMLHttpRequest", "XMLHttpRequest")]
        [TestCase("snake_case", "snakecase")]
        [TestCase("kebab-case", "kebabcase")]
        public void GivenAString_WhenConvertingToSnakeCase_ThenEveryWordIsSeparatedByUnderscore(string text, string expectedResult)
            => Assert.That(text.ToSnakeCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("_", "")]
        [TestCase("__", "")]
        [TestCase("___", "")]
        [TestCase("a_", "a")]
        [TestCase("_a", "a")]
        [TestCase("a__b", "ab")]
        [TestCase("a_ _b", "a_b")]
        [TestCase("a \t b", "a_b")]
        [TestCase("a \t\t b", "a_b")]
        [TestCase("a\t\tb", "a_b")]
        [TestCase("a  b", "a_b")]
        [TestCase("a   b", "a_b")]
        [TestCase("a - b", "a_b")]
        [TestCase("a.b.c", "abc")]
        [TestCase("a,b;c:d", "abcd")]
        [TestCase("a!@#$%^&*()b", "ab")]
        [TestCase("a/b\\b", "abb")]
        [TestCase("a_b-c.d", "abcd")]
        [TestCase("a___b___c", "abc")]
        [TestCase("   ", "_")]
        [TestCase("\t\t", "_")]
        [TestCase("a\nb", "a_b")]
        [TestCase("a\r\nb", "a_b")]
        [TestCase("héllo", "héllo")]
        [TestCase("héllo wörld", "héllo_wörld")]
        [TestCase("日本語", "日本語")]
        [TestCase("日本 語", "日本_語")]
        public void GivenAString_WhenConvertingToSnakeCase_ThenSeparatorsAreRemovedAndWhitespaceIsCollapsed(string text, string expectedResult)
            => Assert.That(text.ToSnakeCase(), Is.EqualTo(expectedResult));

        [Test]
        public void GivenAStringContainingOnlySeparators_WhenConvertingToSnakeCase_ThenAnEmptyStringIsReturned()
            => Assert.That("___---...".ToSnakeCase(), Is.Empty);

        [Test]
        public void GivenAStringWithMixedSeparators_WhenConvertingToSnakeCase_ThenAllNonAlphanumericsAreRemovedExceptCollapsedWhitespace()
            => Assert.That("hello-world_foo.bar baz".ToSnakeCase(), Is.EqualTo("helloworldfoobar_baz"));

        [Test]
        public void GivenAnEmptyString_WhenConvertingToSnakeCase_ThenAnEmptyStringIsReturned()
            => Assert.That(string.Empty.ToSnakeCase(), Is.Empty);

        [Test]
        public void GivenASingleCharacterString_WhenConvertingToSnakeCase_ThenTheCharacterIsReturned()
        {
            Assert.That("a".ToSnakeCase(), Is.EqualTo("a"));
            Assert.That("1".ToSnakeCase(), Is.EqualTo("1"));
            Assert.That("_".ToSnakeCase(), Is.Empty);
        }

        [Test]
        public void GivenAStringWithNonAsciiLetters_WhenConvertingToSnakeCase_ThenLettersArePreserved()
            => Assert.That("café résumé".ToSnakeCase(), Is.EqualTo("café_résumé"));

        [Test]
        public void GivenAStringWithLeadingAndTrailingWhitespace_WhenConvertingToSnakeCase_ThenUnderscoresAreAddedAtTheBoundaries()
            => Assert.That("  hello  ".ToSnakeCase(), Is.EqualTo("_hello_"));

        [Test]
        [TestCase("Praise the Sun!", "PRAISE_THE_SUN")]
        [TestCase("hello world", "HELLO_WORLD")]
        [TestCase("HelloWorld", "HELLOWORLD")]
        [TestCase("helloWorld", "HELLOWORLD")]
        [TestCase("UPPERCASE", "UPPERCASE")]
        [TestCase("lowercase", "LOWERCASE")]
        [TestCase("hello-world", "HELLOWORLD")]
        [TestCase("hello.world", "HELLOWORLD")]
        [TestCase("  spaces  ", "_SPACES_")]
        [TestCase("multiple___underscores", "MULTIPLEUNDERSCORES")]
        [TestCase("PascalCase", "PASCALCASE")]
        [TestCase("camelCase", "CAMELCASE")]
        [TestCase("XMLHttpRequest", "XMLHTTPREQUEST")]
        [TestCase("snake_case", "SNAKECASE")]
        [TestCase("kebab-case", "KEBABCASE")]
        public void GivenAString_WhenConvertingToUpperSnakeCase_ThenEveryWordIsUppercaseAndSeparatedByUnderscore(string text, string expectedResult)
            => Assert.That(text.ToUpperSnakeCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("Praise the Sun!", "praise_the_sun")]
        [TestCase("hello world", "hello_world")]
        [TestCase("HelloWorld", "helloworld")]
        [TestCase("helloWorld", "helloworld")]
        [TestCase("UPPERCASE", "uppercase")]
        [TestCase("lowercase", "lowercase")]
        [TestCase("hello-world", "helloworld")]
        [TestCase("hello.world", "helloworld")]
        [TestCase("  spaces  ", "_spaces_")]
        [TestCase("multiple___underscores", "multipleunderscores")]
        [TestCase("PascalCase", "pascalcase")]
        [TestCase("camelCase", "camelcase")]
        [TestCase("XMLHttpRequest", "xmlhttprequest")]
        [TestCase("snake_case", "snakecase")]
        [TestCase("kebab-case", "kebabcase")]
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

        [Test]
        [SetCulture("tr-TR")]
        [TestCase("iki", "Iki")]
        [TestCase("IKI", "Iki")]
        [TestCase("i", "I")]
        [TestCase("I", "I")]
        [TestCase("istanbul", "Istanbul")]
        [TestCase("İstanbul", "İstanbul")]
        [TestCase("", "")]
        public void GivenATurkishCulture_WhenConvertingToTitleCase_ThenTheResultIsCultureInvariant(
            string text,
            string expectedResult)
            => Assert.That(text.ToTitleCase(), Is.EqualTo(expectedResult));

        [Test]
        [SetCulture("tr-TR")]
        [TestCase("iki", "IKI")]
        [TestCase("i", "I")]
        [TestCase("istanbul", "ISTANBUL")]
        [TestCase("i-stanbul", "ISTANBUL")]
        [TestCase("", "")]
        public void GivenATurkishCulture_WhenConvertingToUpperSnakeCase_ThenTheResultIsCultureInvariant(
            string text,
            string expectedResult)
            => Assert.That(text.ToUpperSnakeCase(), Is.EqualTo(expectedResult));

        [Test]
        [SetCulture("tr-TR")]
        [TestCase("IKI", "iki")]
        [TestCase("I", "i")]
        [TestCase("ISTANBUL", "istanbul")]
        [TestCase("ISI", "isi")]
        [TestCase("", "")]
        public void GivenATurkishCulture_WhenConvertingToLowerSnakeCase_ThenTheResultIsCultureInvariant(
            string text,
            string expectedResult)
            => Assert.That(text.ToLowerSnakeCase(), Is.EqualTo(expectedResult));

        [Test]
        [SetCulture("tr-TR")]
        [TestCase("iki. IKI", "Iki. IKI")]
        [TestCase("i. I", "I. I")]
        [TestCase("iki istanbul. IKI", "Iki istanbul. IKI")]
        [TestCase("", "")]
        public void GivenATurkishCulture_WhenConvertingToSentenceCase_ThenTheResultIsCultureInvariant(
            string text,
            string expectedResult)
            => Assert.That(text.ToSentenceCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("tr-TR")]
        [TestCase("en-US")]
        [TestCase("de-DE")]
        [TestCase("az-AZ")]
        [TestCase("")]
        public void GivenADifferentCulture_WhenConvertingCasing_ThenTheResultIsIdentical(string cultureName)
        {
            CultureInfo originalCulture = CultureInfo.CurrentCulture;

            try
            {
                CultureInfo.CurrentCulture = new CultureInfo(cultureName);

                Assert.Multiple(() =>
                {
                    Assert.That("iki World".ToTitleCase(), Is.EqualTo("Iki World"));
                    Assert.That("iki World".ToUpperSnakeCase(), Is.EqualTo("IKI_WORLD"));
                    Assert.That("IKI World".ToLowerSnakeCase(), Is.EqualTo("iki_world"));
                    Assert.That("iki. IKI".ToSentenceCase(), Is.EqualTo("Iki. IKI"));
                });
            }
            finally
            {
                CultureInfo.CurrentCulture = originalCulture;
            }
        }

        [Test]
        public void GivenADifferentCulture_WhenConvertingCasing_ThenTheOriginalCultureIsRestored()
        {
            CultureInfo originalCulture = CultureInfo.CurrentCulture;

            try
            {
                CultureInfo.CurrentCulture = new CultureInfo("tr-TR");

                _ = "iki".ToTitleCase();
            }
            finally
            {
                CultureInfo.CurrentCulture = originalCulture;
            }

            Assert.That(CultureInfo.CurrentCulture, Is.EqualTo(originalCulture));
        }

        [Test]
        [SetCulture("tr-TR")]
        [TestCase("iki", "IKI")]
        [TestCase("I", "i")]
        [TestCase("Hello", "hELLO")]
        public void GivenATurkishCulture_WhenInvertingTheCase_ThenTheResultIsCultureInvariant(
            string text,
            string expectedResult)
            => Assert.That(text.InvertCase(), Is.EqualTo(expectedResult));
    }
}
