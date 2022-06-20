namespace SK.Framework
{
    /// <summary>
    /// 计时工具接口
    /// </summary>
    public interface ITimer 
    {
        /// <summary>
        /// 是否计时完成
        /// </summary>
        bool IsCompleted { get; }
        /// <summary>
        /// 是否暂停
        /// </summary>
        bool IsPaused { get; }
        /// <summary>
        /// 启动计时
        /// </summary>
        void Launch();
        /// <summary>
        /// 暂停
        /// </summary>
        void Pause();
        /// <summary>
        /// 恢复/继续
        /// </summary>
        void Resume();
        /// <summary>
        /// 停止
        /// </summary>
        void Stop();
        /// <summary>
        /// 计时
        /// </summary>
        /// <returns>计时完成返回true 否则返回false</returns>
        bool Execute();
    }
}