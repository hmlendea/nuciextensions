using System;
using System.Globalization;
using System.Text.Json;

using NUnit.Framework;

using NuciExtensions.UnitTests.Helpers;

namespace NuciExtensions.UnitTests
{
    [TestFixture]
    public sealed class StringExtensionsTests
    {
        private static readonly JsonSerializerOptions jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        [Test]
        [TestCase("Praise the Sun!", "pRAISE THE sUN!")]
        [TestCase("This was a triumph", "tHIS WAS A TRIUMPH")]
        [TestCase("e=mc²", "E=MC²")]
        [TestCase("Hello World", "hELLO wORLD")]
        [TestCase("UPPERCASE", "uppercase")]
        [TestCase("lowercase", "LOWERCASE")]
        [TestCase("MiXeD CaSe", "mIxEd cAsE")]
        [TestCase("123ABC", "123abc")]
        [TestCase("!@#$%", "!@#$%")]
        [TestCase("café", "CAFÉ")]
        public void GivenAString_WhenInvertingTheCase_ThenTheOppositeCaseIsReturned(
            string text,
            string expectedText)
            => Assert.That(text.InvertCase(), Is.EqualTo(expectedText));

        [Test]
        [TestCase("")]
        public void GivenAnEmptyString_WhenInvertingTheCase_ThenTheEmptyStringIsReturned(string text)
            => Assert.That(text.InvertCase(), Is.EqualTo(text));

        [Test]
        public void GivenANullString_WhenInvertingTheCase_ThenNullIsReturned()
            => Assert.That(((string?)null).InvertCase(), Is.Null);

        [Test]
        [TestCase("iki", "IKI")]
        [TestCase("IKI", "iki")]
        [TestCase("I", "i")]
        [TestCase("i", "I")]
        [TestCase("\u0130stanbul", "\u0130STANBUL")]
        [TestCase("D\u0130YARBAKIR", "d\u0130yarbakir")]
        public void GivenACultureSensitiveAlphabet_WhenInvertingTheCase_ThenTheInvariantResultIsReturned(
            string text,
            string expectedText)
            => Assert.That(text.InvertCase(), Is.EqualTo(expectedText));

        [Test]
        [TestCase("tr-TR")]
        [TestCase("az-AZ")]
        [TestCase("lt-LT")]
        [TestCase("de-DE")]
        [TestCase("en-US")]
        public void GivenANonInvariantCulture_WhenInvertingTheCase_ThenTheResultMatchesTheInvariantResult(string cultureName)
        {
            string[] samples = { "iki", "IKI", "I", "i", "\u0130stanbul", "D\u0130YARBAKIR", "Praise the Sun!" };

            string[] expectedResults = new string[samples.Length];

            using (new CultureScope(CultureInfo.InvariantCulture))
            {
                for (int i = 0; i < samples.Length; i++)
                {
                    expectedResults[i] = samples[i].InvertCase()!;
                }
            }

            using (new CultureScope(cultureName))
            {
                for (int i = 0; i < samples.Length; i++)
                {
                    Assert.That(samples[i].InvertCase(), Is.EqualTo(expectedResults[i]));
                }
            }
        }

        [Test]
        [TestCase("a\U0001F3AEb", "A\U0001F3AEB")]
        [TestCase("\U0001F3AEhello", "\U0001F3AEHELLO")]
        [TestCase("hello\U0001F3AE", "HELLO\U0001F3AE")]
        [TestCase("caf\u00E9", "CAF\u00C9")]
        public void GivenAStringContainingSurrogatePairs_WhenInvertingTheCase_ThenThePairIsPreserved(
            string text,
            string expectedText)
            => Assert.That(text.InvertCase(), Is.EqualTo(expectedText));

        [Test]
        [TestCase("", "")]
        [TestCase("4", "4")]
        [TestCase("Praise the Sun!", "!nuS eht esiarP")]
        [TestCase("Hello", "olleH")]
        [TestCase("racecar", "racecar")]
        [TestCase("12345", "54321")]
        [TestCase("a", "a")]
        [TestCase("café", "éfac")]
        public void GivenAString_WhenReversing_ThenTheCharactersAreReturnedInReverseOrder(
            string text,
            string expectedText)
            => Assert.That(text.Reverse(), Is.EqualTo(expectedText));

