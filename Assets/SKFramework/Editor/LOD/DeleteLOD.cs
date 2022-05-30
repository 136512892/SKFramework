using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SK.Framework
{
    /// <summary>
    /// 批量删除场景中的LOD Group组件
    /// 删除后只保留最高层次细节的物体
    /// </summary>
    public class DeleteLOD
    {
        [MenuItem("SKFramework/LOD/Delete")]
        public static void Execute()
        {
            //弹出进行删除操作的提醒窗口
            bool confirm = EditorUtility.DisplayDialog("提醒", "是否确定删除当前场景中所有的LOD Group组件，只保留最高层次的细节", "确定", "取消");
            //点击取消则return
            if (!confirm) return;
            //弹出窗口 询问是否执行Prefab Unpack操作
            bool unpackPrefab = EditorUtility.DisplayDialog("确认", "如果目标物体属于Prefab预制体，执行Prefab Unpack操作还是跳过它？", "Unpack", "Skip");
            //获取当前场景中的所有根物体
            GameObject[] rootObjs = SceneManager.GetActiveScene().GetRootGameObjects();
            //遍历所有根物体
            for (int i = 0; i < rootObjs.Length; i++)
            {
                GameObject rootObj = rootObjs[i];
                //在根物体及子物体身上查找所有LOD Group组件
                LODGroup[] groups = rootObj.GetComponentsInChildren<LODGroup>(true);
                //遍历所有LOD Group组件
                for (int j = 0; j < groups.Length; j++)
                {
                    LODGroup group = groups[j];
                    GameObject obj = group.gameObject;
                    //获取LOD Group中的所有LOD结构
                    LOD[] lods = group.GetLODs();
                    if (lods.Length < 2) continue;
                    //遍历LOD结构 索引从1开始 保留最高层次的细节
                    for (int l = 1; l < lods.Length; l++)
                    {
                        LOD lod = lods[l];
                        //遍历LOD中的Renderer组件
                        for (int n = 0; n < lod.renderers.Length; n++)
                        {
                            Renderer renderer = lod.renderers[n];
                            if (renderer == null) continue;
                            //如果Renderer组件不为空 获取其挂载的物体
                            GameObject target = renderer.gameObject;
                            //物体不为空，进行销毁
                            if (target != null)
                            {
                                //如果目标物体属于Prefab预制体
                                if (PrefabUtility.IsPartOfAnyPrefab(target))
                                {
                                    if (unpackPrefab)
                                    {
                                        //获取Prefab预制体根级
                                        var prefabRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(target);
                                        //Prefab Unpack Completely
                                        PrefabUtility.UnpackPrefabInstance(prefabRoot, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
                                    }
                                    else continue;
                                }
                                //弹出进度条
                                EditorUtility.DisplayProgressBar("LOD deleting...", $"{rootObj.name}/{obj.name}/{target.name}", (i + 1) / rootObjs.Length);
                                Object.DestroyImmediate(target);
                            }
                        }
                    }
                    Debug.Log($"删除{group.name}的LOD Group组件.");
                    //销毁LOD Group组件
                    Object.DestroyImmediate(group);
                    EditorUtility.SetDirty(obj);
                }
            }
            //清除进度条
            EditorUtility.ClearProgressBar();
        }
    }
}