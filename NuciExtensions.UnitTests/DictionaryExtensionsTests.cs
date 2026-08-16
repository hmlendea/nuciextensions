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
        public void GivenAMissingKey_WhenAddingOrUpdating_ThenThePairIsAdded()
        {
            string testKey = "Minecraft";
            string testValue = "Dark Souls III";

            Dictionary<string, string> dictionary = [];

            dictionary.AddOrUpdate(testKey, testValue);

            AssertThatDictionaryPairExists(dictionary, testKey, testValue);
        }

        [Test]
        public void GivenANullKey_WhenGettingAValue_ThenAnArgumentNullExceptionIsThrown()
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            string key = null!;

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
