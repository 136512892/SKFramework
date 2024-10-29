/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace SK.Framework.Resource
{
    [Serializable]
    public class AssetInfo
    {
        public string name;

        public string path;

        public string abName;

        public long memorySize;

        public string memorySizeFormat;

        public AssetInfo(string name, string path, string abName)
        {
            this.name = name;
            this.path = path;
            this.abName = abName;
        }

        public AssetInfo(string path)
        {
            this.path = path;
            string fullPath = Application.dataPath + path.Replace("Assets", "");
            memorySize = new FileInfo(fullPath).Length;
#if UNITY_EDITOR
            memorySizeFormat = EditorUtility.FormatBytes(memorySize);
#endif
        }

        public override string ToString()
        {
            return string.Format("AssetName:{0} AssetBundleName:{1} Path:{2} MemorySize:{3}", 
                name, abName, path, memorySize);
        }
    }
}