/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

#if UNITY_EDITOR
using System;
using System.Text.RegularExpressions;

using UnityEditor;
using Object = UnityEngine.Object;

namespace SK.Framework.Resource
{
    public class AssetBundleUtility
    {
        /**************************************************************************************
         * 递归方式获取一个唯一的AssetBundle名称
         * 检查当前若存在相同名称的AssetBundle，方法将会将数字1附加到名称后再次检查
         * 持续递增数字再次检查直到名称是唯一的
         **************************************************************************************/
        public static string GetUniqueAssetBundleNameRecursive(string assetBundleName)
        {
            string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames();
            int index = Array.FindIndex(assetBundleNames, m => m == assetBundleName.ToLower());
            if (index != -1)
            {
                string target = assetBundleNames[index];
                string numberStr = Regex.Match(target, @"\d+$").Value;
                if (!string.IsNullOrEmpty(numberStr))
                {
                    int.TryParse(numberStr, out int number);
                    int subLength = target.Length - numberStr.Length;
                    string newName = target.Substring(target.Length - subLength - 1, subLength) + (++number);
                    return GetUniqueAssetBundleNameRecursive(newName);
                }
                else return GetUniqueAssetBundleNameRecursive(assetBundleName + 1);
            }
            else return assetBundleName;
        }

        public static bool CreateAssetBundle4Object(Object obj)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            AssetImporter importer = AssetImporter.GetAtPath(path);
            if (string.IsNullOrEmpty(importer.assetBundleName) || EditorUtility.DisplayDialog(
                "提醒",
                string.Format("资产 {0} 已经位于 {1} AssetBundle中，是否重新为其配置AssetBundle？",
                    obj.name, importer.assetBundleName),
                "确认",
                "取消"))
            {
                importer.assetBundleName = GetUniqueAssetBundleNameRecursive(obj.name);
                return true;
            };
            return false;
        }

        public static bool DeleteAssetBundle(string assetBundleName, bool dialogue = true)
        {
            string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
            if (assetPaths.Length > 0)
            {
                if (!dialogue || EditorUtility.DisplayDialog(
                    "提醒",
                    string.Format("是否确认删除 {0} AssetBundle？", assetBundleName),
                    "确认",
                    "取消"))
                {
                    for (int i = 0; i < assetPaths.Length; i++)
                    {
                        AssetImporter importer = AssetImporter.GetAtPath(assetPaths[i]);
                        if (importer != null)
                            importer.assetBundleName = null;
                    }
                    AssetDatabase.RemoveAssetBundleName(assetBundleName, true);
                    return true;
                }
                return false;
            }
            AssetDatabase.RemoveAssetBundleName(assetBundleName, true);
            return true;
        }
    }
}
#endif