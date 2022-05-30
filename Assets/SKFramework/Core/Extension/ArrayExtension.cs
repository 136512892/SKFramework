using System;
using System.Collections.Generic;

namespace SK.Framework
{
    public static class ArrayExtension
    {
        public static T[] ForEach<T>(this T[] self, Action<T> action)
        {
            for (int i = 0; i < self.Length; i++)
            {
                action(self[i]);
            }
            return self;
        }
        public static T[] ForEach<T>(this T[] self, Action<int, T> action)
        {
            for (int i = 0; i < self.Length; i++)
            {
                action(i, self[i]);
            }
            return self;
        }
        public static T[] ForEachReverse<T>(this T[] self, Action<T> action)
        {
            for (int i = self.Length - 1; i >= 0; i--)
            {
                action(self[i]);
            }
            return self;
        }
        public static T[] ForEachReverse<T>(this T[] self, Action<int, T> action)
        {
            for(int i = self.Length - 1; i>=0; i--)
            {
                action(i, self[i]);
            }
            return self;
        }
        public static T[] Merge<T>(this T[] self, T[] target)
        {
            T[] retArray = new T[self.Length + target.Length];
            for (int i = 0; i < self.Length; i++)
            {
                retArray[i] = self[i];
            }
            for (int i = 0; i < target.Length; i++)
            {
                retArray[i + self.Length] = target[i];
            }
            return retArray;
        }
        public static T[] Merge<T>(this T[] self, List<T> target)
        {
            T[] retArray = new T[self.Length + target.Count];
            for (int i = 0; i < self.Length; i++)
            {
                retArray[i] = self[i];
            }
            for (int i = 0; i < target.Count; i++)
            {
                retArray[i + self.Length] = target[i];
            }
            return retArray;
        }
        public static T[] Copy<T>(this T[] self)
        {
            T[] retArray = new T[self.Length];
            for (int i = 0; i < self.Length; i++)
            {
                retArray[i] = self[i];
            }
            return retArray;
        }
        /// <summary>
        /// 插入排序
        /// </summary>
        /// <param name="self">数组</param>
        /// <returns>返回排序后的数组</returns>
        public static int[] SortInsertion(this int[] self)
        {
            for (int i = 1; i < self.Length; i++)
            {
                int temp = self[i];
                int j = i;
                while (j > 0 && self[j - 1] > temp) 
                {
                    self[j] = self[j - 1];
                    j--;
                }
                self[j] = temp;
            }
            return self;
        }
        /// <summary>
        /// 希尔排序
        /// </summary>
        /// <param name="self">数组</param>
        /// <returns>返回排序后的数组</returns>
        public static int[] SortShell(this int[] self)
        {
            int inc;
            for (inc = 1; inc <= self.Length / 9; inc = 3 * inc + 1) ;
            for (; inc > 0; inc /= 3) 
            {
                for (int i = inc + 1; i <= self.Length; i+= inc)
                {
                    int t = self[i - 1];
                    int j = i;
                    while (j > inc && self[j - inc - 1] > t) 
                    {
                        self[j - 1] = self[j - inc - 1];
                        j -= inc;
                    }
                    self[j - 1] = t;
                }
            }
            return self;
        }
        /// <summary>
        /// 选择排序
        /// </summary>
        /// <param name="self">数组</param>
        /// <returns>返回排序后的数组</returns>
        public static int[] SortSelection(this int[] self)
        {
            for (int i = 0; i < self.Length - 1; i++)
            {
                int min = i;
                for (int j = i + 1; j < self.Length; j++)
                {
                    if (self[j] < self[min]) 
                    {
                        min = j;
                    }
                }
                int t = self[min];
                self[min] = self[i];
                self[i] = t;
            }
            return self;
        }
        /// <summary>
        /// 冒泡排序
        /// </summary>
        /// <param name="self">数组</param>
        /// <returns>返回排序后的数组</returns>
        public static int[] SortBubble(this int[] self) 
        {
            for (int i = 0; i < self.Length; i++)
            {
                for (int j = self.Length - 2; j >= i; j--)
                {
                    if (self[j + 1] < self[j]) 
                    {
                        int t = self[j + 1];
                        self[j + 1] = self[j];
                        self[j] = t;
                    }
                }
            }
            return self;
        }
    }
}