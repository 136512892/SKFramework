using System;
using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework
{
    public static class MonoObjectPool
    {
        /// <summary>
        /// 从对象池中获取实例
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>对象实例</returns>
        public static T Allocate<T>() where T : MonoBehaviour, IPoolable
        {
            return MonoObjectPool<T>.Instance.Allocate();
        }
        /// <summary>
        /// 回收对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="t">回收对象实例</param>
        /// <returns>回收成功返回true 否则返回false</returns>
        public static bool Recycle<T>(T t) where T : MonoBehaviour, IPoolable
        {
            return MonoObjectPool<T>.Instance.Recycle(t);
        }
        /// <summary>
        /// 释放对象池
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        public static void Release<T>() where T : MonoBehaviour, IPoolable
        {
            MonoObjectPool<T>.Instance.Release();
        }
        /// <summary>
        /// 设置对象池缓存数量上限
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="maxCacheCount">缓存数量上限</param>
        public static void SetMaxCacheCount<T>(int maxCacheCount) where T : MonoBehaviour, IPoolable
        {
            MonoObjectPool<T>.Instance.MaxCacheCount = maxCacheCount;
        }
        /// <summary>
        /// 设置对象创建方法
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="createMethod">创建方法</param>
        public static void CreateBy<T>(Func<T> createMethod) where T : MonoBehaviour, IPoolable
        {
            MonoObjectPool<T>.Instance.CreateBy(createMethod);
        }

    }
    public class MonoObjectPool<T> : IObjectPool<T> where T : MonoBehaviour, IPoolable
    {
        private static MonoObjectPool<T> instance;
        //对象池缓存数量上限 默认9
        private int maxCacheCount = 9;
        //对象池
        private readonly Stack<T> pool = new Stack<T>();
        //创建方法
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
        /// <summary>
        /// 当前池中缓存的数量
        /// </summary>
        public int CurrentCacheCount
        {
            get
            {
                return pool.Count;
            }
        }
        /// <summary>
        /// 缓存数量上限
        /// </summary>
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
                    Log.Info("<color=cyan><b>[SKFramework.ObjectPool.Info]</b></color> 对象池[{0}]最大缓存数量设置为[{1}]", typeof(MonoObjectPool<T>).Name, value);
                }
            }
        }
        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns>对象实例</returns>
        public T Allocate()
        {
            //若对象池中有缓存则从对象池中获取
            T retT = pool.Count > 0
                ? pool.Pop()
                : (createMethod != null ? createMethod.Invoke() : new GameObject().AddComponent<T>());
            retT.hideFlags = HideFlags.HideInHierarchy;
            retT.IsRecycled = false;
            Log.Info("<color=cyan><b>[SKFramework.ObjectPool.Info]</b></color> 对象池[{0}]分配对象 当前池中数量[{1}]", typeof(MonoObjectPool<T>).Name, pool.Count);
            return retT;
        }
        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="t">回收对象实例</param>
        /// <returns>回收成功返回true 否则返回false</returns>
        public bool Recycle(T t)
        {
            if (null == t || t.IsRecycled) return false;
            t.IsRecycled = true;
            t.OnRecycled();
            //若未达到缓存数量上限 放入池中
            if (pool.Count < maxCacheCount)
            {
                pool.Push(t);
            }
            else
            {
                UnityEngine.Object.Destroy(t.gameObject);
            }
            Log.Info("<color=cyan><b>[SKFramework.ObjectPool.Info]</b></color> 对象池[{0}]回收对象 当前池中数量[{1}]", typeof(MonoObjectPool<T>).Name, pool.Count);
            return true;
        }
        /// <summary>
        /// 释放对象池
        /// </summary>
        public void Release()
        {
            foreach (var o in pool)
            {
                UnityEngine.Object.Destroy(o.gameObject);
            }
            pool.Clear();
            instance = null;
            Log.Info("<color=cyan><b>[SKFramework.ObjectPool.Info]</b></color> 对象池[{0}]被释放", typeof(MonoObjectPool<T>).Name);
        }
        /// <summary>
        /// 设置创建方法
        /// </summary>
        /// <param name="createMethod">创建方法</param>
        public void CreateBy(Func<T> createMethod)
        {
            this.createMethod = createMethod;
            Log.Info("<color=cyan><b>[SKFramework.ObjectPool.Info]</b></color> 对象池[{0}]设置创建方法 {1}", typeof(MonoObjectPool<T>).Name, createMethod.Method);
        }
    }
}