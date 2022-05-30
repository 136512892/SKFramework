using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace SK.Framework
{
    /// <summary>
    /// 资产引用
    /// </summary>
    public class AssetsReference
    {
        /// <summary>
        /// 获取资产的引用项
        /// </summary>
        [MenuItem("SKFramework/Assets/Get References")]
        public static void GetReferences()
        {
            var map = GetDependenciesMap();
            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            string[] reference = map.Where(m => m.Value.Contains(assetPath)).Select(m => m.Key).ToArray();
            List<Object> objects = new List<Object>();
            for (int i = 0; i < reference.Length; i++)
            {
                objects.Add(AssetDatabase.LoadMainAssetAtPath(reference[i]));
                EditorUtility.DisplayProgressBar("获取资产引用项", reference[i], (float)i + 1 / reference.Length);
            }
            EditorUtility.ClearProgressBar();
            Selection.objects = objects.ToArray();
        }
        [MenuItem("SKFramework/Assets/Get References", true)]
        public static bool GetReferencesValidate()
        {
            return Selection.objects.Length == 1;
        }

        private static Dictionary<string, string[]> GetDependenciesMap()
        {
            //存放资源的依赖关系
            Dictionary<string, string[]> map = new Dictionary<string, string[]>();
            //获取所有资产路径
            string[] paths = AssetDatabase.GetAllAssetPaths();
            //遍历 建立资产间的依赖关系
            for (int i = 0; i < paths.Length; i++)
            {
                string path = paths[i];
                //根据资产路径获取该资产的依赖项
                var dependencies = AssetDatabase.GetDependencies(path).ToList();
                //获取依赖项时会包含该资产本身 将本身移除
                dependencies.Remove(path);
                //加入字典
                if (dependencies.Count > 0)
                {
                    map.Add(path, dependencies.ToArray());
                }
                //进度条   
                EditorUtility.DisplayProgressBar("获取资产依赖关系", path, (float)i + 1 / paths.Length);
            }
            EditorUtility.ClearProgressBar();
            return map;
        }

        /// <summary>
        /// 获取没有任何引用的资产 即无用资产
        /// </summary>
        [MenuItem("SKFramework/Assets/Get NoReference Assets")]
        public static void GetNoReferenceAssets()
        {
            var map = GetDependenciesMap();
            var paths = AssetDatabase.GetAllAssetPaths().ToList();
            for (int i = 0; i < paths.Count; i++)
            {
                string path = paths[i];
                foreach (string[] array in map.Values)
                {
                    if (array.Contains(path))
                    {
                        paths.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }
            List<Object> objects = new List<Object>();
            for (int i = 0; i < paths.Count; i++)
            {
                objects.Add(AssetDatabase.LoadMainAssetAtPath(paths[i]));
                EditorUtility.DisplayProgressBar("获取无用资产", paths[i], (float)i + 1 / paths.Count);
            }
            EditorUtility.ClearProgressBar();
            Selection.objects = objects.ToArray();
        }
    }
}