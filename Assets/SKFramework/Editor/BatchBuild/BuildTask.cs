using System;
using UnityEditor;
using System.Collections.Generic;

namespace SK.Framework
{
    /// <summary>
    /// 打包工作项
    /// </summary>
    [Serializable]
    public class BuildTask
    {
        /// <summary>
        /// 打包的产品名称
        /// </summary>
        public string productName;
        /// <summary>
        /// 打包的目标平台
        /// </summary>
        public BuildTarget buildTarget;
        /// <summary>
        /// 打包的保存路径
        /// </summary>
        public string buildPath;
        /// <summary>
        /// 打包的场景列表
        /// </summary>
        public List<SceneAsset> sceneAssets;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="productName">产品名称</param>
        /// <param name="buildTarget">目标平台</param>
        /// <param name="buildPath">保存路径</param>
        public BuildTask(string productName, BuildTarget buildTarget, string buildPath)
        {
            this.productName = productName;
            this.buildTarget = buildTarget;
            this.buildPath = buildPath;
            sceneAssets = new List<SceneAsset>();
        }
    }
}