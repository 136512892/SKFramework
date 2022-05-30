using System;
using System.Collections.Generic;

namespace SK.Framework
{
    public static class QueueExtension
    {
        public static Queue<T> ForEach<T>(this Queue<T> self, Action<T> action)
        {
            foreach (var item in self)
            {
                action(item);
            }
            return self;
        }
        public static T[] ToArray<T>(this Queue<T> self)
        {
            T[] retArray = new T[self.Count];
            for (int i = 0; i < self.Count; i++)
            {
                retArray[i] = self.Dequeue();
            }
            return retArray;
        }
        public static List<T> ToList<T>(this Queue<T> self)
        {
            List<T> retList = new List<T>(self.Count);
            int count = self.Count;
            for (int i = 0; i < count; i++)
            {
                retList.Add(self.Dequeue());
            }
            return retList;
        }
        public static Queue<T> Merge<T>(this Queue<T> self, Queue<T> target)
        {
            int count = target.Count;
            for (int i = 0; i < count; i++)
            {
                self.Enqueue(target.Dequeue());
            }
            return self;
        }
        public static Queue<T> Copy<T>(this Queue<T> self)
        {
            Queue<T> retQueue = new Queue<T>(self.Count);
            foreach (var item in self)
            {
                retQueue.Enqueue(item);
            }
            return retQueue;
        }
    }
}