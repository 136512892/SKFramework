using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace SK.Framework
{
    /// <summary>
    /// 资产依赖
    /// </summary>
    public class AssetsDependencies
    {
        /// <summary>
        /// 获取资产的依赖项
        /// </summary>
        [MenuItem("SKFramework/Assets/Get Dependencies")]
        public static void GetDependencies()
        {
            //获取资产路径
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            //根据路径获取资产依赖项
            var dependencies = AssetDatabase.GetDependencies(path).ToList();
            //获取依赖项时会包含该资产本身 将本身移除
            dependencies.Remove(path);
            List<Object> objects = new List<Object>();
            for (int i = 0; i < dependencies.Count; i++)
            {
                objects.Add(AssetDatabase.LoadMainAssetAtPath(dependencies[i]));
                EditorUtility.DisplayProgressBar("获取资产依赖项", dependencies[i], (float)i + 1 / dependencies.Count);
            }
            EditorUtility.ClearProgressBar();
            Selection.objects = objects.ToArray();
        }

        [MenuItem("SKFramework/Assets/Get Dependencies", true)]
        public static bool GetDependenciesValidate()
        {
            return Selection.objects.Length == 1;
        }
    }
}