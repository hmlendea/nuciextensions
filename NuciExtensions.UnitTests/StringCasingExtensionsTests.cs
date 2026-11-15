using System;
using System.Globalization;

using NUnit.Framework;

using NuciExtensions.UnitTests.Helpers;

namespace NuciExtensions.UnitTests
{
    [TestFixture]
    public sealed class StringCasingExtensionsTests
    {
        private static readonly string[] CultureSensitivitySamples =
        [
            string.Empty,
            "iki",
            "IKI",
            "i",
            "I",
            "\u0130stanbul",
            "D\u0130YARBAKIR",
            "\u0131I",
            "I\u0131",
            "Praise the Sun!",
            "  Hello_World-Test  ",
            "hello-world",
            "a  b",
        ];

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

        [Test]
        [TestCase("", "")]
        [TestCase("iki", "Iki")]
        [TestCase("IKI", "Iki")]
        [TestCase("i", "I")]
        [TestCase("I", "I")]
        [TestCase("İstanbul", "İstanbul")]
        [TestCase("DİYARBAKIR", "Dİyarbakir")]
        [TestCase("ıI", "ıi")]
        [TestCase("Iı", "Iı")]
        [TestCase("Praise the Sun!", "Praise The Sun!")]
        public void GivenACultureSensitiveAlphabet_WhenConvertingToTitleCase_ThenTheInvariantResultIsReturned(
            string text,
            string expectedResult)
            => Assert.That(text.ToTitleCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("", "")]
        [TestCase("iki", "Iki")]
        [TestCase("IKI", "IKI")]
        [TestCase("i", "I")]
        [TestCase("I", "I")]
        [TestCase("İstanbul", "İstanbul")]
        [TestCase("DİYARBAKIR", "DİYARBAKIR")]
        [TestCase("ıI", "ıI")]
        [TestCase("Praise the Sun!", "Praise the Sun!")]
        public void GivenACultureSensitiveAlphabet_WhenConvertingToSentenceCase_ThenTheInvariantResultIsReturned(
            string text,
            string expectedResult)
            => Assert.That(text.ToSentenceCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("iki", "iki")]
        [TestCase("IKI", "IKI")]
        [TestCase("İstanbul", "İstanbul")]
        [TestCase("DİYARBAKIR", "DİYARBAKIR")]
        [TestCase("ıI", "ıI")]
        public void GivenACultureSensitiveAlphabet_WhenConvertingToSnakeCase_ThenTheLettersAreCopiedUnchanged(
            string text,
            string expectedResult)
            => Assert.That(text.ToSnakeCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("iki", "IKI")]
        [TestCase("İstanbul", "İSTANBUL")]
        [TestCase("DİYARBAKIR", "DİYARBAKIR")]
        [TestCase("ıI", "ıI")]
        [TestCase("ı", "ı")]
        public void GivenACultureSensitiveAlphabet_WhenConvertingToUpperSnakeCase_ThenTheInvariantResultIsReturned(
            string text,
            string expectedResult)
            => Assert.That(text.ToUpperSnakeCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("IKI", "iki")]
        [TestCase("İstanbul", "İstanbul")]
        [TestCase("DİYARBAKIR", "dİyarbakir")]
        [TestCase("ıI", "ıi")]
        [TestCase("I", "i")]
        public void GivenACultureSensitiveAlphabet_WhenConvertingToLowerSnakeCase_ThenTheInvariantResultIsReturned(
            string text,
            string expectedResult)
            => Assert.That(text.ToLowerSnakeCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("tr-TR")]
        [TestCase("az-AZ")]
        [TestCase("az-Latn-AZ")]
        [TestCase("lt-LT")]
        [TestCase("de-DE")]
        [TestCase("en-US")]
        [TestCase("fr-FR")]
        public void GivenANonInvariantCulture_WhenConvertingCasing_ThenTheResultMatchesTheInvariantResult(string cultureName)
        {
            string[] expectedTitle = new string[CultureSensitivitySamples.Length];
            string[] expectedSentence = new string[CultureSensitivitySamples.Length];
            string[] expectedSnake = new string[CultureSensitivitySamples.Length];
            string[] expectedUpperSnake = new string[CultureSensitivitySamples.Length];
            string[] expectedLowerSnake = new string[CultureSensitivitySamples.Length];

            using (new CultureScope(CultureInfo.InvariantCulture))
            {
                for (int i = 0; i < CultureSensitivitySamples.Length; i++)
                {
                    expectedTitle[i] = CultureSensitivitySamples[i].ToTitleCase();
                    expectedSentence[i] = CultureSensitivitySamples[i].ToSentenceCase();
                    expectedSnake[i] = CultureSensitivitySamples[i].ToSnakeCase();
                    expectedUpperSnake[i] = CultureSensitivitySamples[i].ToUpperSnakeCase();
                    expectedLowerSnake[i] = CultureSensitivitySamples[i].ToLowerSnakeCase();
                }
            }

            using (new CultureScope(cultureName))
            {
                for (int i = 0; i < CultureSensitivitySamples.Length; i++)
                {
                    Assert.Multiple(() =>
                    {
                        Assert.That(CultureSensitivitySamples[i].ToTitleCase(), Is.EqualTo(expectedTitle[i]));
                        Assert.That(CultureSensitivitySamples[i].ToSentenceCase(), Is.EqualTo(expectedSentence[i]));
                        Assert.That(CultureSensitivitySamples[i].ToSnakeCase(), Is.EqualTo(expectedSnake[i]));
                        Assert.That(CultureSensitivitySamples[i].ToUpperSnakeCase(), Is.EqualTo(expectedUpperSnake[i]));
                        Assert.That(CultureSensitivitySamples[i].ToLowerSnakeCase(), Is.EqualTo(expectedLowerSnake[i]));
                    });
                }
            }
        }

