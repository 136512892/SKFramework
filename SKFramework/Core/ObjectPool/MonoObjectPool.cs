using System;
using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework.ObjectPool
{
    internal class MonoObjectPool<T> : IObjectPool<T> where T : MonoBehaviour, IPoolable
    {
        private static MonoObjectPool<T> instance;

        private int maxCacheCount = 9;

        private readonly Stack<T> pool = new Stack<T>();

        private Func<T> createMethod;

        public static MonoObjectPool<T> Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = Activator.CreateInstance<MonoObjectPool<T>>();
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
                            T t = pool.Pop();
                            UnityEngine.Object.Destroy(t.gameObject);
                            removeCount--;
                        }
                    }
                }
            }
        }

        public T Allocate()
        {
            T retT = pool.Count > 0
                ? pool.Pop()
                : (createMethod != null ? createMethod.Invoke() : new GameObject().AddComponent<T>());
            retT.hideFlags = HideFlags.HideInHierarchy;
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
            else
            {
                UnityEngine.Object.Destroy(t.gameObject);
            }
            return true;
        }

        public void Release()
        {
            foreach (var o in pool)
            {
                UnityEngine.Object.Destroy(o.gameObject);
            }
            pool.Clear();
            instance = null;
        }

        public void CreateBy(Func<T> createMethod)
        {
            this.createMethod = createMethod;
        }
    }
}