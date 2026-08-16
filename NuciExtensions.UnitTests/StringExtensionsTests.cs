using System;
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
        public void GivenAString_WhenInvertingTheCase_ThenTheOppositeCaseIsReturned(
            string text,
            string expectedText)
            => Assert.That(text.InvertCase(), Is.EqualTo(expectedText));

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void GivenANullOrEmptyString_WhenInvertingTheCase_ThenTheOriginalStringIsReturned(string? text)
            => Assert.That(text!.InvertCase(), Is.EqualTo(text));

        [Test]
        [TestCase("", "")]
        [TestCase("4", "4")]
        [TestCase("Praise the Sun!", "!nuS eht esiarP")]
        public void GivenAString_WhenReversing_ThenTheCharactersAreReturnedInReverseOrder(
            string text,
            string expectedText)
            => Assert.That(text.Reverse(), Is.EqualTo(expectedText));

        [Test]
        public void GivenANullString_WhenReversing_ThenANullReferenceExceptionIsThrown()
        {
            string text = null!;

            Assert.That(
                () => text.Reverse(),
                Throws.TypeOf<NullReferenceException>());
        }

        [Test]
        public void GivenAStringAndPositiveCount_WhenRepeating_ThenTheRepeatedStringIsReturned()
            => Assert.That(
                "Praise the Sun!".Repeat(4),
                Is.EqualTo("Praise the Sun!Praise the Sun!Praise the Sun!Praise the Sun!"));

        [Test]
        [TestCase(0)]
        [TestCase(-4)]
        public void GivenAStringAndNonPositiveCount_WhenRepeating_ThenAnEmptyStringIsReturned(int count)
            => Assert.That("Praise the Sun!".Repeat(count), Is.Empty);

        [Test]
        public void GivenANullStringAndPositiveCount_WhenRepeating_ThenAnEmptyStringIsReturned()
        {
            string text = null!;

            Assert.That(text.Repeat(4), Is.Empty);
        }

        [Test]
        public void GivenANullSource_WhenReplacingTheFirstValue_ThenANullReferenceExceptionIsThrown()
        {
            string source = null!;
            string oldValue = "Minecraft";
            string newValue = "Terraria";

            Assert.That(
                () => source.ReplaceFirst(oldValue, newValue),
                Throws.TypeOf<NullReferenceException>());
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
            string oldValue = null!;
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
            string newValue = null!;
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
        [TestCase("Alžir is the serbo-croatian name for Algiers", "Alzhir is the serbo-croatian name for Algiers")]
        [TestCase("Horațiu says héllo", "Horatiu says hello")]
        [TestCase("Šimšat", "Shimshat")]
        [TestCase("Arunáčalpradéš", "Arunachalpradesh")]
        [TestCase("STŘEDNÍ AMERIKA", "STRZHEDNI AMERIKA")]
        [TestCase("", "")]
        [TestCase("Minecraft", "Minecraft")]
        public void GivenAString_WhenEliminatingDiacritics_ThenTheTransliteratedStringIsReturned(
            string input,
            string expected)
            => Assert.That(input.RemoveDiacritics(), Is.EqualTo(expected));

        [Test]
        [TestCase("", "")]
        [TestCase("Minecraft", "Minecraft")]
        [TestCase(
            "Chuck Norris doesn't do push-ups. He pushes the Earth down.",
            "Chuck Norris doesnt do pushups He pushes the Earth down")]
        public void GivenAString_WhenEliminatingPunctuation_ThenOnlyNonPunctuationCharactersAreReturned(
            string input,
            string expected)
            => Assert.That(input.RemovePunctuation(), Is.EqualTo(expected));

        [Test]
        public void GivenANullString_WhenEliminatingPunctuation_ThenANullReferenceExceptionIsThrown()
        {
            string input = null!;

            Assert.That(
                () => input.RemovePunctuation(),
                Throws.TypeOf<NullReferenceException>());
        }

        [Test]
        [TestCase("4", "4")]
        [TestCase("PraiseTheSun_today\tforever", "Praise The Sun today forever")]
        [TestCase("\tpraise the sun!\t", "praise the sun!")]
        public void GivenAString_WhenConvertingToASentence_ThenTheFormattedSentenceIsReturned(
            string input,
            string expected)
            => Assert.That(input.ToSentence(), Is.EqualTo(expected));

        [Test]
        public void GivenAnEmptyString_WhenConvertingToASentence_ThenAnArgumentOutOfRangeExceptionIsThrown()
            => Assert.That(
                () => string.Empty.ToSentence(),
                Throws.TypeOf<ArgumentOutOfRangeException>());

        [Test]
        public void GivenANullString_WhenConvertingToASentence_ThenANullReferenceExceptionIsThrown()
        {
            string input = null!;

            Assert.That(
                () => input.ToSentence(),
                Throws.TypeOf<NullReferenceException>());
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void GivenANullOrEmptyString_WhenTruncating_ThenTheStringIsReturnedAsIs(string? inputString)
            => Assert.That(inputString!.Truncate(4), Is.EqualTo(inputString));

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
        public void GivenAStringAndRestrictedMaximumLength_WhenTruncating_ThenTheTruncatedStringIsReturned(
            string inputString,
            int maximumLength,
            string expectedString)
            => Assert.That(inputString.Truncate(maximumLength), Is.EqualTo(expectedString));

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
        public void GivenAnInvalidJsonString_WhenDeserialising_ThenAJsonExceptionIsThrown()
            => Assert.That(
                () => "Praise the Sun!".FromJson<DummyTestObject>(),
                Throws.TypeOf<JsonException>());

        [Test]
        public void GivenANullJsonString_WhenDeserialising_ThenAnArgumentNullExceptionIsThrown()
        {
            string json = null!;

            Assert.That(
                () => json.FromJson<DummyTestObject>(),
                Throws.TypeOf<ArgumentNullException>());
        }
    }
}
