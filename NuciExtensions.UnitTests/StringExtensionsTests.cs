using System;

using NUnit.Framework;

namespace NuciExtensions.UnitTests
{
    public class StringExtensionsTests
    {
        [Test]
        public void ReplaceFirst_SourceIsNull_ThrowsNullReferenceException()
        {
            string source = null;
            string oldValue = "test";
            string newValue = "this";

            Assert.Throws<NullReferenceException>(() => source.ReplaceFirst(oldValue, newValue));
        }

        [Test]
        public void ReplaceFirst_SourceIsEmpty_ReturnsEmpty()
        {
            string source = string.Empty;
            string oldValue = "test";
            string newValue = "this";
            string expected = string.Empty;

            Assert.That(source.ReplaceFirst(oldValue, newValue), Is.EqualTo(expected));
        }

        [Test]
        public void ReplaceFirst_OldValueIsNull_ThrowsArgumentNullException()
        {
            string source = "test string test text";
            string oldValue = null;
            string newValue = "this";

            Assert.Throws<ArgumentNullException>(() => source.ReplaceFirst(oldValue, newValue));
        }

        [Test]
        public void ReplaceFirst_OldValueIsEmpty_ThrowsArgumentException()
        {
            string source = "test string test text";
            string oldValue = string.Empty;
            string newValue = "this";

            Assert.Throws<ArgumentException>(() => source.ReplaceFirst(oldValue, newValue));
        }

        [Test]
        public void ReplaceFirst_NewValueIsNull_ReturnsCorrectString()
        {
            string source = "test string test text";
            string oldValue = "test";
            string newValue = null;
            string expected = " string test text";

            Assert.That(source.ReplaceFirst(oldValue, newValue), Is.EqualTo(expected));
        }

        [Test]
        public void ReplaceFirst_NewValueIsEmpty_ReturnsCorrectString()
        {
            string source = "test string test text";
            string oldValue = "test";
            string newValue = string.Empty;
            string expected = " string test text";

            Assert.That(source.ReplaceFirst(oldValue, newValue), Is.EqualTo(expected));
        }

        [Test]
        public void ReplaceFirst_ReturnsCorrectString()
        {
            string source = "test string test text";
            string oldValue = "test";
            string newValue = "this";
            string expected = "this string test text";

            Assert.That(source.ReplaceFirst(oldValue, newValue), Is.EqualTo(expected));
        }

        [Test]
        public void ReplaceFirst_OldValueNotFoundInSource_ReturnsSource()
        {
            string source = "test string test text";
            string oldValue = "somethingelse";
            string newValue = "this";
            string expected = source;

            Assert.That(source.ReplaceFirst(oldValue, newValue), Is.EqualTo(expected));
        }

        [Test]
        public void RemoveDiacritics_ReturnsCorrectValue()
        {
            string input = "Horațiu says héllo";
            string expected = "Horatiu says hello";
            string actual = input.RemoveDiacritics();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void RemovePunctuation_ReturnsCorrectValue()
        {
            string input = "Hello world! Am I greeting the world? Yes, I'm indeed greeting the world.";
            string expected = "Hello world Am I greeting the world Yes Im indeed greeting the world";
            string actual = input.RemovePunctuation();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Reverse_ReturnsCorrectValue()
            => Assert.That("abcdefg".Reverse(), Is.EqualTo("gfedcba"));
    }
}
