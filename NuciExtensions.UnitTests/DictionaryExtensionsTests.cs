using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace NuciExtensions.UnitTests
{
    [TestFixture]
    public sealed class DictionaryExtensionsTests
    {
        [Test]
        public void GivenAnExistingKey_WhenAddingOrUpdating_ThenTheValueIsUpdated()
        {
            string testKey = "Minecraft";
            string testValue = "Dark Souls III";

            Dictionary<string, string> dictionary = new()
            {
                { testKey, "Terraria" },
            };

            dictionary.AddOrUpdate(testKey, testValue);

            AssertThatDictionaryPairExists(dictionary, testKey, testValue);
        }

        [Test]
        [TestCase("key1", "value1", "updated1")]
        [TestCase("key2", "value2", "updated2")]
        [TestCase("unicode café", "original", "updated")]
        [TestCase("123", "456", "789")]
        [TestCase("", "empty", "notempty")]
        public void GivenAnExistingKey_WhenAddingOrUpdating_ThenTheValueIsUpdated_Parameterized(
            string key, string initialValue, string updatedValue)
        {
            Dictionary<string, string> dictionary = new() { { key, initialValue } };
            dictionary.AddOrUpdate(key, updatedValue);
            AssertThatDictionaryPairExists(dictionary, key, updatedValue);
        }

        [Test]
        public void GivenAMissingKey_WhenAddingOrUpdating_ThenThePairIsAdded()
        {
            string testKey = "Minecraft";
            string testValue = "Dark Souls III";

            Dictionary<string, string> dictionary = [];

            dictionary.AddOrUpdate(testKey, testValue);

            AssertThatDictionaryPairExists(dictionary, testKey, testValue);
        }

        [Test]
        [TestCase("newkey", "newvalue")]
        [TestCase("unicode café", "value")]
        [TestCase("123", "456")]
        [TestCase("", "empty")]
        [TestCase("key with spaces", "value")]
        public void GivenAMissingKey_WhenAddingOrUpdating_ThenThePairIsAdded_Parameterized(
            string key, string value)
        {
            Dictionary<string, string> dictionary = [];
            dictionary.AddOrUpdate(key, value);
            AssertThatDictionaryPairExists(dictionary, key, value);
        }

        [Test]
        public void GivenANullDictionary_WhenAddingOrUpdating_ThenAnArgumentNullExceptionIsThrown()
        {
            IDictionary<string, string> dictionary = null;

            Assert.That(
                () => dictionary.AddOrUpdate("Minecraft", "Dark Souls III"),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GivenANullKey_WhenAddingOrUpdating_ThenAnArgumentNullExceptionIsThrown()
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            string key = null;

            Assert.That(
                () => dictionary.AddOrUpdate(key, "Dark Souls III"),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GivenANullDictionary_WhenGettingAValue_ThenAnArgumentNullExceptionIsThrown()
        {
            IDictionary<string, string> dictionary = null;

            Assert.That(
                () => dictionary.TryGetValue("Minecraft"),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GivenANullKey_WhenGettingAValue_ThenAnArgumentNullExceptionIsThrown()
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            string key = null;

            Assert.That(
                () => dictionary.TryGetValue(key),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void GivenAMissingKey_WhenGettingAValue_ThenNullIsReturned()
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();

            Assert.That(dictionary.TryGetValue("Minecraft"), Is.Null);
        }

        [Test]
        [TestCase("missing1")]
        [TestCase("missing2")]
        [TestCase("unicode café")]
        [TestCase("123")]
        [TestCase("")]
        public void GivenAMissingKey_WhenGettingAValue_ThenNullIsReturned_Parameterized(string key)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            Assert.That(dictionary.TryGetValue(key), Is.Null);
        }

        [Test]
        public void GivenAnExistingKey_WhenGettingAValue_ThenTheAssociatedValueIsReturned()
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>
            {
                { "Minecraft", "Dark Souls III" },
            };

            Assert.That(
                dictionary.TryGetValue("Minecraft"),
                Is.EqualTo("Dark Souls III"));
        }

        [Test]
        [TestCase("key1", "value1")]
        [TestCase("key2", "value2")]
        [TestCase("unicode café", "value")]
        [TestCase("123", "456")]
        [TestCase("", "empty")]
        public void GivenAnExistingKey_WhenGettingAValue_ThenTheAssociatedValueIsReturned_Parameterized(
            string key, string value)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string> { { key, value } };
            Assert.That(dictionary.TryGetValue(key), Is.EqualTo(value));
        }

        private static void AssertThatDictionaryPairExists<TKey, TValue>(
            Dictionary<TKey, TValue> dictionary,
            TKey key,
            TValue value)
            where TKey : notnull
        {
            Assert.That(dictionary.ContainsKey(key));
            Assert.That(dictionary[key], Is.EqualTo(value));
        }
    }
}
