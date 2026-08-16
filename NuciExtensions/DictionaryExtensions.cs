using System.Collections.Generic;

namespace NuciExtensions
{
    /// <summary>
    /// Provides extension methods for dictionary operations.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Adds or updates the specified key-value pair in the source dictionary.
        /// </summary>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TElement">The element type.</typeparam>
        /// <param name="source">Source dictionary.</param>
        /// <param name="key">Key to add or update.</param>
        /// <param name="value">Value to set.</param>
        public static void AddOrUpdate<TKey, TElement>(this IDictionary<TKey, TElement> source, TKey key, TElement value)
        {
            if (source.ContainsKey(key))
            {
                source[key] = value;
            }
            else
            {
                source.Add(key, value);
            }
        }

        /// <summary>
        /// Tries to get the value associated with the specified key in the source dictionary.
        /// If the key does not exist, it returns the default value for the type.
        /// </summary>
        /// <param name="source">Source dictionary.</param>
        /// <param name="key">Key to look for.</param>
        /// <returns>The value associated with the key if it exists; otherwise, the default value for the type.</returns>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TValue">The value type.</typeparam>
        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
        {
            source.TryGetValue(key, out TValue value);

            return value;
        }
    }
}
