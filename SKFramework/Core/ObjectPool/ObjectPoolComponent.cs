using UnityEngine;

namespace SK.Framework.ObjectPool
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/Object Pool")]
    public class ObjectPoolComponent : MonoBehaviour
    {
        public MonoObjectPoolComponent Mono { get; private set; }

        private void Awake()
        {
            Mono = GetComponent<MonoObjectPoolComponent>();
        }

        public T Allocate<T>() where T : IPoolable, new()
        {
            return ObjectPool<T>.Instance.Allocate();
        }

        public bool Recycle<T>(T t) where T : IPoolable, new()
        {
            return ObjectPool<T>.Instance.Recycle(t);
        }

        public void Release<T>() where T : IPoolable, new()
        {
            ObjectPool<T>.Instance.Release();
        }

        public void SetMaxCacheCount<T>(int maxCacheCount) where T : IPoolable, new()
        {
            ObjectPool<T>.Instance.MaxCacheCount = maxCacheCount;
        }

        public int GetCurrentCacheCount<T>() where T : IPoolable, new()
        {
            return ObjectPool<T>.Instance.CurrentCacheCount;
        }
    }
}