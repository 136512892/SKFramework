using System;
using System.Collections.Generic;

namespace SK.Framework
{
    public static class StackExtension
    {
        /// <summary>
        /// 遍历
        /// </summary>
        /// <param name="action">遍历事件</param>
        public static Stack<T> ForEach<T>(this Stack<T> self, Action<T> action)
        {
            foreach (var item in self)
            {
                action(item);
            }
            return self;
        }
    }
}