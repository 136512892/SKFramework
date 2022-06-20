namespace SK.Framework
{
    /// <summary>
    /// 可瞄准物体接口
    /// </summary>
    public interface IAimableObject
    {
        /// <summary>
        /// 物体描述
        /// </summary>
        string Description { get; }
        /// <summary>
        /// 可检测距离
        /// </summary>
        float AimableDistance { get; }
        /// <summary>
        /// 瞄准进入
        /// </summary>
        void Enter();
        /// <summary>
        /// 瞄准退出
        /// </summary>
        void Exit();
        /// <summary>
        /// 瞄准停留
        /// </summary>
        void Stay();
    }
}