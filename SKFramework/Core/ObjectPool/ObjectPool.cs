using System;
using System.Reflection;
using System.Collections.Generic;

namespace SK.Framework.ObjectPool
{
    internal class ObjectPool<T> : IObjectPool<T> where T : IPoolable, new()
    {
        private static ObjectPool<T> instance;

        private int maxCacheCount = 9;

        private readonly Stack<T> pool = new Stack<T>();

        public static ObjectPool<T> Instance
        {
            get
            {
                if (null == instance)
                {
                    var ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.Public);
                    var index = Array.FindIndex(ctors, m => m.GetParameters().Length == 0);
                    if (index == -1)
                    {
                        UnityEngine.Debug.LogError(string.Format("[{0}]类型不具有无参构造函数", typeof(T).Name));
                    }
                    else
                    {
                        instance = Activator.CreateInstance<ObjectPool<T>>();
                    }
                }
                return instance;
            }
        }

        public int CurrentCacheCount
        {
            get
            {
                return pool.Count;
            }
        }
        
        public int MaxCacheCount
        {
            get
            {
                return maxCacheCount;
            }
            set
            {
                if (maxCacheCount != value)
                {
                    maxCacheCount = value;
                    if (maxCacheCount > 0 && maxCacheCount < pool.Count)
                    {
                        int removeCount = pool.Count - maxCacheCount;
                        while (removeCount > 0)
                        {
                            pool.Pop();
                            removeCount--;
                        }
                    }
                }
            }
        }

        public T Allocate()
        {
            T retT = pool.Count > 0 ? pool.Pop() : new T();
            retT.IsRecycled = false;
            return retT;
        }

        public bool Recycle(T t)
        {
            if (null == t || t.IsRecycled) return false;
            t.IsRecycled = true;
            t.OnRecycled();
            if (pool.Count < maxCacheCount)
            {
                pool.Push(t);
            }
            return true;
        }

        public void Release()
        {
            pool.Clear();
            instance = null;
        }
    }
}