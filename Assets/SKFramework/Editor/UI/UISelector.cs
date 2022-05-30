using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace SK.Framework
{
    /// <summary>
    /// Scene窗口UI元素选择工具
    /// </summary>
    [InitializeOnLoad]
    public static class UISelector
    {
        static UISelector()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            var ec = Event.current;

            if (ec != null && ec.button == 1 && ec.type == EventType.MouseUp)
            {
                ec.Use();
                // 当前屏幕坐标，左上角是（0，0）右下角（camera.pixelWidth，camera.pixelHeight）
                Vector2 mousePosition = Event.current.mousePosition;
                // Retina 屏幕需要拉伸值
                float mult = EditorGUIUtility.pixelsPerPoint;
                // 转换成摄像机可接受的屏幕坐标，左下角是（0，0，0）右上角是（camera.pixelWidth，camera.pixelHeight，0）
                mousePosition.y = sceneView.camera.pixelHeight - mousePosition.y * mult;
                mousePosition.x *= mult;

                var scenes = GetAllScenes();
                var groups = scenes
                    .Where(m => m.isLoaded)
                    .SelectMany(m => m.GetRootGameObjects())
                    .Where(m => m.activeInHierarchy)
                    .SelectMany(m => m.GetComponentsInChildren<RectTransform>())
                    .Where(m => RectTransformUtility.RectangleContainsScreenPoint(m, mousePosition, sceneView.camera))
                .GroupBy(m => m.gameObject.scene.name)
                .ToArray();
                var sceneCount = scenes.Count(m => m.isLoaded);
                var gc = new GenericMenu();
                var dic = new Dictionary<string, int>();
                foreach (var group in groups)
                {
                    foreach (var rt in group)
                    {
                        var name = rt.name;
                        var sceneName = rt.gameObject.scene.name;
                        var nameWithSceneName = sceneName + "/" + name;
                        var isContains = dic.ContainsKey(nameWithSceneName);
                        var text = sceneCount <= 1 ? name : nameWithSceneName;
                        if (isContains)
                        {
                            var count = dic[nameWithSceneName]++;
                            text += " [" + count.ToString() + "]";
                        }
                        var content = new GUIContent(text);
                        gc.AddItem(content, false, () =>
                        {
                            Selection.activeTransform = rt;
                            EditorGUIUtility.PingObject(rt.gameObject);
                        });
                        if (!isContains)
                        {
                            dic.Add(nameWithSceneName, 1);
                        }
                    }
                }
                gc.ShowAsContext();
            }
        }
        private static IEnumerable<Scene> GetAllScenes()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                yield return SceneManager.GetSceneAt(i);
            }
        }
    }
}