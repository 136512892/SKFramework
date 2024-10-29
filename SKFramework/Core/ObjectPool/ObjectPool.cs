/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;

namespace SK.Framework.ObjectPool
{
    public class ObjectPool : ModuleBase
    {
        public bool IsExists<T>() where T : IPoolable
        {
            return ObjectPool<T>.IsExists;
        }

        public T Allocate<T>() where T : IPoolable
        {
            return ObjectPool<T>.Instance.Allocate();
        }

        public int GetCurrentCacheCount<T>() where T : IPoolable
        {
            return ObjectPool<T>.Instance.currentCacheCount;
        }

        public void SetMaxCacheCount<T>(int maxCacheCount) where T : IPoolable
        {
            ObjectPool<T>.Instance.maxCacheCount = maxCacheCount;
        }

        public void CreateBy<T>(Func<T> createMethod) where T : IPoolable
        {
            ObjectPool<T>.Instance.CreateBy(createMethod);
        }

        public bool Recycle<T>(T obj) where T : IPoolable
        {
            return ObjectPool<T>.Instance.Recycle(obj);
        }

        public void Release<T>() where T : IPoolable
        {
            if (ObjectPool<T>.IsExists)
                ObjectPool<T>.Instance.Release();
        }
    }

    public class ObjectPool<T> : IObjectPool<T> where T : IPoolable
    {
        private static ObjectPool<T> m_Instance;

        public static ObjectPool<T> Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    if (typeof(T).IsSubclassOf(typeof(MonoBehaviour)))
                    {
                        m_Instance = Activator.CreateInstance<ObjectPool<T>>();
                        m_Instance.m_CreateMethod = () => (T)(new GameObject(typeof(T).Name)
                            .AddComponent(typeof(T)) as object);
                    }
                    else
                    {
                        ConstructorInfo[] ctors = typeof(T).GetConstructors(
                            BindingFlags.Instance| BindingFlags.Public);
                        if (Array.FindIndex(ctors, m => m.GetParameters().Length == 0) != -1)
                        {
                            m_Instance = Activator.CreateInstance<ObjectPool<T>>();
                            m_Instance.m_CreateMethod = () => Activator.CreateInstance<T>();
                        }
                        else throw new ArgumentException();
                    }
                }
                return m_Instance;
            }
        }

        public static bool IsExists
        {
            get
            {
                return m_Instance != null;
            }
        }

        private Func<T> m_CreateMethod;

        private readonly Stack<T> m_Pool = new Stack<T>();

        public int currentCacheCount
        {
            get
            {
                return m_Pool.Count;
            }
        }

        public int m_MaxCacheCount = 9;

        public int maxCacheCount
        {
            get
            {
                return m_MaxCacheCount;
            }
            set
            {
                if (m_MaxCacheCount != value)
                {
                    m_MaxCacheCount = value;
                    if (m_MaxCacheCount > 0 && m_MaxCacheCount < m_Pool.Count)
                    {
                        int removeCount = m_Pool.Count - m_MaxCacheCount;
                        while (removeCount > 0)
                        {
                            T obj = m_Pool.Pop();
                            removeCount--;
                            if (typeof(T).IsSubclassOf(typeof(MonoBehaviour)))
                                UnityEngine.Object.Destroy((obj as MonoBehaviour).gameObject);
                        }
                    }
                }
            }
        }

        public void CreateBy(Func<T> createMethod)
        {
            m_CreateMethod = createMethod;
        }

        public T Allocate()
        {
            T obj = m_Pool.Count > 0
                ? m_Pool.Pop()
                : m_CreateMethod.Invoke();
            obj.isRecycled = false;
            return obj;
        }

        public bool Recycle(T obj)
        {
            if (obj == null || obj.isRecycled) return false;
            obj.isRecycled = true;
            obj.OnRecycled();

            if (m_Pool.Count < m_MaxCacheCount)
                m_Pool.Push(obj);
            else
            {
                if (typeof(T).IsSubclassOf(typeof(MonoBehaviour)))
                    UnityEngine.Object.Destroy((obj as MonoBehaviour).gameObject);
            }
            return true;
        }

        public void Release()
        {
            if (typeof(T).IsSubclassOf(typeof(MonoBehaviour)))
            {
                foreach (var obj in m_Pool)
                    UnityEngine.Object.Destroy((obj as MonoBehaviour).gameObject);
            }
            m_Pool.Clear();
            m_Instance = null;
        }
    }
}