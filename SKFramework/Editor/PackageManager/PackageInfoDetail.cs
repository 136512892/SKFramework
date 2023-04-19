using System;

namespace SK.Framework
{
    /// <summary>
    /// 资源包详细信息
    /// </summary>
    [Serializable]
    public class PackageInfoDetail : PackageInfoBrief
    {
        /// <summary>
        /// 作者
        /// </summary>
        public string author;
        /// <summary>
        /// 介绍
        /// </summary>
        public string description;
        /// <summary>
        /// 文档地址
        /// </summary>
        public string documentationUrl;
        /// <summary>
        /// 发布日期
        /// </summary>
        public string releaseDate;
        /// <summary>
        /// 依赖项
        /// </summary>
        public PackageInfoBrief[] dependencies;
        /// <summary>
        /// 被引用
        /// </summary>
        public PackageInfoBrief[] referencies;
    }
}