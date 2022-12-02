using System;
using System.Collections.Generic;

namespace SK.Framework
{
    public static class DictionaryExtension
    {
        public static Dictionary<K, V> ForEach<K, V>(this Dictionary<K, V> self, Action<K, V> action)
        {
            using(var dicE = self.GetEnumerator())
            {
                while (dicE.MoveNext())
                {
                    action(dicE.Current.Key, dicE.Current.Value);
                }
            }
            return self;
        }
        public static Dictionary<K, V> AddRange<K, V>(this Dictionary<K, V> self, Dictionary<K, V> target, bool isOverride = false)
        {
            using(var dicE = target.GetEnumerator())
            {
                while (dicE.MoveNext())
                {
                    var current = dicE.Current;
                    if (self.ContainsKey(current.Key))
                    {
                        if (isOverride)
                        {
                            self[current.Key] = current.Value;
                            continue;
                        }
                    }
                    self.Add(current.Key, current.Value);
                }
            }
            return self;
        }
        public static Dictionary<K, V> TryAdd<K, V>(this Dictionary<K, V> self, K k, V v)
        {
            if (!self.ContainsKey(k))
            {
                self.Add(k, v);
            }
            return self;
        }
    }
}