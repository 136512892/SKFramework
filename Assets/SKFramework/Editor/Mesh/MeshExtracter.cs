using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework
{
    /// <summary>
    /// Mesh网格提取器
    /// </summary>
    public class MeshExtracter
    {
        [MenuItem("SKFramework/Mesh/Extract")]
        public static void Execute()
        {
            string folder = EditorUtility.OpenFolderPanel("选择提取路径", Application.dataPath, null);
            if (string.IsNullOrEmpty(folder)) return;
            folder = folder.Replace(Application.dataPath, "Assets");
            if (!AssetDatabase.IsValidFolder(folder))
            {
                Debug.Log("无效路径");
                return;
            }
            List<Mesh> extracted = new List<Mesh>();
            for (int i = 0; i < Selection.objects.Length; i++)
            {
                GameObject go = Selection.objects[i] as GameObject;
                if (go == null) continue;
                MeshFilter meshFilter = go.GetComponent<MeshFilter>();
                if (meshFilter == null) continue;
                Mesh target = meshFilter.sharedMesh;
                if (target == null) continue;
                EditorUtility.DisplayProgressBar("网格提取", go.name, (float)i + 1/ Selection.objects.Length);
                try
                {
                    Mesh mesh = UnityEngine.Object.Instantiate(target);
                    AssetDatabase.CreateAsset(mesh, string.Format("{0}/{1}-{2}.asset", folder, go.name, target.name));
                    AssetDatabase.Refresh();
                    extracted.Add(mesh);
                    Debug.Log(string.Format("成功提取{0}的Mesh网格{1}至{2}目录下", go.name, target.name, folder));
                }
                catch (Exception error)
                {
                    Debug.Log(string.Format("{0}提取Mesh网格失败: {1}", go.name, error));
                }
            }
            EditorUtility.ClearProgressBar();
            Selection.objects = extracted.ToArray();
        }

        [MenuItem("SKFramework/Mesh/Extract", true)]
        public static bool Validate()
        {
            return Selection.objects.Length != 0;
        }
    }
}