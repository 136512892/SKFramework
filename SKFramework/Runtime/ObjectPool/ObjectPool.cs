/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;
using Object = UnityEngine.Object;

using SK.Framework.Logger;
using ILogger = SK.Framework.Logger.ILogger;

namespace SK.Framework.ObjectPool
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/SKFramework.Object Pool")]
    public class ObjectPool : ModuleBase
    {
        private readonly Dictionary<Type, IObjectPool> m_Dic = new Dictionary<Type, IObjectPool>();
        private ILogger m_Logger;

        protected internal override void OnInitialization()
        {
            base.OnInitialization();
            m_Logger = SKFramework.Module<Log>().GetLogger<ModuleLogger>();
        }

        protected internal override void OnUpdate()
        {
            base.OnUpdate();
            var pools = m_Dic.Values.GetEnumerator();
            while (pools.MoveNext())
            {
                pools.Current?.Update();
            }
        }

        protected internal override void OnTermination()
        {
            base.OnTermination();
            m_Dic.Clear();
            m_Logger = null;
        }

        public bool Create<T>() where T : IPoolable, new()
        {
            if (!m_Dic.ContainsKey(typeof(T)))
            {
                var ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.Public);
                if (Array.FindIndex(ctors, m => m.GetParameters().Length == 0) != -1
                    && Activator.CreateInstance(typeof(ObjectPool<T>)) is ObjectPool<T> pool)
                {
                    pool.Init(Activator.CreateInstance<T>, false);
                    m_Dic.Add(typeof(T), pool);
                    m_Logger.Info("[ObjectPool] Create object pool: {0}", typeof(T).FullName);
                    return true;
                }
                m_Logger.Error("[ObjectPool] A constructor with 0 arguments does not exist in the type {0}.", typeof(T).FullName);
                return false;
            }
            m_Logger.Warning("[ObjectPool] An object pool of type {0} already exists.", typeof(T).FullName);
            return false;
        }

        public bool Create<T>(Func<T> createBy) where T : MonoBehaviour, IPoolable
        {
            if (!m_Dic.ContainsKey(typeof(T)))
            {
                var pool = Activator.CreateInstance<ObjectPool<T>>();
                pool.Init(createBy ?? (() => new GameObject(typeof(T).FullName).AddComponent<T>()), true);
                m_Dic.Add(typeof(T), pool);
                m_Logger.Info("[ObjectPool] Create object pool: {0}", typeof(T).FullName);
                return true;
            }
            m_Logger.Warning("[ObjectPool] An object pool of type {0} already exists.", typeof(T).FullName);
            return false;
        }

        public ObjectPool<T> Get<T>() where T : IPoolable, new()
        {
            if (!m_Dic.TryGetValue(typeof(T), out var pool))
            {
                return Create<T>() ? m_Dic[typeof(T)] as ObjectPool<T> : null;
            }
            return pool as ObjectPool<T>;
        }

        public ObjectPool<T> Get<T>(Func<T> createBy) where T : MonoBehaviour, IPoolable
        {
            if (!m_Dic.TryGetValue(typeof(T), out var pool))
            {
                return Create(createBy) ? m_Dic[typeof(T)] as ObjectPool<T> : null;
            }
            return pool as ObjectPool<T>;
        }

        public bool TryGet(Type type, out IObjectPool pool)
        {
            return m_Dic.TryGetValue(type, out pool);
        }

        public void Release<T>() where T : IPoolable
        {
            if (m_Dic.TryGetValue(typeof(T), out var pool) && pool is ObjectPool<T> matched)
            {
                matched.Release();
                m_Dic.Remove(typeof(T));
                m_Logger.Info("[ObjectPool] Release object pool: {0}", typeof(T).FullName);
            }
            else
            {
                m_Logger.Warning("[ObjectPool] An object pool of type {0} does not exists.", typeof(T).FullName);
            }
        }

        internal void Remove(IObjectPool pool)
        {
            var type = pool.GetType();
            if (m_Dic.ContainsKey(type))
            {
                m_Dic.Remove(type);
                m_Logger.Info("[ObjectPool] Remove object pool: {0}", type.FullName);
            }
            else
            {
                m_Logger.Warning("[ObjectPool] An object pool of type {0} does not exists.", type.FullName);
            }
        }
    }

    public class ObjectPool<T> : IObjectPool where T : IPoolable
    {
        private Func<T> m_CreateMethod;

        private readonly Queue<object> m_Pool = new Queue<object>();

        private bool m_IsSubclassMonoBehaviour;

        //Maximum number of caches in the object pool.
        private int m_MaxCacheCount = 99;

        //Maximum duration(in minutes) for which an object is held by the object pool.
        private float m_MaxKeepDuration = 2f;

        public int currentCacheCount { get { return m_Pool.Count; } }

        public int maxCacheCount
        {
            get
            {
                return m_MaxCacheCount;
            }
            set
            {
                if (m_MaxCacheCount == value) 
                    return;
                m_MaxCacheCount = value;
                if (m_MaxCacheCount <= 0 || m_MaxCacheCount >= m_Pool.Count) 
                    return;
                var removeCount = m_Pool.Count - m_MaxCacheCount;
                while (removeCount > 0)
                {
                    var obj = m_Pool.Dequeue() as IPoolable;
                    removeCount--;
                    if (m_IsSubclassMonoBehaviour && obj is MonoBehaviour mono)
                        Object.Destroy(mono.gameObject);
                }
            }
        }

        public float maxKeepDuration
        {
            get
            {
                return m_MaxKeepDuration;
            }
            set
            {
                if (m_MaxKeepDuration != value)
                {
                    m_MaxKeepDuration = value;
                }
            }
        }

        internal void Init(Func<T> createMethod, bool isSubclassMonoBehaviour)
        {
            m_CreateMethod = createMethod;
            m_IsSubclassMonoBehaviour = isSubclassMonoBehaviour;
        }

        public T Allocate()
        {
            var obj = (T)(m_Pool.Count > 0 ? m_Pool.Dequeue() : m_CreateMethod.Invoke());
            obj.isRecycled = false;
            return obj;
        }

        public bool Recycle<O>(O obj) where O : IPoolable
        {
            if (obj == null || obj.isRecycled)
                return false;
            obj.isRecycled = true;
            obj.entryTime = DateTime.Now;
            obj.OnRecycled();

            if (m_Pool.Count < m_MaxCacheCount)
                m_Pool.Enqueue(obj);
            else
            {
                if (m_IsSubclassMonoBehaviour && obj is MonoBehaviour mono)
                    Object.Destroy(mono.gameObject);
            }
            return true;
        }

        public void Update()
        {
            if (m_Pool.Count <= 0) 
                return;
            if (m_Pool.First() is IPoolable first && (DateTime.Now - first.entryTime).TotalMinutes > m_MaxKeepDuration)
            {
                var obj = m_Pool.Dequeue();
                if (m_IsSubclassMonoBehaviour && obj is MonoBehaviour mono)
                    Object.Destroy(mono.gameObject);
            }
        }

        internal void Release()
        {
            if (m_IsSubclassMonoBehaviour)
            {
                foreach (var obj in m_Pool)
                {
                    if (obj is MonoBehaviour mono)
                    {
                        Object.Destroy(mono.gameObject);
                    }
                }
            }
            m_Pool.Clear();
        }
    }
}