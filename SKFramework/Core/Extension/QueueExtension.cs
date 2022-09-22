using System;
using System.Collections.Generic;

namespace SK.Framework
{
    public static class QueueExtension
    {
        /// <summary>
        /// 遍历
        /// </summary>
        /// <param name="action">遍历事件</param>
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