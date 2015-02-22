using System.Collections.Generic;

namespace BigEgg.Framework.Utils
{
    public static class DictionaryExtension
    {
        /// <summary>
        /// Add or update the value in a dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The object to use as the key of the element to add or update.</param>
        /// <param name="value">The object to use as the value of the element to add or update.</param>
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
            else
            {
                dictionary[key] = value;
            }
        }

        /// <summary>
        /// Add or update the value in a dictionary which value is a list.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The object to use as the key of the element to add or update.</param>
        /// <param name="value">The object to use as the value of the element to add or update.</param>
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, IList<TValue>> dictionary, TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, new List<TValue>() { value });
            }
            else
            {
                dictionary[key].Add(value);
            }
        }
    }
}
