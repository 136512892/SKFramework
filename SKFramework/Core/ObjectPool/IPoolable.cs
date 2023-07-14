namespace SK.Framework.ObjectPool
{
    public interface IPoolable
    {
        /// <summary>
        /// 是否已经回收
        /// </summary>
        bool IsRecycled { get; set; }

        /// <summary>
        /// 回收事件
        /// </summary>
        void OnRecycled();
    } 
}