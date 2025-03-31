using System;
using System.Collections.Generic;

namespace Extensions
{
    public static class DictionaryExtensions
    {
        public static KeyValuePair<K, V> GetMaxByValue<K, V>(this Dictionary<K, V> dictionary) where V : IComparable
        {
            if (dictionary == null) 
                throw new ArgumentNullException(nameof(dictionary));

            bool isFirstPair = true;
            KeyValuePair<K, V> max = default;

            var enumerator = dictionary.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var pair = enumerator.Current;
                if (isFirstPair)
                {
                    max = pair;
                    isFirstPair = false;
                    continue;
                }
                if (pair.Value.CompareTo(max.Value) > 0)
                {
                    max = pair;
                }
            }
            enumerator.Dispose();
            return max;
        }

        public static KeyValuePair<K, V> GetMinByValue<K, V>(this Dictionary<K, V> dictionary) where V : IComparable
        {
            if (dictionary == null) 
                throw new ArgumentNullException(nameof(dictionary));

            bool isFirstPair = true;
            KeyValuePair<K, V> min = default;
            var enumerator = dictionary.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var pair = enumerator.Current;
                if (isFirstPair)
                {
                    min = pair;
                    isFirstPair = false;
                    continue;
                }
                if (pair.Value.CompareTo(min.Value) < 0)
                {
                    min = pair;
                }
            }
            enumerator.Dispose();
            return min;
        }
    }
}