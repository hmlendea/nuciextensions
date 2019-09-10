using System;
using System.Collections.Generic;

using NUnit.Framework;

using NuciExtensions;

namespace NuciExtensions.UnitTests
{
    public class DictionaryExtensionsTests
    {
        [Test]
        public void AddOrUpdate_KeyExists_ValueUpdated()
        {
            string testKey = "testKeyTest";
            string testValue = "new value to update";

            Dictionary<string, string> dict = new Dictionary<string, string>
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

            Dictionary<string, string> dict = new Dictionary<string, string>();

            dict.AddOrUpdate(testKey, testValue);
            
            AssertThatDictionaryPairExists(dict, testKey, testValue);
        }
        
        [Test]
        public void TryGetValue_KeyIsNull_ThrowsArgumentNullException()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            Assert.Throws<ArgumentNullException>(() => dict.TryGetValue(null));
        }
        
        [Test]
        public void TryGetValue_KeyDoesNotExist_NullReturned()
        {
            string testKey = "testKeyTest";

            Dictionary<string, string> dict = new Dictionary<string, string>();

            string actual = dict.TryGetValue(testKey);
            
            Assert.AreEqual(null, actual);
        }

        void AssertThatDictionaryPairExists<TKey, TValue>(Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            Assert.IsTrue(dict.ContainsKey(key));
            Assert.AreEqual(value, dict[key]);
        }
    }
}
