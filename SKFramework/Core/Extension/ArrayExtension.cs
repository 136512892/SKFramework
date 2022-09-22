using System;

namespace SK.Framework
{
    public static class ArrayExtension
    {
        /// <summary>
        /// 遍历
        /// </summary>
        /// <param name="action">遍历事件</param>
        public static T[] ForEach<T>(this T[] self, Action<int, T> action)
        {
            for (int i = 0; i < self.Length; i++)
            {
                action(i, self[i]);
            }
            return self;
        }
        /// <summary>
        /// 倒序遍历
        /// </summary>
        /// <param name="action">遍历事件</param>
        public static T[] ForEachReverse<T>(this T[] self, Action<T> action)
        {
            for (int i = self.Length - 1; i >= 0; i--)
            {
                action(self[i]);
            }
            return self;
        }
        /// <summary>
        /// 倒序遍历
        /// </summary>
        /// <param name="action">遍历事件</param>
        public static T[] ForEachReverse<T>(this T[] self, Action<int, T> action)
        {
            for(int i = self.Length - 1; i>=0; i--)
            {
                action(i, self[i]);
            }
            return self;
        }
    }
}