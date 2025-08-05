/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
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

        public string[] dependencies;

        public List<AssetInfo> assets;

        public long memorySize;

        public string memorySizeFormat;

#if UNITY_EDITOR
        public AssetBundleInfo(string name)
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
            int index = assets.FindIndex(m => m.path == assetPath);
            if (index != -1)
            {
                var asset = assets[index];
                assets.RemoveAt(index);
                memorySize -= asset.memorySize;
                memorySizeFormat = EditorUtility.FormatBytes(memorySize);
            }
        }

        public void Rename(string newName)
        {
            name = AssetBundleUtility.GetUniqueAssetBundleNameRecursive(newName);
            for (int i = 0; i < assets.Count; i++)
            {
                AssetImporter importer = AssetImporter.GetAtPath(assets[i].path);
                if (importer != null)
                    importer.assetBundleName = name;
            }
        }
#endif
    }
}