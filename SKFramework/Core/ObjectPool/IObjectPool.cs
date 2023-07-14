using System;

namespace SK.Framework.ObjectPool
{
    public interface IObjectPool<T>
    {
        /// <summary>
        /// 对象池中当前的缓存数量
        /// </summary>
        int CurrentCacheCount { get; }

        /// <summary>
        /// 对象池的最大缓存数量
        /// </summary>
        int MaxCacheCount { get; set; }

        /// <summary>
        /// 设置对象的创建方法
        /// （从对象池中获取对象时 如果池中为空 需要调用创建方法来创建新的对象实例）
        /// </summary>
        /// <param name="createMethod">创建方法</param>
        void CreateBy(Func<T> createMethod);

        /// <summary>
        /// 从对象池中获取对象
        /// </summary>
        /// <returns>对象</returns>
        T Allocate();

        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="t">对象</param>
        /// <returns>true：回收成功  false：回收失败</returns>
        bool Recycle(T t);

        /// <summary>
        /// 释放对象池
        /// </summary>
        void Release();
    }
}