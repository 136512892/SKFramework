using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace SK.Framework
{
    /// <summary>
    /// 蓝湖UI界面搭建工具
    /// </summary>
    public class LanHu : EditorWindow
    {
        [MenuItem("SKFramework/UI/LanHu")]
        public static void Open()
        {
            GetWindow<LanHu>("LanHu").Show();
        }

        private string path;
        private List<LanHuViewElement> elements;
        private Vector2 scroll;
        private const float labelWidth = 70f;
        private Dictionary<LanHuViewElement, bool> foldoutDic;
        private CanvasScaler canvasScaler;

        private void OnEnable()
        {
            path = "Assets";
            elements = new List<LanHuViewElement>();
            foldoutDic = new Dictionary<LanHuViewElement, bool>();
        }

        private void OnGUI()
        {
            OnTopGUI();
            OnElementsGUI();
            OnMenuGUI();
        }
        private void OnTopGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("切图文件夹路径：", GUILayout.Width(100f));
            EditorGUILayout.TextField(path);
            if (GUILayout.Button("浏览", GUILayout.Width(40f)))
            {
                //Assets相对路径
                path = EditorUtility.OpenFolderPanel("选择切图文件夹", "", "").Replace(Application.dataPath, "Assets");
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Canvas Scaler", GUILayout.Width(100f));
            canvasScaler = (CanvasScaler)EditorGUILayout.ObjectField(canvasScaler, typeof(CanvasScaler), true);
            if (canvasScaler == null)
            {
                if (GUILayout.Button("创建", GUILayout.Width(40f)))
                {
                    var canvas = new GameObject("Canvas").AddComponent<Canvas>();
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    canvasScaler = canvas.gameObject.AddComponent<CanvasScaler>();
                    canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
                    EditorGUIUtility.PingObject(canvas);
                }
            }
            GUILayout.EndHorizontal();
        }
        private void OnElementsGUI()
        {
            EditorGUILayout.Space();

            GUI.enabled = canvasScaler != null;
            scroll = EditorGUILayout.BeginScrollView(scroll);
            for (int i = 0; i < elements.Count; i++)
            {
                var element = elements[i];
                if (!foldoutDic.ContainsKey(element))
                {
                    foldoutDic.Add(element, true);
                }

                foldoutDic[element] = EditorGUILayout.Foldout(foldoutDic[element], element.name, true);

                if (!foldoutDic[element]) continue;

                GUILayout.BeginVertical("Box");

                GUILayout.BeginHorizontal();
                GUILayout.Label("图层", GUILayout.Width(labelWidth));
                element.name = EditorGUILayout.TextField(element.name);
                if (GUILayout.Button("粘贴", GUILayout.Width(40f)))
                {
                    element.name = GUIUtility.systemCopyBuffer;
                }
                if (GUILayout.Button("-", GUILayout.Width(20f)))
                {
                    foldoutDic.Remove(element);
                    elements.RemoveAt(i);
                    Repaint();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("位置", GUILayout.Width(labelWidth));
                element.x = EditorGUILayout.TextField(element.x);
                if (GUILayout.Button("粘贴", GUILayout.Width(40f)))
                {
                    element.x = GUIUtility.systemCopyBuffer;
                }
                element.y = EditorGUILayout.TextField(element.y);
                if (GUILayout.Button("粘贴", GUILayout.Width(40f)))
                {
                    element.y = GUIUtility.systemCopyBuffer;
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("大小", GUILayout.Width(labelWidth));
                element.width = EditorGUILayout.TextField(element.width);
                if (GUILayout.Button("粘贴", GUILayout.Width(40f)))
                {
                    element.width = GUIUtility.systemCopyBuffer;
                }
                element.height = EditorGUILayout.TextField(element.height);
                if (GUILayout.Button("粘贴", GUILayout.Width(40f)))
                {
                    element.height = GUIUtility.systemCopyBuffer;
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("不透明度", GUILayout.Width(labelWidth));
                element.opacity = EditorGUILayout.TextField(element.opacity);
                if (GUILayout.Button("粘贴", GUILayout.Width(40f)))
                {
                    element.opacity = GUIUtility.systemCopyBuffer;
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("像素倍数", GUILayout.Width(labelWidth));
                if (GUILayout.Button(element.pixel))
                {
                    GenericMenu gm = new GenericMenu();
                    gm.AddItem(new GUIContent("x1"), element.pixel == "x1", () => element.pixel = "x1");
                    gm.AddItem(new GUIContent("x2"), element.pixel == "x2", () => element.pixel = "x2");
                    gm.ShowAsContext();
                }
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();
        }
        private void OnMenuGUI()
        {
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("导入", "ButtonLeft"))
            {
                string presetPath = EditorUtility.OpenFilePanel("选择预设文件", Application.dataPath, "asset");
                if (File.Exists(presetPath))
                {
                    var import = AssetDatabase.LoadAssetAtPath<LanHuView>(presetPath.Replace(Application.dataPath, "Assets"));
                    if (import != null)
                    {
                        elements.Clear();
                        foldoutDic.Clear();

                        path = import.path;
                        for (int i = 0; i < import.elements.Count; i++)
                        {
                            elements.Add(import.elements[i]);
                        }
                        Repaint();
                    }
                }
            }
            if (GUILayout.Button("添加", "ButtonMid"))
            {
                elements.Add(new LanHuViewElement("", "0px", "0px", "1920px", "1080px", "100%", "x1"));
            }
            if (GUILayout.Button("清空", "ButtonMid"))
            {
                if (EditorUtility.DisplayDialog("提醒", "确定删除当前所有配置信息?", "确定", "取消"))
                {
                    elements.Clear();
                    foldoutDic.Clear();
                }
            }
            if (GUILayout.Button("展开", "ButtonMid"))
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    foldoutDic[elements[i]] = true;
                }
            }
            if (GUILayout.Button("收缩", "ButtonMid"))
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    foldoutDic[elements[i]] = false;
                }
            }
            if (GUILayout.Button("生成", "ButtonRight"))
            {
                if (!AssetDatabase.IsValidFolder(path)) return;
                var array = path.Split('/');
                var view = new GameObject(array[array.Length - 1]).AddComponent<RectTransform>();
                view.transform.SetParent(canvasScaler.transform, false);
                SetRectTransform(view, 0, 0, canvasScaler.referenceResolution.x, canvasScaler.referenceResolution.y);

                for (int i = 0; i < elements.Count; i++)
                {
                    var element = elements[i];
                    string spritePath = string.Format("{0}/{1}{2}.png", path, element.name, element.pixel == "x1" ? string.Empty : "@2x");
                    var obj = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
                    if (obj != null)
                    {
                        var image = new GameObject(obj.name).AddComponent<Image>();
                        image.transform.SetParent(view.transform, false);
                        image.sprite = obj;
                        RectTransform rt = image.transform as RectTransform;

                        float.TryParse(element.x.Replace(element.x.Substring(element.x.Length - 2, 2), string.Empty), out float xValue);
                        float.TryParse(element.y.Replace(element.y.Substring(element.y.Length - 2, 2), string.Empty), out float yValue);
                        float.TryParse(element.width.Replace(element.width.Substring(element.width.Length - 2, 2), string.Empty), out float wValue);
                        float.TryParse(element.height.Replace(element.height.Substring(element.height.Length - 2, 2), string.Empty), out float hValue);
                        /*
                        float.TryParse(element.x, out float xValue);
                        float.TryParse(element.y, out float yValue);
                        float.TryParse(element.width, out float wValue);
                        float.TryParse(element.height, out float hValue);
                        */
                        SetRectTransform(rt, xValue, yValue, wValue, hValue);
                    }
                    else
                    {
                        Debug.Log($"<color=yellow>加载切图失败 {spritePath}</color>");
                    }
                }

                //创建预设文件
                var preset = CreateInstance<LanHuView>();
                for (int i = 0; i < elements.Count; i++)
                {
                    preset.elements.Add(elements[i]);
                }
                preset.path = path;
                AssetDatabase.CreateAsset(preset, string.Format("Assets/{0}.asset", view.name));
                AssetDatabase.Refresh();
                Selection.activeObject = preset;

                //创建Prefab
                var prefab = PrefabUtility.SaveAsPrefabAsset(view.gameObject, $"Assets/{view.name}.prefab", out bool result);
                if (!result)
                {
                    Debug.Log($"<color=yellow>生成预制体失败 {view.name}</color>");
                }
                else
                {
                    EditorGUIUtility.PingObject(prefab);
                }
            }
            GUILayout.EndHorizontal();
        }

        private void SetRectTransform(RectTransform rt, float x, float y, float width, float height)
        {
            //调整位置及大小
            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(0, 1);
            rt.pivot = Vector2.one * .5f;
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            rt.anchoredPosition = new Vector2(x + width / 2f, -(y + height / 2f));

            //调整完成后自动设置锚点
            RectTransform prt = rt.parent as RectTransform;
            Vector2 anchorMin = new Vector2(
                rt.anchorMin.x + rt.offsetMin.x / prt.rect.width,
                rt.anchorMin.y + rt.offsetMin.y / prt.rect.height);
            Vector2 anchorMax = new Vector2(
                rt.anchorMax.x + rt.offsetMax.x / prt.rect.width,
                rt.anchorMax.y + rt.offsetMax.y / prt.rect.height);
            rt.anchorMin = anchorMin;
            rt.anchorMax = anchorMax;
            rt.offsetMin = rt.offsetMax = Vector2.zero;
        }
    }
}