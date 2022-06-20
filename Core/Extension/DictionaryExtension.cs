using System;
using System.Collections.Generic;

namespace SK.Framework
{
    public static class DictionaryExtension
    {
        /// <summary>
        /// 拷贝字典
        /// </summary>
        public static Dictionary<K, V> Copy<K, V>(this Dictionary<K, V> self)
        {
            Dictionary<K, V> retDic = new Dictionary<K, V>(self.Count);
            using (var dicE = self.GetEnumerator())
            {
                while (dicE.MoveNext())
                {
                    retDic.Add(dicE.Current.Key, dicE.Current.Value);
                }
            }
            return retDic;
        }
        /// <summary>
        /// 遍历字典
        /// </summary>
        /// <param name="action">遍历事件</param>
        public static Dictionary<K, V> ForEach<K, V>(this Dictionary<K, V> self, Action<K, V> action)
        {
            using(var dicE = self.GetEnumerator())
            {
                while (dicE.MoveNext())
                {
                    action(dicE.Current.Key, dicE.Current.Value);
                }
            }
            return self;
        }
        /// <summary>
        /// 合并字典
        /// </summary>
        /// <param name="target">被合并的字典</param>
        /// <param name="isOverride">若存在相同键，是否覆盖对应值</param>
        /// <returns>合并后的字典</returns>
        public static Dictionary<K, V> AddRange<K, V>(this Dictionary<K, V> self, Dictionary<K, V> target, bool isOverride = false)
        {
            using(var dicE = target.GetEnumerator())
            {
                while (dicE.MoveNext())
                {
                    var current = dicE.Current;
                    if (self.ContainsKey(current.Key))
                    {
                        if (isOverride)
                        {
                            self[current.Key] = current.Value;
                            continue;
                        }
                    }
                    self.Add(current.Key, current.Value);
                }
            }
            return self;
        }
        /// <summary>
        /// 将字典的所有值放入一个列表
        /// </summary>
        /// <returns>列表</returns>
        public static List<V> Value2List<K, V>(this Dictionary<K, V> self)
        {
            List<V> retList = new List<V>(self.Count);
            foreach (var kv in self)
            {
                retList.Add(kv.Value);
            }
            return retList;
        }
        /// <summary>
        /// 将字典的所有值放入一个数组
        /// </summary>
        /// <returns>数组</returns>
        public static V[] Value2Array<K, V>(this Dictionary<K, V> self)
        {
            V[] retArray = new V[self.Count];
            int index = -1;
            foreach (var kv in self)
            {
                retArray[++index] = kv.Value;
            }
            return retArray;
        }
        /// <summary>
        /// 尝试添加
        /// </summary>
        /// <param name="k">键</param>
        /// <param name="v">值</param>
        /// <returns>若不存在相同键，添加成功并返回true，否则返回false</returns>
        public static bool TryAdd<K, V>(this Dictionary<K, V> self, K k, V v)
        {
            if (!self.ContainsKey(k))
            {
                self.Add(k, v);
                return true;
            }
            return false;
        }
    }
}