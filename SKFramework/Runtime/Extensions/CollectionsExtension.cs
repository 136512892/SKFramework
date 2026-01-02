/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Collections.Generic;

namespace SK.Framework
{
    public static class CollectionsExtension
    {
        /*********************************IEnumerable*********************************/
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> self, Action<T> action)
        {
            foreach (var item in self)
                action(item);
            return self;
        }

        /*********************************Array*********************************/
        public static T[] ForEach<T>(this T[] self, Action<T> action)
        {
            for (int i = 0; i < self.Length; i++)
                action(self[i]);
            return self;
        }

        public static T[] ForEach<T>(this T[] self, Action<int, T> action)
        {
            for (int i = 0; i < self.Length; i++)
                action(i, self[i]);
            return self;
        }

        public static T[] ForEachReverse<T>(this T[] self, Action<T> action)
        {
            for (int i = self.Length - 1; i >= 0; i--)
                action(self[i]);
            return self;
        }

        public static T[] ForEachReverse<T>(this T[] self, Action<int, T> action)
        {
            for (int i = self.Length - 1; i >= 0; i--)
                action(i, self[i]);
            return self;
        }

        /*********************************List*********************************/
        public static List<T> ForEach<T>(this List<T> self, Action<T> action)
        {
            for (int i = 0; i < self.Count; i++)
                action(self[i]);
            return self;
        }

        public static List<T> ForEach<T>(this List<T> self, Action<int, T> action)
        {
            for (int i = 0; i < self.Count; i++)
                action(i, self[i]);
            return self;
        }

        public static List<T> ForEachReverse<T>(this List<T> self, Action<T> action)
        {
            for (int i = self.Count - 1; i >= 0; i--)
                action(self[i]);
            return self;
        }

        public static List<T> ForEachReverse<T>(this List<T> self, Action<int, T> action)
        {
            for (int i = self.Count - 1; i >= 0; i--)
                action(i, self[i]);
            return self;
        }

        public static bool TryAdd<T>(this List<T> self, T t)
        {
            if (!self.Contains(t))
            {
                self.Add(t);
                return true;
            }
            return false;
        }

        /*********************************Dictionary*********************************/
        public static Dictionary<K, V> ForEach<K, V>(this Dictionary<K, V> self, Action<K, V> action)
        {
            using (var dicE = self.GetEnumerator())
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
            using (var dicE = target.GetEnumerator())
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

        /*********************************Queue*********************************/
        public static Queue<T> ForEach<T>(this Queue<T> self, Action<T> action)
        {
            foreach (var item in self)
                action(item);
            return self;
        }

        /*********************************Stack*********************************/
        public static Stack<T> ForEach<T>(this Stack<T> self, Action<T> action)
        {
            foreach (var item in self)
                action(item);
            return self;
        }
    }
}