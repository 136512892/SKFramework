using System;
using System.Collections.Generic;

namespace SK.Framework
{
    public static class ListExtension
    {
        public static List<T> ForEach<T>(this List<T> self, Action<T> action)
        {
            for (int i = 0; i < self.Count; i++)
            {
                action(self[i]);
            }
            return self;
        }
        public static List<T> ForEach<T>(this List<T> self, Action<int, T> action)
        {
            for (int i = 0; i < self.Count; i++)
            {
                action(i, self[i]);
            }
            return self;
        }
        public static List<T> ForEachReverse<T>(this List<T> self, Action<T> action)
        {
            for (int i = self.Count - 1; i >= 0; i--)
            {
                action(self[i]);
            }
            return self;
        }
        public static List<T> ForEachReverse<T>(this List<T> self, Action<int ,T> action)
        {
            for (int i = self.Count - 1; i >= 0; i--)
            {
                action(i, self[i]);
            }
            return self;
        }
        public static List<T> TryAdd<T>(this List<T> self, T t)
        {
            if (!self.Contains(t))
            {
                self.Add(t);
            }
            return self;
        }
    }
}