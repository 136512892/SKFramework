using System;
using UnityEngine;

namespace SK.Framework.ObjectPool
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/Mono Object Pool")]
    public class MonoObjectPoolComponent : MonoBehaviour
    {
        public T Allocate<T>() where T : MonoBehaviour, IPoolable
        {
            return MonoObjectPool<T>.Instance.Allocate();
        }

        public bool Recycle<T>(T t) where T : MonoBehaviour, IPoolable
        {
            return MonoObjectPool<T>.Instance.Recycle(t);
        }

        public void Release<T>() where T : MonoBehaviour, IPoolable
        {
            MonoObjectPool<T>.Instance.Release();
        }

        public void SetMaxCacheCount<T>(int maxCacheCount) where T : MonoBehaviour, IPoolable
        {
            MonoObjectPool<T>.Instance.MaxCacheCount = maxCacheCount;
        }

        public void CreateBy<T>(Func<T> createMethod) where T : MonoBehaviour, IPoolable
        {
            MonoObjectPool<T>.Instance.CreateBy(createMethod);
        }
    }
}