using System;
using System.Collections.Generic;

namespace SK.Framework
{
    public static class StackExtension
    {
        public static Stack<T> ForEach<T>(this Stack<T> self, Action<T> action)
        {
            foreach (var item in self)
            {
                action(item);
            }
            return self;
        }
        public static T[] ToArray<T>(this Stack<T> self)
        {
            T[] retArray = new T[self.Count];
            for (int i = 0; i < retArray.Length; i++)
            {
                retArray[i] = self.Pop();
            }
            return retArray;
        }
        public static List<T> ToList<T>(this Stack<T> self)
        {
            List<T> retList = new List<T>(self.Count);
            int count = self.Count;
            for (int i = 0; i < count; i++)
            {
                retList.Add(self.Pop());
            }
            return retList;
        }
    }
}