/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Linq;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SK.Framework.Resource
{
    [Serializable]
    public class AssetBundleInfo
    {
        public string name;
        public string md5;
        public long size;
        public string[] tags;

        public AssetBundleInfo() { }

        public AssetBundleInfo(string name, string md5, long size, string[] tags)
        {
            this.name = name;
            this.md5 = md5;
            this.size = size;
            this.tags = tags;
        }
    }

#if UNITY_EDITOR
    [Serializable]
    public class AssetBundleEditorInfo
    {
        public string name;

        public string[] dependencies;

        public List<AssetInfo> assets;

        public long memorySize;

        public string memorySizeFormat;

        public AssetBundleEditorInfo(string name)
        {
            this.name = name;
            dependencies = AssetDatabase.GetAssetBundleDependencies(name, true);
            string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(name);
            assets = new List<AssetInfo>(assetPaths.Length);
            for (int i = 0; i < assetPaths.Length; i++)
            {
                assets.Add(new AssetInfo(assetPaths[i]));
            }
            memorySize = assets.Sum(m => m.memorySize);
            memorySizeFormat = EditorUtility.FormatBytes(memorySize);
        }

        public void AddAsset(string assetPath)
        {
            int index = assets.FindIndex(m => m.path == assetPath);
            if (index == -1)
            {
                var asset = new AssetInfo(assetPath);
                assets.Add(asset);
                memorySize += asset.memorySize;
                memorySizeFormat = EditorUtility.FormatBytes(memorySize);
            }
        }

        public void DeleteAsset(string assetPath)
        {
            var index = assets.FindIndex(m =>m.path == assetPath);
            if (index != -1)
            {
                var asset = assets[index];
                assets.Remove(asset);
                memorySize -= asset.memorySize;
                memorySizeFormat = EditorUtility.FormatBytes(memorySize);
            }
        }
    }
#endif
}