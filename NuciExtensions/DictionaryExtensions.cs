using System.Collections.Generic;

namespace NuciExtensions
{
    /// <summary>
    /// Dictionary extensions.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Adds or updates the specified key-value pair in the source dictionary.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="key">Key.</param>
        /// <param name="value">Element.</param>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TElement">The element type.</typeparam>
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

        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
        {
            source.TryGetValue(key, out TValue value);

            return value;
        }
    }
}
