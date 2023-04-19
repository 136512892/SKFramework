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
        string Name { get; }

        /// <summary>
        /// 被引用的数量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 申请引用
        /// </summary>
        void Apply();

        /// <summary>
        /// 释放引用
        /// </summary>
        void Release();
    }
}