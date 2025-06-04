using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace NuciExtensions.UnitTests
{
    public class DictionaryExtensionsTests
    {
        [Test]
        public void AddOrUpdate_KeyExists_ValueUpdated()
        {
            string testKey = "testKeyTest";
            string testValue = "new value to update";

            Dictionary<string, string> dict = new()
            {
                { testKey, "old value" }
            };

            dict.AddOrUpdate(testKey, testValue);

            AssertThatDictionaryPairExists(dict, testKey, testValue);
        }

        [Test]
        public void AddOrUpdate_KeyDoesntExist_PairAdded()
        {
            string testKey = "testKeyTest";
            string testValue = "new value to add";

            Dictionary<string, string> dict = [];

            dict.AddOrUpdate(testKey, testValue);

            AssertThatDictionaryPairExists(dict, testKey, testValue);
        }

        [Test]
        public void TryGetValue_KeyIsNull_ThrowsArgumentNullException()
            => Assert.Throws<ArgumentNullException>(() => new Dictionary<string, string>().TryGetValue(null));

        [Test]
        public void TryGetValue_KeyDoesNotExist_NullReturned()
            => Assert.That(new Dictionary<string, string>().TryGetValue("testKeyTest"), Is.Null);

        static void AssertThatDictionaryPairExists<TKey, TValue>(Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            Assert.That(dict.ContainsKey(key));
            Assert.That(dict[key], Is.EqualTo(value));
        }
    }
}
