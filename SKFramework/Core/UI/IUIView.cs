namespace SK.Framework.UI
{
    /// <summary>
    /// UI视图接口
    /// </summary>
    public interface IUIView
    {
        /// <summary>
        /// 视图名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 是否已经打开
        /// </summary>
        bool IsOpened { get; }

        /// <summary>
        /// 初始化事件
        /// </summary>
        /// <param name="data">视图数据</param>
        void OnInit(object data);

        /// <summary>
        /// 打开事件
        /// </summary>
        /// <param name="instant">是否立即显示</param>
        /// <param name="data">视图数据</param>
        void OnOpen(bool instant, object data);
        
        /// <summary>
        /// 关闭事件
        /// </summary>
        /// <param name="instant">是否立即显示</param>
        void OnClose(bool instant);

        /// <summary>
        /// 卸载事件
        /// </summary>
        void OnUnload();
    }
}