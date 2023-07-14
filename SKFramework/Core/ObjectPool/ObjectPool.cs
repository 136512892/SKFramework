using System;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace SK.Framework.ObjectPool
{
    internal class ObjectPool<T> : IObjectPool<T> where T : IPoolable
    {
        private static ObjectPool<T> instance;

        //最大缓存数量
        private int maxCacheCount = 9;

        //对象池容器
        private readonly Stack<T> pool = new Stack<T>();

        //对象创建方法
        private Func<T> createMethod;

        public static ObjectPool<T> Instance
        {
            get
            {
                if (null == instance)
                {
                    //Mono类型的对象
                    if (typeof(T).IsSubclassOf(typeof(MonoBehaviour)))
                    {
                        instance = Activator.CreateInstance<ObjectPool<T>>();
                        instance.createMethod = () => (T)(new GameObject(typeof(T).Name).AddComponent(typeof(T)) as object);
                    }
                    else
                    {
                        var ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.Public);
                        //是否有无参构造函数
                        if (Array.FindIndex(ctors, m => m.GetParameters().Length == 0) != -1)
                        {
                            instance = Activator.CreateInstance<ObjectPool<T>>();
                            instance.createMethod = () => Activator.CreateInstance<T>();
                        }
                        else Debug.LogError(string.Format("[{0}]类型不具有无参构造函数", typeof(T).Name));
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// 对象池中当前的缓存数量
        /// </summary>
        public int CurrentCacheCount
        {
            get
            {
                return pool.Count;
            }
        }
        
        /// <summary>
        /// 对象池的最大缓存数量
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
                            removeCount--;
                            //Mono类型的对象 需要销毁
                            if (typeof(T).IsSubclassOf(typeof(MonoBehaviour)))
                                UnityEngine.Object.Destroy((t as MonoBehaviour).gameObject);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 设置对象的创建方法
        /// （从对象池中获取对象时 如果池中为空 需要调用创建方法来创建新的对象实例）
        /// </summary>
        /// <param name="createMethod">创建方法</param>
        public void CreateBy(Func<T> createMethod)
        {
            this.createMethod = createMethod;
        }

        /// <summary>
        /// 从对象池中获取对象
        /// </summary>
        /// <returns>对象</returns>
        public T Allocate()
        {
            //当前池中数量不为0 从池中获取 否则通过创建方法创建新的对象
            T retT = pool.Count > 0 ? pool.Pop() : createMethod.Invoke();
            retT.IsRecycled = false;
            return retT;
        }

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="t">对象</param>
        /// <returns>true：回收成功  false：回收失败</returns>
        public bool Recycle(T t)
        {
            if (null == t || t.IsRecycled) return false;
            t.IsRecycled = true;
            t.OnRecycled();
            
            //池中数量未达到最大缓存数量上限 将其放入池中缓存
            if (pool.Count < maxCacheCount)
                pool.Push(t);
            //池中数量已达到最大缓存数量上限
            else
            {
                //Mono类型的对象 直接销毁
                if (typeof(T).IsSubclassOf(typeof(MonoBehaviour)))
                    UnityEngine.Object.Destroy((t as MonoBehaviour).gameObject);
            }
            return true;
        }
        
        /// <summary>
        /// 释放对象池
        /// </summary>
        public void Release()
        {
            //Mono类型的对象 全部销毁
            if (typeof(T).IsSubclassOf(typeof(MonoBehaviour)))
            {
                foreach (var t in pool)
                {
                    UnityEngine.Object.Destroy((t as MonoBehaviour).gameObject);
                }
            }
            pool.Clear();
            instance = null;
        }
    }
}