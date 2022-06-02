using System;

namespace SK.Framework
{
    /// <summary>
    /// 资源包简要信息
    /// </summary>
    [Serializable]
    public class PackageInfoBrief 
    {
        /// <summary>
        /// 资源包名称
        /// </summary>
        public string name;
        /// <summary>
        /// 资源包版本
        /// </summary>
        public string version;
        /// <summary>
        /// 是否已经安装
        /// </summary>
        public bool isInstalled;

        public override string ToString()
        {
            return string.Format("{0}-{1}", name, version);
        }
    }
}