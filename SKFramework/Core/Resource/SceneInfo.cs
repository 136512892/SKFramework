namespace SK.Framework.Resource
{
    public class SceneInfo : ResourceInfo
    {
        /// <summary>
        /// 场景名称
        /// </summary>
        public string SceneName { get; private set; }

        public SceneInfo(string assetBundleName, string assetPath) : base(assetBundleName, assetPath)
        {
            int startIndex = assetPath.LastIndexOf('/') + 1;
            int endIndex = assetPath.LastIndexOf('.');
            SceneName = assetPath.Substring(startIndex, endIndex - startIndex);
        }
    }
}