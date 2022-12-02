namespace SK.Framework.Resource
{
    public abstract class ResourceInfo
    {  
        /// <summary>
        /// Assets路径
        /// </summary>
        public readonly string assetPath;
        /// <summary>
        /// AssetBundle名称
        /// </summary>
        public readonly string assetBundleName;
  
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="assetBundleName">AssetBundle名称</param>
        /// <param name="assetPath">Asset路径</param>
        public ResourceInfo(string assetBundleName, string assetPath)
        {
            this.assetBundleName = assetBundleName;
            this.assetPath = assetPath;
        }
    }
}