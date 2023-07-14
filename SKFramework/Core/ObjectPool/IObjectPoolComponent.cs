using System;

namespace SK.Framework.ObjectPool
{
    public interface IObjectPoolComponent
    {
        /// <summary>
        /// 从对象池中获取对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>对象</returns>
        T Allocate<T>() where T : IPoolable;

        /// <summary>
        /// 将对象回收到对象池中
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="t">对象实例</param>
        /// <returns>true：回收成功  false：回收失败</returns>
        bool Recycle<T>(T t) where T : IPoolable;

        /// <summary>
        /// 释放对象池
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        void Release<T>() where T : IPoolable;

        /// <summary>
        /// 设置对象池的最大缓存数量
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="maxCacheCount">最大缓存数量</param>
        void SetMaxCacheCount<T>(int maxCacheCount) where T : IPoolable;

        /// <summary>
        /// 获取对象池中当前的缓存数量
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>当前缓存数量</returns>
        int GetCurrentCacheCount<T>() where T : IPoolable;

        /// <summary>
        /// 设置对象的创建方法
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="createMethod">创建方法</param>
        void CreateBy<T>(Func<T> createMethod) where T : IPoolable;
    }
}