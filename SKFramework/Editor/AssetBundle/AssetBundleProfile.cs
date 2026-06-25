/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace SK.Framework
{
    public class AssetBundleProfile : ScriptableObject
    {
        public List<AssetBundleEntry> entries = new List<AssetBundleEntry>();

        public AssetBundleEntry GetEntry(string bundleName)
        {
            return entries.Find(m => m.bundleName == bundleName);
        }

        public AssetBundleEntry GetOrCreateEntry(string bundleName)
        {
            var entry = GetEntry(bundleName);
            if (entry == null)
            {
                entry = new AssetBundleEntry { bundleName = bundleName };
                entries.Add(entry);
                EditorUtility.SetDirty(this);
            }
            return entry;
        }

        public void RenameEntry(string oldName, string newName)
        {
            var entry = GetEntry(oldName);
            if (entry != null)
            {
                entry.bundleName = newName;
                EditorUtility.SetDirty(this);
            }
        }

        public List<string> GetTags(string bundleName)
        {
            return GetEntry(bundleName)?.tags ?? new List<string>();
        }

        public void SetTags(string bundleName, List<string> tags)
        {
            GetOrCreateEntry(bundleName).tags = new List<string>(tags);
            EditorUtility.SetDirty(this);
        }
    }

    [Serializable]
    public class AssetBundleEntry
    {
        public string bundleName;
        public List<string> assetPaths = new List<string>(0);
        public List<string> tags = new List<string>(0);
    }
}