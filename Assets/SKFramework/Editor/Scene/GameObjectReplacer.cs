using UnityEditor;
using UnityEngine;

namespace SK.Framework
{
    /// <summary>
    /// 物体替换器
    /// </summary>
    public class GameObjectReplacer : EditorWindow
    {
        [MenuItem("SKFramework/Scene/GameObject Replacer")]
        public static void Open()
        {
            var window = GetWindow<GameObjectReplacer>("GameObject Replacer");
            window.maxSize = new Vector2(300f, 100f);
            window.minSize = new Vector2(300f, 100f);
            window.Show();
        }

        private GameObject target;

        private void OnGUI()
        {
            EditorGUILayout.HelpBox("将使用Target替换所有选中的物体，并保留原有物体的Position、Rotation及Scale信息", MessageType.Info);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Target：", GUILayout.Width(100f));
            target = EditorGUILayout.ObjectField(target, typeof(GameObject), true) as GameObject;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            int count = Selection.gameObjects.Length;
            GUILayout.Label(string.Format("Selected Count： {0}", count));
            GUILayout.EndHorizontal();

            GUI.enabled = target != null && Selection.gameObjects.Length > 0;
            if (GUILayout.Button("Replace"))
            {
                if (EditorUtility.DisplayDialog("提醒", string.Format("是否确认使用{0}替换所有选中的物体？", target.name), "确认", "取消"))
                {
                    for (int i = 0; i < Selection.gameObjects.Length; i++)
                    {
                        var go = Selection.gameObjects[i];
                        var instance = Instantiate(target);
                        instance.transform.position = go.transform.position;
                        instance.transform.rotation = go.transform.rotation;
                        instance.transform.localScale = go.transform.localScale;
                        instance.transform.SetParent(go.transform.parent);
                        DestroyImmediate(go.gameObject);
                        i--;
                    }
                    target = null;
                }
            }
        }

        private void OnSelectionChange()
        {
            Repaint();
        }
    }
}