using System;

namespace SK.Framework.Resource
{
    /// <summary>
    /// Asset资产信息
    /// </summary>
    [Serializable]
    public class AssetInfo
    {
        /// <summary>
        /// Asset资产名称
        /// </summary>
        public string name;

        /// <summary>
        /// 资源路径
        /// </summary>
        public string path;

        /// <summary>
        /// AssetBundle包名称
        /// </summary>
        public string abName;

        public override string ToString()
        {
            return string.Format("AssetName:{0}  AssetBundleName:{1}  Path:{2}", name, abName, path);
        }
    }
}