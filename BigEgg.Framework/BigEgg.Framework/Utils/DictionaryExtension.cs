using System.Collections.Generic;

namespace BigEgg.Framework.Utils
{
    public static class DictionaryExtension
    {
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> list, TKey key, TValue value)
        {
            if (!list.ContainsKey(key))
            {
                list.Add(key, value);
            }
            else
            {
                list[key] = value;
            }
        }

        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, IList<TValue>> list, TKey key, TValue value)
        {
            if (!list.ContainsKey(key))
            {
                list.Add(key, new List<TValue>() { value });
            }
            else
            {
                list[key].Add(value);
            }
        }
    }
}
