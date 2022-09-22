using System;
using System.Collections.Generic;

namespace SK.Framework
{
    public static class ListExtension
    {
        /// <summary>
        /// 遍历
        /// </summary>
        /// <param name="action">遍历事件</param>
        public static List<T> ForEach<T>(this List<T> self, Action<T> action)
        {
            for (int i = 0; i < self.Count; i++)
            {
                action(self[i]);
            }
            return self;
        }
        /// <summary>
        /// 遍历
        /// </summary>
        /// <param name="action">遍历事件</param>
        public static List<T> ForEach<T>(this List<T> self, Action<int, T> action)
        {
            for (int i = 0; i < self.Count; i++)
            {
                action(i, self[i]);
            }
            return self;
        }
        /// <summary>
        /// 倒序遍历
        /// </summary>
        /// <param name="action">遍历事件</param>
        public static List<T> ForEachReverse<T>(this List<T> self, Action<T> action)
        {
            for (int i = self.Count - 1; i >= 0; i--)
            {
                action(self[i]);
            }
            return self;
        }
        /// <summary>
        /// 倒序遍历
        /// </summary>
        /// <param name="action">遍历事件</param>
        public static List<T> ForEachReverse<T>(this List<T> self, Action<int ,T> action)
        {
            for (int i = self.Count - 1; i >= 0; i--)
            {
                action(i, self[i]);
            }
            return self;
        }
        /// <summary>
        /// 尝试添加
        /// </summary>
        /// <param name="t">添加的目标</param>
        /// <returns>添加成功返回true 否则返回false</returns>
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