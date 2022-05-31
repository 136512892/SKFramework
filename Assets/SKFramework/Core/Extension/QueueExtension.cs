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
        /// <summary>
        /// 合并 目标队列中的元素将依次出列被合并
        /// </summary>
        /// <param name="target">合并的目标</param>
        public static Queue<T> Merge<T>(this Queue<T> self, Queue<T> target)
        {
            int count = target.Count;
            for (int i = 0; i < count; i++)
            {
                self.Enqueue(target.Dequeue());
            }
            return self;
        }
        /// <summary>
        /// 拷贝
        /// </summary>
        /// <returns>返回一个包含相同元素的新的队列</returns>
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