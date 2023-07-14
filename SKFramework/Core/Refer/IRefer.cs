namespace SK.Framework.Refer
{
    /// <summary>
    /// 引用接口
    /// </summary>
    public interface IRefer
    {
        /// <summary>
        /// 引用名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 被引用的数量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 申请引用
        /// </summary>
        /// <param name="applicant">申请者</param>
        void Apply(object applicant);

        /// <summary>
        /// 释放引用
        /// </summary>
        /// <param name="applicant">申请者</param>
        void Release(object applicant);

        /// <summary>
        /// 初始化事件
        /// </summary>
        void OnInitialization();

        /// <summary>
        /// 刷新事件
        /// </summary>
        void OnUpdate();

        /// <summary>
        /// 销毁事件
        /// </summary>
        void OnDestroy();
    }
}