        [Test]
        public void GivenANullString_WhenReversing_ThenAnArgumentNullExceptionIsThrown()
        {
            string text = null;

            Assert.That(
                () => text.Reverse(),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        [TestCase("a🎮b", "b🎮a")]
        [TestCase("🎮", "🎮")]
        [TestCase("🎮🎯", "🎯🎮")]
        [TestCase("ab🎮", "🎮ba")]
        [TestCase("🎮ab", "ba🎮")]
        [TestCase("𝕬𝕭", "𝕭𝕬")]
        [TestCase("日本語", "語本日")]
        [TestCase("𐍈𐍉𐍊", "𐍊𐍉𐍈")]
        [TestCase("a𝕬b𝕭c", "c𝕭b𝕬a")]
        [TestCase("Praise the Sun! 🎮", "🎮 !nuS eht esiarP")]
        [TestCase("x🎮y🎯z", "z🎯y🎮x")]
        public void GivenAStringContainingSurrogatePairs_WhenReversing_ThenTheSurrogatePairsStayIntact(
            string text,
            string expectedText)
            => Assert.That(text.Reverse(), Is.EqualTo(expectedText));

        [Test]
        [TestCase("a🎮b")]
        [TestCase("🎮")]
        [TestCase("🎮🎯")]
        [TestCase("𝕬𝕭")]
        [TestCase("𐍈𐍉𐍊")]
        [TestCase("Praise the Sun! 🎮")]
        [TestCase("x🎮y🎯z")]
        public void GivenAStringContainingSurrogatePairs_WhenReversing_ThenTheResultHasNoUnpairedSurrogates(string text)
        {
            string result = text.Reverse();

            for (int i = 0; i < result.Length; i++)
            {
                if (char.IsHighSurrogate(result[i]))
                {
                    Assert.That(i + 1, Is.LessThan(result.Length));
                    Assert.That(char.IsLowSurrogate(result[i + 1]), Is.True);
                    i++;
                }
                else
                {
                    Assert.That(char.IsLowSurrogate(result[i]), Is.False);
                }
            }
        }

        [Test]
        [TestCase("a🎮b")]
        [TestCase("🎮")]
        [TestCase("🎮🎯")]
        [TestCase("𝕬𝕭")]
        [TestCase("𐍈𐍉𐍊")]
        [TestCase("Praise the Sun! 🎮")]
        [TestCase("x🎮y🎯z")]
        [TestCase("café")]
        [TestCase("Hello")]
        [TestCase("")]
        public void GivenAString_WhenReversingTwice_ThenTheOriginalStringIsReturned(string text)
            => Assert.That(text.Reverse().Reverse(), Is.EqualTo(text));

        [Test]
        [TestCase("a🎮b")]
        [TestCase("🎮🎯")]
        [TestCase("𝕬𝕭")]
        [TestCase("𐍈𐍉𐍊")]
        [TestCase("x🎮y🎯z")]
        public void GivenAStringContainingSurrogatePairs_WhenReversing_ThenTheLengthIsPreserved(string text)
            => Assert.That(text.Reverse().Length, Is.EqualTo(text.Length));

        [Test]
        public void GivenACombiningMarkSequence_WhenReversing_ThenTheCombiningMarkIsNotReorderedWithItsBase()
        {
            string text = "e\u0301x";

            string result = text.Reverse();

            Assert.That(result.Length, Is.EqualTo(text.Length));
            Assert.That(result, Is.EqualTo("x\u0301e"));
        }

        [Test]
        public void GivenAStringWithUnpairedHighSurrogateAtTheStart_WhenReversing_ThenTheSurrogateIsPreserved()
        {
            string text = "\uD800ab";

            string result = text.Reverse();

            Assert.That(result, Is.EqualTo("ba\uD800"));
            Assert.That(result.Length, Is.EqualTo(3));
        }

        [Test]
        public void GivenAStringWithUnpairedLowSurrogateAtTheStart_WhenReversing_ThenTheSurrogateIsPreserved()
        {
            string text = "\uDC00ab";

            string result = text.Reverse();

            Assert.That(result, Is.EqualTo("ba\uDC00"));
            Assert.That(result.Length, Is.EqualTo(3));
        }

        [Test]
        public void GivenAStringWithUnpairedSurrogatesInTheMiddle_WhenReversing_ThenTheSurrogatesArePreserved()
        {
            string text = "a\uD800b\uDC00c";

            string result = text.Reverse();

            Assert.That(result, Is.EqualTo("c\uDC00b\uD800a"));
        }

        [Test]
        [TestCase("hello")]
        [TestCase("café")]
        [TestCase("🎮🎯🕹️")]
        [TestCase("𝕬𝕭𝕮")]
        [TestCase("a🎮b🎯c")]
        public void GivenAString_WhenReversing_ThenTheResultOnlyContainsCharactersFromTheInput(string text)
        {
            string result = text.Reverse();

            System.Collections.Generic.Dictionary<char, int> inputChars = new();

            foreach (char c in text)
            {
                inputChars[c] = inputChars.TryGetValue(c, out int existingCount)
                    ? existingCount + 1
                    : 1;
            }

            foreach (char c in result)
            {
                Assert.That(inputChars.ContainsKey(c), Is.True);
                inputChars[c]--;
            }

            Assert.That(inputChars.Values, Is.All.EqualTo(0));
        }

        [Test]
        public void GivenAStringAndPositiveCount_WhenRepeating_ThenTheRepeatedStringIsReturned()
            => Assert.That(
                "Praise the Sun!".Repeat(4),
                Is.EqualTo("Praise the Sun!Praise the Sun!Praise the Sun!Praise the Sun!"));

        [Test]
        [TestCase("test", 1, "test")]
        [TestCase("a", 5, "aaaaa")]
        [TestCase("ab", 3, "ababab")]
        [TestCase("🎮", 2, "🎮🎮")]
        [TestCase("hello ", 3, "hello hello hello ")]
        public void GivenAStringAndPositiveCount_WhenRepeating_ThenTheRepeatedStringIsReturned_Parameterized(
            string text,
            int count,
            string expected)
            => Assert.That(text.Repeat(count), Is.EqualTo(expected));

        [Test]
        [TestCase(0)]
        [TestCase(-4)]
        public void GivenAStringAndNonPositiveCount_WhenRepeating_ThenAnEmptyStringIsReturned(int count)
            => Assert.That("Praise the Sun!".Repeat(count), Is.Empty);

        [Test]
        public void GivenANullStringAndPositiveCount_WhenRepeating_ThenAnEmptyStringIsReturned()
        {
            string text = null;

            Assert.That(text.Repeat(4), Is.Empty);
        }

        [Test]
        public void GivenASingleCharacterStringAndMaximumCount_WhenRepeating_ThenAnArgumentOutOfRangeExceptionIsThrown()
            => Assert.That(
                () => "x".Repeat(int.MaxValue),
                Throws.TypeOf<ArgumentOutOfRangeException>()
                    .With.Property(nameof(ArgumentOutOfRangeException.ParamName))
                    .EqualTo("count"));

        [Test]
        [TestCase("ab", int.MaxValue)]
        [TestCase("Praise the Sun!", int.MaxValue)]
        [TestCase("zzzzzzzzzz", int.MaxValue)]
        [TestCase("ab", 1073741796)]
        [TestCase("abc", 715827883)]
        public void GivenAStringAndACountWhoseProductExceedsTheMaximumLength_WhenRepeating_ThenAnArgumentOutOfRangeExceptionIsThrown(
            string text,
            int count)
            => Assert.That(
                () => text.Repeat(count),
                Throws.TypeOf<ArgumentOutOfRangeException>()
                    .With.Property(nameof(ArgumentOutOfRangeException.ParamName))
                    .EqualTo("count"));

        [Test]
        [TestCase("ab", int.MaxValue)]
        [TestCase("Praise the Sun!", int.MaxValue)]
        [TestCase("abc", 715827883)]
        public void GivenAStringAndACountWhoseProductExceedsTheMaximumLength_WhenRepeating_ThenTheExceptionMessageStatesTheRequestedLength(
            string text,
            int count)
        {
            long expectedLength = (long)text.Length * count;

            Assert.That(
                () => text.Repeat(count),
                Throws.TypeOf<ArgumentOutOfRangeException>()
                    .With.Message.Contains(expectedLength.ToString()));
        }

        [Test]
        [TestCase("ab", int.MaxValue)]
        [TestCase("abc", 715827883)]
        public void GivenAStringAndACountWhoseProductExceedsTheMaximumLength_WhenRepeating_ThenTheExceptionReportsTheOffendingCount(
            string text,
            int count)
            => Assert.That(
                () => text.Repeat(count),
                Throws.TypeOf<ArgumentOutOfRangeException>()
                    .With.Property(nameof(ArgumentOutOfRangeException.ActualValue))
                    .EqualTo(count));

        [Test]
        public void GivenASingleCharacterStringAndACountOneAboveTheMaximumLength_WhenRepeating_ThenAnArgumentOutOfRangeExceptionIsThrown()
            => Assert.That(
                () => "x".Repeat(Array.MaxLength + 1),
                Throws.TypeOf<ArgumentOutOfRangeException>()
                    .With.Property(nameof(ArgumentOutOfRangeException.ParamName))
                    .EqualTo("count"));

        [Test]
        public void GivenATwoCharacterStringAndACountJustAboveTheMaximumLength_WhenRepeating_ThenAnArgumentOutOfRangeExceptionIsThrown()
        {
            int count = (Array.MaxLength / 2) + 1;

            Assert.That(
                () => "ab".Repeat(count),
                Throws.TypeOf<ArgumentOutOfRangeException>()
                    .With.Property(nameof(ArgumentOutOfRangeException.ParamName))
                    .EqualTo("count"));
        }

        [Test]
        public void GivenAThreeCharacterStringAndACountJustAboveTheMaximumLength_WhenRepeating_ThenAnArgumentOutOfRangeExceptionIsThrown()
        {
            int count = (Array.MaxLength / 3) + 1;

            Assert.That(
                () => "abc".Repeat(count),
                Throws.TypeOf<ArgumentOutOfRangeException>()
                    .With.Property(nameof(ArgumentOutOfRangeException.ActualValue))
                    .EqualTo(count));
        }

        [Test]
        [TestCase("ab")]
        [TestCase("Praise the Sun!")]
        public void GivenAStringAndACountWhoseProductExceedsTheMaximumLength_WhenRepeating_ThenTheExceptionMessageStatesTheLimit(
            string text)
            => Assert.That(
                () => text.Repeat(int.MaxValue),
                Throws.TypeOf<ArgumentOutOfRangeException>()
                    .With.Message.Contains(Array.MaxLength.ToString()));

        [Test]
        public void GivenASingleCharacterStringAndMaximumCount_WhenRepeating_ThenTheExceptionMessageStatesTheSupportedMaximum()
            => Assert.That(
                () => "x".Repeat(int.MaxValue),
                Throws.TypeOf<ArgumentOutOfRangeException>()
                    .With.Message.Contains($"of {Array.MaxLength} characters"));

        [Test]
        public void GivenALongStringAndMaximumCount_WhenRepeating_ThenAnArgumentOutOfRangeExceptionIsThrownInsteadOfOverflowing()
        {
            string text = new('a', 1000);

            Assert.That(
                () => text.Repeat(int.MaxValue),
                Throws.TypeOf<ArgumentOutOfRangeException>()
                    .With.Property(nameof(ArgumentOutOfRangeException.ParamName))
                    .EqualTo("count"));
        }

        [Test]
        public void GivenALongStringAndMaximumCount_WhenRepeating_ThenTheExceptionMessageStatesTheWidenedLength()
        {
            string text = new('a', 1000);
            long expectedLength = (long)text.Length * int.MaxValue;

            Assert.That(
                () => text.Repeat(int.MaxValue),
                Throws.TypeOf<ArgumentOutOfRangeException>()
                    .With.Message.Contains(expectedLength.ToString()));
        }

        [Test]
        public void GivenAStringAndMinimumCount_WhenRepeating_ThenAnEmptyStringIsReturned()
            => Assert.That("Praise the Sun!".Repeat(int.MinValue), Is.Empty);

        [Test]
        public void GivenAStringAndACountOfOneMillion_WhenRepeating_ThenTheResultIsTheExactRepetition()
        {
            string text = "Minecraft";

            string result = text.Repeat(1_000_000);

            Assert.That(result, Has.Length.EqualTo(9_000_000));
            Assert.That(result, Does.StartWith(text));
            Assert.That(result, Does.EndWith(text));
        }

        [Test]
        [TestCase("🎮", 100000)]
        [TestCase("🎮🎯", 50000)]
        public void GivenAStringContainingSurrogatePairsAndALargeCountBelowTheMaximum_WhenRepeating_ThenTheResultLengthIsTheExactProduct(
            string text,
            int count)
            => Assert.That(
                text.Repeat(count),
                Has.Length.EqualTo(text.Length * count));

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(1000)]
        [TestCase(100000)]
        public void GivenAnEmptyStringAndAnyPositiveCount_WhenRepeating_ThenAnEmptyStringIsReturnedWithoutThrowing(
            int count)
            => Assert.That(string.Empty.Repeat(count), Is.Empty);

        [Test]
        public void GivenAnEmptyStringAndMaximumCount_WhenRepeating_ThenAnEmptyStringIsReturnedWithoutThrowing()
            => Assert.That(string.Empty.Repeat(int.MaxValue), Is.Empty);

        [Test]
        public void GivenANullStringAndMaximumCount_WhenRepeating_ThenAnEmptyStringIsReturnedWithoutThrowing()
        {
            string text = null;

            Assert.That(text.Repeat(int.MaxValue), Is.Empty);
        }

        [Test]
        [TestCase("a", 100000)]
        [TestCase("ab", 50000)]
        [TestCase("Praise the Sun!", 100000)]
        public void GivenAStringAndALargeAcceptedCount_WhenRepeating_ThenTheResultLengthIsTheExactProduct(
            string text,
            int count)
            => Assert.That(
                text.Repeat(count).Length,
                Is.EqualTo((long)text.Length * count));

        [Test]
        [TestCase("a", 100000, "a")]
        [TestCase("ab", 50000, "ab")]
        [TestCase("xyz", 4096, "xyz")]
        public void GivenAStringAndALargeAcceptedCount_WhenRepeating_ThenTheResultIsTheSourceConcatenatedRepeatedly(
            string text,
            int count,
            string unit)
        {
            string result = text.Repeat(count);

            Assert.That(result, Is.EqualTo(string.Concat(System.Linq.Enumerable.Repeat(unit, count))));
            Assert.That(result.StartsWith(unit, StringComparison.Ordinal), Is.True);
            Assert.That(result.EndsWith(unit, StringComparison.Ordinal), Is.True);
        }

        [Test]
        public void GivenAStringAndALargeAcceptedCount_WhenRepeating_ThenTheResultIsAnExactMultipleOfTheSource()
        {
            string text = "Praise the Sun!";

            string result = text.Repeat(1000);

            Assert.That(result.Length % text.Length, Is.EqualTo(0));

            for (int i = 0; i < result.Length; i += text.Length)
            {
                Assert.That(result.Substring(i, text.Length), Is.EqualTo(text));
            }
        }

        [Test]
        public void GivenACountOfOne_WhenRepeating_ThenAStringWithTheSameContentIsReturned()
        {
            string text = "Praise the Sun!";

            string result = text.Repeat(1);

            Assert.That(result, Is.EqualTo(text));
        }

        [Test]
        [TestCase("🎮", 1000)]
        [TestCase("🎮🎯", 500)]
        public void GivenAStringContainingSurrogatePairsAndALargeAcceptedCount_WhenRepeating_ThenNoUnpairedSurrogateIsProduced(
            string text,
            int count)
        {
            string result = text.Repeat(count);

            for (int i = 0; i < result.Length; i++)
            {
                if (char.IsHighSurrogate(result[i]))
                {
                    Assert.That(i + 1, Is.LessThan(result.Length));
                    Assert.That(char.IsLowSurrogate(result[i + 1]), Is.True);
                    i++;
                }
                else
                {
                    Assert.That(char.IsLowSurrogate(result[i]), Is.False);
                }
            }
        }

        [Test]
        public void GivenANullSource_WhenReplacingTheFirstValue_ThenAnArgumentNullExceptionIsThrown()
        {
            string source = null;
            string oldValue = "Minecraft";
            string newValue = "Terraria";

            Assert.That(
                () => source.ReplaceFirst(oldValue, newValue),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GivenAnEmptySource_WhenReplacingTheFirstValue_ThenAnEmptyStringIsReturned()
        {
            string source = string.Empty;
            string oldValue = "Minecraft";
            string newValue = "Terraria";
            string expected = string.Empty;

            Assert.That(source.ReplaceFirst(oldValue, newValue), Is.EqualTo(expected));
        }

        [Test]
        public void GivenANullOldValue_WhenReplacingTheFirstValue_ThenAnArgumentNullExceptionIsThrown()
        {
            string source = "Praise the Sun! Praise the Sun!";
            string oldValue = null;
            string newValue = "Jolly cooperation!";

            Assert.That(
                () => source.ReplaceFirst(oldValue, newValue),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GivenAnEmptyOldValue_WhenReplacingTheFirstValue_ThenAnArgumentExceptionIsThrown()
        {
            string source = "Praise the Sun! Praise the Sun!";
            string oldValue = string.Empty;
            string newValue = "Jolly cooperation!";

            Assert.That(
                () => source.ReplaceFirst(oldValue, newValue),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void GivenANullNewValue_WhenReplacingTheFirstValue_ThenTheFirstValueIsEliminated()
        {
            string source = "Praise the Sun! Praise the Sun!";
            string oldValue = "Praise the Sun!";
            string newValue = null;
            string expected = " Praise the Sun!";

            Assert.That(source.ReplaceFirst(oldValue, newValue), Is.EqualTo(expected));
        }

        [Test]
        public void GivenAnEmptyNewValue_WhenReplacingTheFirstValue_ThenTheFirstValueIsEliminated()
        {
            string source = "Praise the Sun! Praise the Sun!";
            string oldValue = "Praise the Sun!";
            string newValue = string.Empty;
            string expected = " Praise the Sun!";

            Assert.That(source.ReplaceFirst(oldValue, newValue), Is.EqualTo(expected));
        }

        [Test]
        public void GivenMatchingValues_WhenReplacingTheFirstValue_ThenOnlyTheFirstValueIsReplaced()
        {
            string source = "Praise the Sun! Praise the Sun!";
            string oldValue = "Praise the Sun!";
            string newValue = "Jolly cooperation!";
            string expected = "Jolly cooperation! Praise the Sun!";

            Assert.That(source.ReplaceFirst(oldValue, newValue), Is.EqualTo(expected));
        }

        [Test]
        public void GivenANonMatchingOldValue_WhenReplacingTheFirstValue_ThenTheSourceIsReturned()
        {
            string source = "Praise the Sun!";
            string oldValue = "Minecraft";
            string newValue = "Terraria";
            string expected = source;

            Assert.That(source.ReplaceFirst(oldValue, newValue), Is.EqualTo(expected));
        }

        [Test]
        [TestCase("hello world hello", "hello", "hi", "hi world hello")]
        [TestCase("test", "test", "replaced", "replaced")]
        [TestCase("aaaa", "aa", "b", "baa")]
        [TestCase("unicode café", "café", "coffee", "unicode coffee")]
        [TestCase("case sensitive", "Case", "different", "case sensitive")]
        public void GivenMatchingValues_WhenReplacingTheFirstValue_ThenOnlyTheFirstValueIsReplaced_Parameterized(
            string source,
            string oldValue,
            string newValue,
            string expected)
            => Assert.That(source.ReplaceFirst(oldValue, newValue), Is.EqualTo(expected));

        [Test]
        [TestCase("Alžir is the serbo-croatian name for Algiers", "Alzhir is the serbo-croatian name for Algiers")]
        [TestCase("Horațiu says héllo", "Horatiu says hello")]
        [TestCase("Šimšat", "Shimshat")]
        [TestCase("Arunáčalpradéš", "Arunachalpradesh")]
        [TestCase("STŘEDNÍ AMERIKA", "STRZHEDNI AMERIKA")]
        [TestCase("", "")]
        [TestCase("Minecraft", "Minecraft")]
        [TestCase("café", "cafe")]
        [TestCase("naïve", "naive")]
        [TestCase("résumé", "resume")]
        [TestCase("piñata", "pinata")]
        [TestCase("über", "uber")]
        [TestCase("façade", "facade")]
        [TestCase("Ångström", "Angstroem")]
        [TestCase("Ελληνικά", "Ελληνικα")]
        [TestCase("日本語", "日本語")]
        public void GivenAString_WhenEliminatingDiacritics_ThenTheTransliteratedStringIsReturned(
            string input,
            string expected)
            => Assert.That(input.RemoveDiacritics(), Is.EqualTo(expected));

        [Test]
        [TestCase("Ö", "Oe")]
        [TestCase("ö", "oe")]
        [TestCase("OÖ", "OOE")]
        [TestCase("ÖO", "OEO")]
        [TestCase("MÖ", "MOE")]
        [TestCase("mö", "moe")]
        [TestCase("OÖ!", "OOE!")]
        [TestCase("aÖ!", "aOe!")]
        [TestCase("!Ö", "!Oe")]
        [TestCase("Ö!", "Oe!")]
        public void GivenAnOWithUmlaut_WhenEliminatingDiacritics_ThenTheCaseOfTheDigraphFollowsItsContext(
            string input,
            string expected)
            => Assert.That(input.RemoveDiacritics(), Is.EqualTo(expected));

        [Test]
        [TestCase("Č", "Ch")]
        [TestCase("č", "ch")]
        [TestCase("ČAS", "CHAS")]
        [TestCase("Čas", "Chas")]
        [TestCase("čas", "chas")]
        [TestCase("aČ", "aCh")]
        [TestCase("Ča", "Cha")]
        public void GivenACWithCaron_WhenEliminatingDiacritics_ThenTheCaseOfTheDigraphFollowsItsContext(
            string input,
            string expected)
            => Assert.That(input.RemoveDiacritics(), Is.EqualTo(expected));

        [Test]
        [TestCase("Š", "Sh")]
        [TestCase("š", "sh")]
        [TestCase("ŠKODA", "SHKODA")]
        [TestCase("Škoda", "Shkoda")]
        [TestCase("škoda", "shkoda")]
        [TestCase("aŠ", "aSh")]
        [TestCase("Ša", "Sha")]
        public void GivenAnSWithCaron_WhenEliminatingDiacritics_ThenTheCaseOfTheDigraphFollowsItsContext(
            string input,
            string expected)
            => Assert.That(input.RemoveDiacritics(), Is.EqualTo(expected));

        [Test]
        [TestCase("Ř", "Rzh")]
        [TestCase("ř", "rzh")]
        [TestCase("ŘEKA", "RZHEKA")]
        [TestCase("Řeka", "Rzheka")]
        [TestCase("řeka", "rzheka")]
        [TestCase("aŘ", "aRzh")]
        [TestCase("Řa", "Rzha")]
        public void GivenAnRWithCaron_WhenEliminatingDiacritics_ThenTheCaseOfTheDigraphFollowsItsContext(
            string input,
            string expected)
            => Assert.That(input.RemoveDiacritics(), Is.EqualTo(expected));

        [Test]
        [TestCase("Ž", "Zh")]
        [TestCase("ž", "zh")]
        [TestCase("ŽIŽEK", "ZHIZHEK")]
        [TestCase("Žižek", "Zhizhek")]
        [TestCase("žižek", "zhizhek")]
        [TestCase("aŽ", "aZh")]
        [TestCase("Ža", "Zha")]
        public void GivenAZWithCaron_WhenEliminatingDiacritics_ThenTheCaseOfTheDigraphFollowsItsContext(
            string input,
            string expected)
            => Assert.That(input.RemoveDiacritics(), Is.EqualTo(expected));

        [Test]
        [TestCase("ČŠŘŽÖ", "CHSHRZHZHOE")]
        [TestCase("čšřžö", "chshrzhzhoe")]
        [TestCase("ČŠŘŽÖ ČŠŘŽÖ", "CHSHRZHZHOE CHSHRZHZHOE")]
        public void GivenEveryMappedCharacter_WhenEliminatingDiacritics_ThenEachOneIsTransliteratedConsistently(
            string input,
            string expected)
            => Assert.That(input.RemoveDiacritics(), Is.EqualTo(expected));

        [Test]
        [TestCase("Ö", "Oe")]
        [TestCase("Č", "Ch")]
        [TestCase("Š", "Sh")]
        [TestCase("Ř", "Rzh")]
        [TestCase("Ž", "Zh")]
        public void GivenAnIsolatedUppercaseMappedCharacter_WhenEliminatingDiacritics_ThenTheTitleCaseDigraphIsReturned(
            string input,
            string expected)
            => Assert.That(input.RemoveDiacritics(), Is.EqualTo(expected));

        [Test]
        [TestCase("ö", "oe")]
        [TestCase("č", "ch")]
        [TestCase("š", "sh")]
        [TestCase("ř", "rzh")]
        [TestCase("ž", "zh")]
        public void GivenAnIsolatedLowercaseMappedCharacter_WhenEliminatingDiacritics_ThenTheLowercaseDigraphIsReturned(
            string input,
            string expected)
            => Assert.That(input.RemoveDiacritics(), Is.EqualTo(expected));

        [Test]
        [SetCulture("tr-TR")]
        [TestCase("ŽIŽEK", "ZHIZHEK")]
        [TestCase("Žižek", "Zhizhek")]
        [TestCase("ö", "oe")]
        [TestCase("Ö", "Oe")]
        [TestCase("IÖ", "IOE")]
        public void GivenATurkishCulture_WhenEliminatingDiacritics_ThenTheMappingIsCultureInvariant(
            string input,
            string expected)
            => Assert.That(input.RemoveDiacritics(), Is.EqualTo(expected));

        [Test]
        public void GivenANullString_WhenEliminatingDiacritics_ThenAnArgumentNullExceptionIsThrown()
        {
            string input = null;

            Assert.That(
                () => input.RemoveDiacritics(),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        [TestCase("", "")]
        [TestCase("Minecraft", "Minecraft")]
        [TestCase(
            "Chuck Norris doesn't do push-ups. He pushes the Earth down.",
            "Chuck Norris doesnt do pushups He pushes the Earth down")]
        [TestCase("Hello, World!", "Hello World")]
        [TestCase("test...", "test")]
        [TestCase("a.b.c", "abc")]
        [TestCase("!@#$%^&*()", "$^")]
        [TestCase("café!", "café")]
        [TestCase("日本語。", "日本語")]
        [TestCase("  spaces  ", "  spaces  ")]
        public void GivenAString_WhenEliminatingPunctuation_ThenOnlyNonPunctuationCharactersAreReturned(
            string input,
            string expected)
            => Assert.That(input.RemovePunctuation(), Is.EqualTo(expected));

        [Test]
        public void GivenANullString_WhenEliminatingPunctuation_ThenAnArgumentNullExceptionIsThrown()
        {
            string input = null;

            Assert.That(
                () => input.RemovePunctuation(),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        [TestCase("4", "4")]
        [TestCase("PraiseTheSun_today\tforever", "Praise The Sun today forever")]
        [TestCase("\tpraise the sun!\t", "praise the sun!")]
        [TestCase("hello world", "hello world")]
        [TestCase("HELLO WORLD", "H E L L O W O R L D")]
        [TestCase("already Sentence.", "already Sentence.")]
        [TestCase("multiple words here", "multiple words here")]
        [TestCase("café", "café")]
        [TestCase("123", "123")]
        public void GivenAString_WhenConvertingToASentence_ThenTheFormattedSentenceIsReturned(
            string input,
            string expected)
            => Assert.That(input.ToSentence(), Is.EqualTo(expected));

        [Test]
        [TestCase("_x", "x")]
        [TestCase("x_", "x")]
        [TestCase("a\t\tb", "a b")]
        [TestCase("a  b", "a b")]
        [TestCase("a__b", "a b")]
        [TestCase("a \t b", "a b")]
        [TestCase("_\t_", "")]
        [TestCase("\t\t", "")]
        [TestCase("   ", "")]
        [TestCase("  x  ", "x")]
        [TestCase("a_b\tc d", "a b c d")]
        [TestCase("_Praise\t_the_Sun_", "Praise the Sun")]
        [TestCase("A B", "A B")]
        [TestCase("AB", "A B")]
        [TestCase("aB", "a B")]
        [TestCase("a b C", "a b C")]
        [TestCase("x\t\t\tY", "x Y")]
        [TestCase("already  Sentence.", "already Sentence.")]
        public void GivenAStringWithRepeatedOrAdjacentSeparators_WhenConvertingToASentence_ThenConsecutiveSpacesAreCollapsed(
            string input,
            string expected)
            => Assert.That(input.ToSentence(), Is.EqualTo(expected));

        [Test]
        [TestCase("4")]
        [TestCase("PraiseTheSun_today\tforever")]
        [TestCase("\tpraise the sun!\t")]
        [TestCase("hello world")]
        [TestCase("HELLO WORLD")]
        [TestCase("already Sentence.")]
        [TestCase("multiple words here")]
        [TestCase("café")]
        [TestCase("123")]
        [TestCase("_x")]
        [TestCase("x_")]
        [TestCase("a\t\tb")]
        [TestCase("a  b")]
        [TestCase("a__b")]
        [TestCase("_Praise\t_the_Sun_")]
        public void GivenAString_WhenConvertingToASentence_ThenNoTwoConsecutiveSpacesAreEmitted(string input)
            => Assert.That(input.ToSentence(), Does.Not.Contain("  "));

        [Test]
        [TestCase("4")]
        [TestCase("PraiseTheSun_today\tforever")]
        [TestCase("\tpraise the sun!\t")]
        [TestCase("hello world")]
        [TestCase("HELLO WORLD")]
        [TestCase("already Sentence.")]
        [TestCase("_x")]
        [TestCase("x_")]
        [TestCase("a\t\tb")]
        [TestCase("a  b")]
        [TestCase("_Praise\t_the_Sun_")]
        public void GivenAString_WhenConvertingToASentence_ThenTheResultIsTrimmed(string input)
        {
            string result = input.ToSentence();

            Assert.That(result, Is.EqualTo(result.Trim()));
            Assert.That(result.StartsWith(' '), Is.False);
            Assert.That(result.EndsWith(' '), Is.False);
        }

        [Test]
        [TestCase("a Sentence", "a Sentence")]
        [TestCase("already Sentence.", "already Sentence.")]
        [TestCase("x Y z", "x Y z")]
        [TestCase("_Inner_Capitals_Here", "Inner Capitals Here")]
        public void GivenAStringWithInteriorCapitals_WhenConvertingToASentence_ThenTheCapitalsArePreserved(
            string input,
            string expected)
            => Assert.That(input.ToSentence(), Is.EqualTo(expected));

        [Test]
        public void GivenAnEmptyString_WhenConvertingToASentence_ThenAnArgumentOutOfRangeExceptionIsThrown()
            => Assert.That(
                () => string.Empty.ToSentence(),
                Throws.TypeOf<ArgumentOutOfRangeException>());

        [Test]
        public void GivenANullString_WhenConvertingToASentence_ThenAnArgumentNullExceptionIsThrown()
        {
            string input = null;

            Assert.That(
                () => input.ToSentence(),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        [TestCase("")]
        public void GivenAnEmptyString_WhenTruncating_ThenTheStringIsReturnedAsIs(string inputString)
            => Assert.That(inputString.Truncate(4), Is.EqualTo(inputString));

        [Test]
        public void GivenANullString_WhenTruncating_ThenAnArgumentNullExceptionIsThrown()
        {
            Assert.That(
                () => ((string)null).Truncate(4),
                Throws.TypeOf<ArgumentNullException>()
                    .With.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("value"));
        }

        [Test]
        [TestCase("Minecraft", 16)]
        [TestCase("Dark Souls III", 42)]
        public void GivenAStringAndExcessiveMaximumLength_WhenTruncating_ThenTheStringIsReturnedAsIs(
            string inputString,
            int maximumLength)
            => Assert.That(inputString.Truncate(maximumLength), Is.EqualTo(inputString));

        [Test]
        [TestCase("Minecraft", 4, "Mine")]
        [TestCase("Praise the Sun!", 8, "Praise t")]
        [TestCase("Hello", 3, "Hel")]
        [TestCase("a", 1, "a")]
        [TestCase("unicode café", 6, "unicod")]
        public void GivenAStringAndRestrictedMaximumLength_WhenTruncating_ThenTheTruncatedStringIsReturned(
            string inputString,
            int maximumLength,
            string expectedString)
            => Assert.That(inputString.Truncate(maximumLength), Is.EqualTo(expectedString));

        [Test]
        [TestCase("🎮", 1, "")]
        [TestCase("🎮", 2, "🎮")]
        [TestCase("🎮", 3, "🎮")]
        [TestCase("a🎮", 1, "a")]
        [TestCase("a🎮", 2, "a")]
        [TestCase("a🎮", 3, "a🎮")]
        [TestCase("🎮a", 1, "")]
        [TestCase("🎮a", 2, "🎮")]
        [TestCase("🎮a", 3, "🎮a")]
        [TestCase("𝔘𝔫𝔦", 1, "")]
        [TestCase("𝔘𝔫𝔦", 2, "𝔘")]
        [TestCase("𝔘𝔫𝔦", 3, "𝔘")]
        [TestCase("𝔘𝔫𝔦", 4, "𝔘𝔫")]
        [TestCase("𝔘𝔫𝔦", 5, "𝔘𝔫")]
        [TestCase("𝔘𝔫𝔦", 6, "𝔘𝔫𝔦")]
        public void GivenAStringWithSurrogatePairsAndARestrictedMaximumLength_WhenTruncating_ThenNoUnpairedSurrogateIsProduced(
            string inputString,
            int maximumLength,
            string expectedString)
        {
            string result = inputString.Truncate(maximumLength);

            Assert.That(result, Is.EqualTo(expectedString));
            Assert.That(
                result.Length == 0 || !char.IsHighSurrogate(result[^1]),
                Is.True,
                "The truncated result must not end with an unpaired high surrogate.");
        }

        [Test]
        [TestCase("🎮", 1)]
        [TestCase("🎮", 3)]
        [TestCase("a🎮", 1)]
        [TestCase("a🎮", 2)]
        [TestCase("🎮a", 1)]
        [TestCase("🎮a", 2)]
        [TestCase("𝔘𝔫𝔦", 1)]
        [TestCase("𝔘𝔫𝔦", 2)]
        [TestCase("𝔘𝔫𝔦", 3)]
        [TestCase("𝔘𝔫𝔦", 4)]
        [TestCase("𝔘𝔫𝔦", 5)]
        [TestCase("𝔘𝔫𝔦", 6)]
        public void GivenAStringWithSurrogatePairs_WhenTruncating_ThenTheResultNeverEndsWithAnUnpairedSurrogate(
            string inputString,
            int maximumLength)
        {
            string result = inputString.Truncate(maximumLength);

            Assert.That(
                result.Length == 0 || !char.IsHighSurrogate(result[^1]),
                Is.True,
                "The truncated result must not end with an unpaired high surrogate.");

            Assert.That(
                result.Length <= maximumLength,
                Is.True,
                "The truncated result must not exceed the maximum length.");
        }

        [Test]
        public void GivenAnEmojiAtTheLimit_WhenTruncating_ThenTheWholeEmojiIsReturned()
            => Assert.That("🎮".Truncate(2), Is.EqualTo("🎮"));

        [Test]
        public void GivenAnEmojiAfterTheLimit_WhenTruncating_ThenTheEmojiIsDroppedEntirely()
            => Assert.That("ab🎮".Truncate(2), Is.EqualTo("ab"));

        [Test]
        public void GivenAStringWithSurrogatePairs_WhenTruncatingToZero_ThenAnEmptyStringIsReturned()
            => Assert.That("🎮🎮".Truncate(0), Is.Empty);

        [Test]
        public void GivenAStringAndZeroMaximumLength_WhenTruncating_ThenAnEmptyStringIsReturned()
            => Assert.That("Minecraft".Truncate(0), Is.Empty);

        [Test]
        public void GivenAStringAndNegativeMaximumLength_WhenTruncating_ThenAnArgumentOutOfRangeExceptionIsThrown()
            => Assert.That(
                () => "Minecraft".Truncate(-4),
                Throws.TypeOf<ArgumentOutOfRangeException>());

        [Test]
        public void GivenAJsonString_WhenDeserialising_ThenTheExpectedObjectIsReturned()
        {
            string json =
                $"{{\"{nameof(DummyTestObject.StringProperty)}\":\"Minecraft\"," +
                $"\"{nameof(DummyTestObject.IntProperty)}\":4}}";

            DummyTestObject expectedObject = new()
            {
                StringProperty = "Minecraft",
                IntProperty = 4,
            };

            Assert.That(json.FromJson<DummyTestObject>(), Is.EqualTo(expectedObject));
        }

        [Test]
        public void GivenAJsonStringAndCustomOptions_WhenDeserialising_ThenTheExpectedObjectIsReturned()
        {
            string json = "{\"stringProperty\":\"Minecraft\",\"intProperty\":4}";
            DummyTestObject expectedObject = new()
            {
                StringProperty = "Minecraft",
                IntProperty = 4,
            };

            Assert.That(
                json.FromJson<DummyTestObject>(jsonOptions),
                Is.EqualTo(expectedObject));
        }

        [Test]
        [TestCase("{\"stringProperty\":\"test\",\"intProperty\":123}")]
        [TestCase("{\"stringProperty\":\"\",\"intProperty\":0}")]
        [TestCase("{\"stringProperty\":\"unicode café\",\"intProperty\":-1}")]
        [TestCase("{\"stringProperty\":\"🎮\",\"intProperty\":42}")]
        public void GivenAJsonString_WhenDeserialising_ThenTheExpectedObjectIsReturned_Parameterized(string json)
        {
            var result = json.FromJson<DummyTestObject>();
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GivenAnInvalidJsonString_WhenDeserialising_ThenAJsonExceptionIsThrown()
            => Assert.That(
                () => "Praise the Sun!".FromJson<DummyTestObject>(),
                Throws.TypeOf<JsonException>());

        [Test]
        public void GivenANullJsonString_WhenDeserialising_ThenAnArgumentNullExceptionIsThrown()
        {
            string json = null;

            Assert.That(
                () => json.FromJson<DummyTestObject>(),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GivenANullJsonString_WhenDeserialisingWithOptions_ThenAnArgumentNullExceptionIsThrown()
        {
            string? json = null;

            Assert.That(
                () => json!.FromJson<DummyTestObject>(jsonOptions),
                Throws.TypeOf<ArgumentNullException>()
                    .With.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("json"));
        }

        [Test]
        [TestCase("null")]
        public void GivenAJsonLiteralNull_WhenDeserialising_ThenNullIsReturned(string json)
            => Assert.That(json.FromJson<object>(), Is.Null);

        [Test]
        [TestCase("")]
        [TestCase("{")]
        [TestCase("Praise the Sun!")]
        public void GivenAMalformedJsonString_WhenDeserialising_ThenAJsonExceptionIsThrown(string json)
            => Assert.That(
                () => json.FromJson<DummyTestObject>(),
                Throws.TypeOf<JsonException>());

        [Test]
        public void GivenANullString_WhenReversing_ThenTheExceptionNamesTheTextParameter()
            => Assert.That(
                () => ((string?)null)!.Reverse(),
                Throws.TypeOf<ArgumentNullException>()
                    .With.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("text"));

        [Test]
        public void GivenANullString_WhenEliminatingDiacritics_ThenTheExceptionNamesTheSourceParameter()
            => Assert.That(
                () => ((string?)null)!.RemoveDiacritics(),
                Throws.TypeOf<ArgumentNullException>()
                    .With.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("source"));

        [Test]
        public void GivenANullString_WhenEliminatingPunctuation_ThenTheExceptionNamesTheSourceParameter()
            => Assert.That(
                () => ((string?)null)!.RemovePunctuation(),
                Throws.TypeOf<ArgumentNullException>()
                    .With.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("source"));

        [Test]
        public void GivenANullSource_WhenReplacingTheFirstValue_ThenTheExceptionNamesTheSourceParameter()
            => Assert.That(
                () => ((string?)null)!.ReplaceFirst("Praise the Sun!", "Jolly cooperation!"),
                Throws.TypeOf<ArgumentNullException>()
                    .With.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("source"));

        [Test]
        public void GivenANullOldValue_WhenReplacingTheFirstValue_ThenTheExceptionNamesTheOldValueParameter()
            => Assert.That(
                () => "Praise the Sun!".ReplaceFirst((string?)null!, "Jolly cooperation!"),
                Throws.TypeOf<ArgumentNullException>()
                    .With.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("oldValue"));

        [Test]
        public void GivenAnEmptyOldValue_WhenReplacingTheFirstValue_ThenTheExceptionNamesTheOldValueParameter()
            => Assert.That(
                () => "Praise the Sun!".ReplaceFirst(string.Empty, "Jolly cooperation!"),
                Throws.TypeOf<ArgumentException>()
                    .With.Property(nameof(ArgumentException.ParamName))
                    .EqualTo("oldValue"));

        [Test]
        public void GivenANullString_WhenConvertingToASentence_ThenTheExceptionNamesTheSourceParameter()
            => Assert.That(
                () => ((string?)null)!.ToSentence(),
                Throws.TypeOf<ArgumentNullException>()
                    .With.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("source"));

        [Test]
        public void GivenAnEmptyString_WhenConvertingToASentence_ThenTheExceptionNamesTheSourceParameter()
            => Assert.That(
                () => string.Empty.ToSentence(),
                Throws.TypeOf<ArgumentOutOfRangeException>()
                    .With.Property(nameof(ArgumentOutOfRangeException.ParamName))
                    .EqualTo("source"));

        [Test]
        public void GivenANegativeMaximumLength_WhenTruncating_ThenTheExceptionNamesTheMaximumLengthParameter()
            => Assert.That(
                () => "Minecraft".Truncate(-4),
                Throws.TypeOf<ArgumentOutOfRangeException>()
                    .With.Property(nameof(ArgumentOutOfRangeException.ParamName))
                    .EqualTo("maxLength"));

        [Test]
        public void GivenANullJsonString_WhenDeserialising_ThenTheExceptionNamesTheJsonParameter()
            => Assert.That(
                () => ((string?)null)!.FromJson<DummyTestObject>(),
                Throws.TypeOf<ArgumentNullException>()
                    .With.Property(nameof(ArgumentNullException.ParamName))
                    .EqualTo("json"));

        [Test]
        [TestCase("Praise the Sun!")]
        [TestCase("")]
        [TestCase("café")]
        [TestCase("🎮")]
        [TestCase("日本語")]
        public void GivenAString_WhenReversing_ThenTheResultIsNeverNull(string text)
            => Assert.That(text.Reverse(), Is.Not.Null);

        [Test]
        [TestCase("Praise the Sun!")]
        [TestCase("")]
        [TestCase("café")]
        [TestCase("🎮")]
        [TestCase("日本語")]
        public void GivenAString_WhenEliminatingDiacritics_ThenTheResultIsNeverNull(string text)
            => Assert.That(text.RemoveDiacritics(), Is.Not.Null);

        [Test]
        [TestCase("Praise the Sun!")]
        [TestCase("")]
        [TestCase("café")]
        [TestCase("🎮")]
        [TestCase("日本語")]
        public void GivenAString_WhenEliminatingPunctuation_ThenTheResultIsNeverNull(string text)
            => Assert.That(text.RemovePunctuation(), Is.Not.Null);

        [Test]
        [TestCase("Praise the Sun!")]
        [TestCase("café")]
        [TestCase("🎮")]
        [TestCase("日本語")]
        public void GivenAString_WhenConvertingToASentence_ThenTheResultIsNeverNull(string text)
            => Assert.That(text.ToSentence(), Is.Not.Null);

        [Test]
        [TestCase("Praise the Sun!")]
        [TestCase("")]
        [TestCase("café")]
        [TestCase("🎮")]
        [TestCase("日本語")]
        public void GivenAString_WhenTruncating_ThenTheResultIsNeverNull(string text)
            => Assert.That(text.Truncate(4), Is.Not.Null);

        [Test]
        [TestCase("Praise the Sun!", "Sun", "Moon")]
        [TestCase("", "Sun", "Moon")]
        [TestCase("Minecraft", "craft", "")]
        public void GivenAString_WhenReplacingTheFirstValue_ThenTheResultIsNeverNull(
            string source,
            string oldValue,
            string newValue)
            => Assert.That(source.ReplaceFirst(oldValue, newValue), Is.Not.Null);

        [Test]
        [TestCase("a🎮b", "A🎮B")]
        [TestCase("🎮", "🎮")]
        [TestCase("🎮🎯", "🎮🎯")]
        [TestCase("日本語", "日本語")]
        [TestCase("𐍈𐍉𐍊", "𐍈𐍉𐍊")]
        [TestCase("a🎮b🎯c", "A🎮B🎯C")]
        public void GivenAStringContainingSurrogatePairs_WhenInvertingTheCase_ThenTheSurrogatePairsStayIntact(
            string text,
            string expected)
        {
            string? result = text.InvertCase();

            Assert.That(result, Is.EqualTo(expected));
            Assert.That(result!.Length, Is.EqualTo(text.Length));
        }

        [Test]
        [TestCase("\uD800ab", "\uD800AB")]
        [TestCase("ab\uDC00", "AB\uDC00")]
        [TestCase("a\uD800b", "A\uD800B")]
        public void GivenAStringWithUnpairedSurrogates_WhenInvertingTheCase_ThenTheSurrogatesArePreserved(
            string text,
            string expected)
            => Assert.That(text.InvertCase(), Is.EqualTo(expected));

        [Test]
        [TestCase("🎮!", "🎮")]
        [TestCase("a🎮b.", "a🎮b")]
        [TestCase("𐍈…", "𐍈")]
        [TestCase("🎮", "🎮")]
        public void GivenAStringContainingSurrogatePairs_WhenEliminatingPunctuation_ThenTheSurrogatePairsStayIntact(
            string text,
            string expected)
            => Assert.That(text.RemovePunctuation(), Is.EqualTo(expected));

        [Test]
        [TestCase("a\uD800b.", "a\uD800b")]
        [TestCase("\uD800!", "\uD800")]
        public void GivenAStringWithUnpairedSurrogates_WhenEliminatingPunctuation_ThenTheSurrogatesArePreserved(
            string text,
            string expected)
            => Assert.That(text.RemovePunctuation(), Is.EqualTo(expected));

        [Test]
        [TestCase("🎮Žižek", "🎮Zhizhek")]
        [TestCase("Č🎮Š", "Ch🎮Sh")]
        [TestCase("A🎯Ö", "A🎯Oe")]
        [TestCase("Ö🎯", "Oe🎯")]
        [TestCase("🎯Ö", "🎯Oe")]
        public void GivenAMappedCharacterAdjacentToASurrogatePair_WhenEliminatingDiacritics_ThenTheDigraphCasingIsUnaffected(
            string text,
            string expected)
            => Assert.That(text.RemoveDiacritics(), Is.EqualTo(expected));

        [Test]
        [TestCase("🎮\tx", "🎮 x")]
        [TestCase("helloWorld", "hello World")]
        [TestCase("_Praise\t_the_Sun_", "Praise the Sun")]
        [TestCase("a\uD800b", "a\uD800b")]
        public void GivenAStringContainingSurrogatePairs_WhenConvertingToASentence_ThenTheSurrogatePairsStayIntact(
            string text,
            string expected)
            => Assert.That(text.ToSentence(), Is.EqualTo(expected));

        [Test]
        public void GivenAStringWithAHighSurrogateAtTheBoundary_WhenTruncating_ThenTheBoundaryCharacterIsDropped()
        {
            string text = "\uD800ab";

            string result = text.Truncate(1);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GivenAStringWithALowSurrogateAtTheBoundary_WhenTruncating_ThenTheLowSurrogateIsPreserved()
        {
            string text = "\uDC00ab";

            string result = text.Truncate(1);

            Assert.That(result, Has.Length.EqualTo(1));
            Assert.That(char.IsLowSurrogate(result[0]), Is.True);
        }

        [Test]
        [TestCase("🎮", 2, "🎮🎮")]
        [TestCase("a🎮", 3, "a🎮a🎮a🎮")]
        [TestCase("𐍈𐍉", 2, "𐍈𐍉𐍈𐍉")]
        public void GivenAStringContainingSurrogatePairs_WhenRepeating_ThenTheSurrogatePairsStayIntact(
            string text,
            int count,
            string expected)
            => Assert.That(text.Repeat(count), Is.EqualTo(expected));

        [Test]
        public void GivenAStringWithAnUnpairedSurrogate_WhenRepeating_ThenTheSurrogateIsPreserved()
        {
            string text = "\uD800";

            string result = text.Repeat(2);

            Assert.That(result, Has.Length.EqualTo(2));
            Assert.That(result, Is.EqualTo("\uD800\uD800"));
        }

        [Test]
        public void GivenAStringContainingSurrogatePairs_WhenReplacingTheFirstValue_ThenTheSurrogatePairsStayIntact()
        {
            string source = "🎮ab";

            Assert.That(source.ReplaceFirst("🎮", "G"), Is.EqualTo("Gab"));
        }

        [Test]
        public void GivenAnEmptyStringAndANegativeMaximumLength_WhenTruncating_ThenTheEmptyStringIsReturned()
            => Assert.That(string.Empty.Truncate(-4), Is.Empty);
    }
}