        [Test]
        public void GivenACompletedCultureScope_WhenTheScopeIsDisposed_ThenThePreviousCulturesAreRestored()
        {
            CultureInfo originalCulture = CultureInfo.CurrentCulture;
            CultureInfo originalUICulture = CultureInfo.CurrentUICulture;

            using (new CultureScope("tr-TR"))
            {
                Assert.That(CultureInfo.CurrentCulture.Name, Is.EqualTo("tr-TR"));
                Assert.That(CultureInfo.CurrentUICulture.Name, Is.EqualTo("tr-TR"));
                Assert.That("iki".ToTitleCase(), Is.EqualTo("Iki"));
            }

            Assert.Multiple(() =>
            {
                Assert.That(CultureInfo.CurrentCulture, Is.EqualTo(originalCulture));
                Assert.That(CultureInfo.CurrentUICulture, Is.EqualTo(originalUICulture));
            });
        }

        [Test]
        public void GivenACultureScope_WhenDisposedMoreThanOnce_ThenTheOriginalCultureIsStillRestored()
        {
            CultureInfo originalCulture = CultureInfo.CurrentCulture;

            CultureScope scope = new("tr-TR");

            using (scope)
            {
                Assert.That(CultureInfo.CurrentCulture.Name, Is.EqualTo("tr-TR"));
            }

            scope.Dispose();

            Assert.That(CultureInfo.CurrentCulture, Is.EqualTo(originalCulture));
        }

        [Test]
        public void GivenANullCulture_WhenCreatingACultureScope_ThenAnArgumentNullExceptionIsThrown()
            => Assert.That(
                () => _ = new CultureScope((CultureInfo)null!),
                Throws.TypeOf<ArgumentNullException>());

        [Test]
        [TestCase("a\U0001F3AEb")]
        [TestCase("\U0001F3AE")]
        [TestCase("\U0001F3AEhello")]
        [TestCase("hello\U0001F3AE")]
        public void GivenAStringContainingSurrogatePairs_WhenConvertingCasing_ThenNoUnpairedSurrogateIsProduced(string text)
        {
            Assert.Multiple(() =>
            {
                AssertNoUnpairedSurrogates(text.ToTitleCase());
                AssertNoUnpairedSurrogates(text.ToSentenceCase());
                AssertNoUnpairedSurrogates(text.ToSnakeCase());
                AssertNoUnpairedSurrogates(text.ToUpperSnakeCase());
                AssertNoUnpairedSurrogates(text.ToLowerSnakeCase());
            });
        }

