using UnityEditor;
using UnityEngine;
using System.Text;
using System.Collections.Generic;

namespace SK.Framework
{
    /// <summary>
    /// 材质复制机
    /// </summary>
    public class MaterialDuplicator : EditorWindow
    {
        [MenuItem("SKFramework/Duplicator/Material")]
        public static void Open()
        {
            var window = GetWindow<MaterialDuplicator>("Material Duplicator");
            window.minSize = new Vector2(300f, 60f);
            window.maxSize = new Vector2(300f, 60f);
            window.Show();
        }

        private Transform from;
        private Transform to;

        private void OnGUI()
        {
            //From
            GUILayout.BeginHorizontal();
            GUILayout.Label("From", GUILayout.Width(50f));
            from = EditorGUILayout.ObjectField(from, typeof(Transform), true) as Transform;
            GUILayout.EndHorizontal();

            //To
            GUILayout.BeginHorizontal();
            GUILayout.Label("To", GUILayout.Width(50f));
            to = EditorGUILayout.ObjectField(to, typeof(Transform), true) as Transform;
            GUILayout.EndHorizontal();

            GUI.enabled = from && to;
            if (GUILayout.Button("Duplicate"))
            {
                Dictionary<string, Renderer> fromMap = new Dictionary<string, Renderer>();
                Dictionary<string, Renderer> toMap = new Dictionary<string, Renderer>();

                if (!Validate(fromMap, toMap))
                {
                    Debug.Log(string.Format("复制失败，请检查{0}与{1}层级结构是否一致", from.name, to.name));
                    return;
                }
                foreach (var kv in toMap)
                {
                    kv.Value.sharedMaterials = fromMap[kv.Key].sharedMaterials;
                }
            }
        }

        //验证两者Hierarchy层级结构是否一致
        private bool Validate(Dictionary<string, Renderer> fromMap, Dictionary<string, Renderer> toMap)
        {
            ForEach(from, fromMap);
            ForEach(to, toMap);
            if (fromMap.Count != toMap.Count) return false;
            bool retV = true;
            foreach (var key in toMap.Keys)
            {
                if (!fromMap.ContainsKey(key))
                {
                    retV = false;
                    break;
                }
            }
            return retV;
        }

        //遍历层级结构
        private void ForEach(Transform t, Dictionary<string, Renderer> map)
        {
            Renderer[] renderers = t.GetComponentsInChildren<Renderer>(true);
            for (int i = 0; i < renderers.Length; i++)
            {
                Renderer renderer = renderers[i];
                map.Add(GetFullName(renderer.transform), renderer);
            }
        }

        //获取全路径
        private string GetFullName(Transform t)
        {
            List<Transform> tfs = new List<Transform>();
            Transform tf = t.transform;
            tfs.Add(tf);
            while (tf.parent)
            {
                tf = tf.parent;
                tfs.Add(tf);
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(tfs[tfs.Count - 2].name);
            for (int i = tfs.Count - 3; i >= 0; i--)
            {
                sb.Append("/" + tfs[i].name);
            }
            return sb.ToString();
        }
    }
}