using System;
using UnityEngine;

namespace SK.Framework.ObjectPool
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/Object Pool")]
    public class ObjectPoolComponent : MonoBehaviour, IObjectPoolComponent
    {
        /// <summary>
        /// 从对象池中获取对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>对象</returns>
        public T Allocate<T>() where T : IPoolable
        {
            return ObjectPool<T>.Instance.Allocate();
        }

        /// <summary>
        /// 将对象回收到对象池中
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="t">对象实例</param>
        /// <returns>true：回收成功  false：回收失败</returns>
        public bool Recycle<T>(T t) where T : IPoolable
        {
            return ObjectPool<T>.Instance.Recycle(t);
        }

        /// <summary>
        /// 释放对象池
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        public void Release<T>() where T : IPoolable
        {
            ObjectPool<T>.Instance.Release();
        }

        /// <summary>
        /// 设置对象池的最大缓存数量
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="maxCacheCount">最大缓存数量</param>
        public void SetMaxCacheCount<T>(int maxCacheCount) where T : IPoolable
        {
            ObjectPool<T>.Instance.MaxCacheCount = maxCacheCount;
        }

        /// <summary>
        /// 获取对象池中当前的缓存数量
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>当前缓存数量</returns>
        public int GetCurrentCacheCount<T>() where T : IPoolable
        {
            return ObjectPool<T>.Instance.CurrentCacheCount;
        }

        /// <summary>
        /// 设置对象的创建方法
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="createMethod">创建方法</param>
        public void CreateBy<T>(Func<T> createMethod) where T : IPoolable
        {
            ObjectPool<T>.Instance.CreateBy(createMethod);
        }
    }
}