        [Test]
        [TestCase("a\U0001F3AEb", "A\U0001F3AEB")]
        [TestCase("\U0001F3AEhello", "\U0001F3AEHello")]
        [TestCase("hello\U0001F3AE", "Hello\U0001F3AE")]
        [TestCase("\U0001F3AE", "\U0001F3AE")]
        public void GivenAStringContainingSurrogatePairs_WhenConvertingToTitleCase_ThenThePairIsPreserved(
            string text,
            string expectedResult)
            => Assert.That(text.ToTitleCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("a\U0001F3AEb", "A\U0001F3AEb")]
        [TestCase("\U0001F3AEhello", "\U0001F3AEHello")]
        [TestCase("hello\U0001F3AE", "Hello\U0001F3AE")]
        public void GivenAStringContainingSurrogatePairs_WhenConvertingToSentenceCase_ThenThePairIsPreserved(
            string text,
            string expectedResult)
            => Assert.That(text.ToSentenceCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("a\U0001F3AEb", "ab")]
        [TestCase("\U0001F3AEhello", "hello")]
        [TestCase("hello\U0001F3AE", "hello")]
        [TestCase("\U0001F3AE", "")]
        public void GivenAStringContainingSurrogatePairs_WhenConvertingToSnakeCase_ThenTheNonAlphanumericPairIsRemoved(
            string text,
            string expectedResult)
            => Assert.That(text.ToSnakeCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("a\U0001F3AEb", "AB")]
        [TestCase("\U0001F3AEhello", "HELLO")]
        [TestCase("hello\U0001F3AE", "HELLO")]
        [TestCase("\U0001F3AE", "")]
        public void GivenAStringContainingSurrogatePairs_WhenConvertingToUpperSnakeCase_ThenTheNonAlphanumericPairIsRemoved(
            string text,
            string expectedResult)
            => Assert.That(text.ToUpperSnakeCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("a\U0001F3AEb", "ab")]
        [TestCase("\U0001F3AEhello", "hello")]
        [TestCase("hello\U0001F3AE", "hello")]
        [TestCase("\U0001F3AE", "")]
        public void GivenAStringContainingSurrogatePairs_WhenConvertingToLowerSnakeCase_ThenTheNonAlphanumericPairIsRemoved(
            string text,
            string expectedResult)
            => Assert.That(text.ToLowerSnakeCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("a\u00A0b", "a_b")]
        [TestCase("a\u3000b", "a_b")]
        [TestCase("a\u2003b", "a_b")]
        [TestCase("a\u1680b", "a_b")]
        [TestCase("a\u205Fb", "a_b")]
        [TestCase("a\u000Bb", "a_b")]
        [TestCase("a\u000Cb", "a_b")]
        [TestCase("a\u0085b", "a_b")]
        public void GivenAStringWithUnicodeWhitespace_WhenConvertingToSnakeCase_ThenTheWhitespaceBecomesOneUnderscore(
            string text,
            string expectedResult)
            => Assert.That(text.ToSnakeCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("a\u200Bb")]
        [TestCase("a\uFEFFb")]
        public void GivenAStringWithZeroWidthCharacters_WhenConvertingToSnakeCase_ThenTheyAreRemoved(string text)
            => Assert.That(text.ToSnakeCase(), Is.EqualTo("ab"));

        [Test]
        [TestCase("a-b  c", "ab_c")]
        public void GivenAStringWithSeparatorsAndRepeatedWhitespace_WhenConvertingToSnakeCase_ThenSeparatorsAreDroppedAndWhitespaceCollapsed(string text, string expectedResult)
            => Assert.That(text.ToSnakeCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("a-b  c", "AB_C")]
        public void GivenAStringWithSeparatorsAndRepeatedWhitespace_WhenConvertingToUpperSnakeCase_ThenTheInvariantUppercaseIsReturned(string text, string expectedResult)
            => Assert.That(text.ToUpperSnakeCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("a-b  c", "ab_c")]
        public void GivenAStringWithSeparatorsAndRepeatedWhitespace_WhenConvertingToLowerSnakeCase_ThenTheInvariantLowercaseIsReturned(string text, string expectedResult)
            => Assert.That(text.ToLowerSnakeCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("stra\u00DFe", "Stra\u00DFe")]
        [TestCase("Fu\u00DFball", "Fu\u00DFball")]
        [TestCase("\u00DF", "\u00DF")]
        [TestCase("\u1E9E", "\u00DF")]
        public void GivenAGermanSharpS_WhenConvertingToTitleCase_ThenTheResultIsCultureInvariant(
            string text,
            string expectedResult)
            => Assert.That(text.ToTitleCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("\u1E9E", "\u00DF")]
        [TestCase("\u00DF", "\u00DF")]
        public void GivenAGermanSharpS_WhenConvertingToLowerSnakeCase_ThenTheResultIsCultureInvariant(
            string text,
            string expectedResult)
            => Assert.That(text.ToLowerSnakeCase(), Is.EqualTo(expectedResult));

        [Test]
        [TestCase("3.14 and more", "3.14 And more")]
        [TestCase("first.\nsecond", "First.\nSecond")]
        [TestCase("hello? world!", "Hello? world!")]
        public void GivenAString_WhenConvertingToSentenceCase_ThenOnlyPeriodsStartANewSentence(
            string text,
            string expectedResult)
            => Assert.That(text.ToSentenceCase(), Is.EqualTo(expectedResult));

        private static void AssertNoUnpairedSurrogates(string value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (char.IsHighSurrogate(value[i]))
                {
                    Assert.That(i + 1, Is.LessThan(value.Length), $"Unpaired high surrogate at index {i}.");
                    Assert.That(char.IsLowSurrogate(value[i + 1]), Is.True, $"High surrogate at index {i} is not followed by a low surrogate.");
                    i++;
                }
                else
                {
                    Assert.That(char.IsLowSurrogate(value[i]), Is.False, $"Unpaired low surrogate at index {i}.");
                }
            }
        }
    }
}
