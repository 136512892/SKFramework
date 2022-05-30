using System;

namespace SK.Framework
{
    /// <summary>
    /// 网络接口
    /// </summary>
    [Serializable]
    public class WebInterface
    {
        /// <summary>
        /// 接口名称
        /// </summary>
        public string name;
        /// <summary>
        /// 接口地址
        /// </summary>
        public string url;
        /// <summary>
        /// 请求方式
        /// </summary>
        public WebRequestMethod method;
        /// <summary>
        /// 接口参数
        /// </summary>
        public string[] args;
    }
}