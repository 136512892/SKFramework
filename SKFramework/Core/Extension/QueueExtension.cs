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
    }
}