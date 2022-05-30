using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace SK.Framework
{
    /// <summary>
    /// Mesh网格合并
    /// </summary>
    public class MeshCombiner
    {
        [MenuItem("SKFramework/Mesh/Combine")]
        public static void Execute()
        {
            bool mergeSubMeshes = EditorUtility.DisplayDialog("Mesh Combine", 
                "是否将所有网格合并到单个子网格中？(如果所有网格都共享相同的材质，则应选择是，反之应选择否)", "是", "否");
            List<CombineInstance> instances = new List<CombineInstance>();
            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                GameObject go = Selection.gameObjects[i];
                if (go == null) continue;
                MeshFilter meshFilter = go.GetComponent<MeshFilter>();
                if (meshFilter == null) continue;
                Mesh target = meshFilter.sharedMesh;
                if (target == null) continue;
                EditorUtility.DisplayProgressBar("网格合并", go.name, (float)i + 1 / Selection.gameObjects.Length);
                instances.Add(new CombineInstance { mesh = target, transform = meshFilter.transform.localToWorldMatrix });
            }
            EditorUtility.ClearProgressBar();

            var mrs = Selection.gameObjects.Select(m => (m as GameObject).GetComponent<MeshRenderer>()).ToArray();
            List<Material> materials;
            if (mergeSubMeshes)
            {
                materials = mrs.First().sharedMaterials.ToList();
            }
            else
            {
                materials = new List<Material>();
                for (int i = 0; i < mrs.Length; i++)
                {
                    materials.AddRange(mrs[i].sharedMaterials);
                }
            }
            GameObject instance = new GameObject("New Mesh Combined");
            MeshFilter filter = instance.AddComponent<MeshFilter>();
            filter.mesh = new Mesh { name = instance.name };
            filter.sharedMesh.CombineMeshes(instances.ToArray(), mergeSubMeshes);
            MeshRenderer renderer = instance.AddComponent<MeshRenderer>();
            renderer.sharedMaterials = materials.ToArray();
        }

        [MenuItem("SKFramework/Mesh/Combine", true)]
        public static bool Validate()
        {
            return Selection.gameObjects.Length >= 2;
        }
    }
}