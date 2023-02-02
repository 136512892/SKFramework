namespace SK.Framework.Resource
{
    public abstract class ResourceInfo
    {  
        /// <summary>
        /// Assets路径
        /// </summary>
        public string AssetPath { get; private set; }
        /// <summary>
        /// AssetBundle名称
        /// </summary>
        public string AssetBundleName { get; private set; }
  
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="assetBundleName">AssetBundle名称</param>
        /// <param name="assetPath">Asset路径</param>
        public ResourceInfo(string assetBundleName, string assetPath)
        {
            AssetBundleName = assetBundleName;
            AssetPath = assetPath;
        }
    }
}