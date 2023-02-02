namespace SK.Framework.Resource
{
    public class AssetInfo : ResourceInfo
    {
        /// <summary>
        /// 资源名称
        /// </summary>
        public string AssetName { get; private set; }

        public AssetInfo(string assetBundleName, string assetPath) : base(assetBundleName, assetPath)
        {
            int startIndex = assetPath.LastIndexOf('/') + 1;
            int endIndex = assetPath.LastIndexOf('.');
            AssetName = assetPath.Substring(startIndex, endIndex - startIndex);
        }
    }
}