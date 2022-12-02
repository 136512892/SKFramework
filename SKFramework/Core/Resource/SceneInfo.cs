namespace SK.Framework.Resource
{
    public class SceneInfo : ResourceInfo
    {
        /// <summary>
        /// 场景名称
        /// </summary>
        public readonly string sceneName;

        public SceneInfo(string assetBundleName, string assetPath) : base(assetBundleName, assetPath)
        {
            int startIndex = assetPath.LastIndexOf('/') + 1;
            int endIndex = assetPath.LastIndexOf('.');
            sceneName = assetPath.Substring(startIndex, endIndex - startIndex);
        }
    }
}