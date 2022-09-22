namespace SK.Framework.ObjectPool
{
    /// <summary>
    /// 对象池接口
    /// </summary>
    /// <typeparam name="T">对象池类型</typeparam>
    public interface IObjectPool<T>
    {
        /// <summary>
        /// 当前池中缓存的数量
        /// </summary>
        int CurrentCacheCount { get; }
        /// <summary>
        /// 对象池缓存数量上限
        /// </summary>
        int MaxCacheCount { get; set; }
        /// <summary>
        /// 从池中获取实例
        /// </summary>
        /// <returns>对象实例</returns>
        T Allocate();
        /// <summary>
        /// 回收对象
        /// </summary>
        /// <param name="t">回收对象实例</param>
        /// <returns>回收成功返回true 否则返回false</returns>
        bool Recycle(T t);
        /// <summary>
        /// 释放对象池
        /// </summary>
        void Release();
    }
}