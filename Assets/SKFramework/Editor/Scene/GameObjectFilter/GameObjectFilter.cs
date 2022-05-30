using System;
using UnityEditor;
using System.Linq;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace SK.Framework
{
    /// <summary>
    /// Hierarchy窗口物体筛选工具
    /// </summary>
    public class GameObjectFilter : EditorWindow
    {
        [MenuItem("SKFramework/Scene/GameObject Filter")]
        public static void Open()
        {
            GetWindow<GameObjectFilter>("GameObject Filter").Show();
        }
        //过滤的目标 默认为Hierarchy层级中的所有物体
        private GameObjectFilterTarget target = GameObjectFilterTarget.All;
        //存储当前所用的过滤条件
        private List<GameObjectFilterCondition> conditions;
        //指定的过滤目标
        private Transform specifiedTarget;
        //用于存储所有的组件类型
        private List<Type> components;
        //视图滚动
        private Vector2 scroll;

        private void OnEnable()
        {
            conditions = new List<GameObjectFilterCondition>();
            //获取所有组件类型
            components = new List<Type>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                var types = assemblies[i].GetTypes();
                for (int j = 0; j < types.Length; j++)
                {
                    if (types[j].IsSubclassOf(typeof(Component)))
                    {
                        components.Add(types[j]);
                    }
                }
            }
        }

        private void OnGUI()
        {
            OnTargetGUI();
            OnConditionGUI();
            OnBottomGUI();
        }

        private void OnTargetGUI()
        {
            target = (GameObjectFilterTarget)EditorGUILayout.EnumPopup("Target", target);
            if (target== GameObjectFilterTarget.Specified) 
            {
                specifiedTarget = EditorGUILayout.ObjectField(specifiedTarget, typeof(Transform), true) as Transform;
            }
            EditorGUILayout.Space();
        }
        private void OnConditionGUI()
        {
            EditorGUILayout.Space();
            if (conditions.Count == 0) return;
            GUILayout.BeginVertical("Box");
            scroll = EditorGUILayout.BeginScrollView(scroll);
            for (int i = 0; i < conditions.Count; i++)
            {
                var condition = conditions[i];
                GUILayout.BeginHorizontal();
                switch (condition.filterMode)
                {
                    case GameObjectFilterMode.Name:
                        GUILayout.Label("Name", GUILayout.Width(80f));
                        condition.stringValue = EditorGUILayout.TextField(condition.stringValue);
                        break;
                    case GameObjectFilterMode.Component:
                        var index = components.FindIndex(m => m.Name == condition.typeValue.Name);
                        GUILayout.Label("Component", GUILayout.Width(80f));
                        var newIndex = EditorGUILayout.Popup(index, components.Select(m => m.Name).ToArray());
                        if (index != newIndex) condition.typeValue = components[newIndex];
                        break;
                    case GameObjectFilterMode.Layer:
                        GUILayout.Label("Layer", GUILayout.Width(80f));
                        condition.intValue = EditorGUILayout.LayerField(condition.intValue);
                        break;
                    case GameObjectFilterMode.Tag:
                        GUILayout.Label("Tag", GUILayout.Width(80f));
                        condition.stringValue = EditorGUILayout.TagField(condition.stringValue);
                        break;
                    case GameObjectFilterMode.Active:
                        GUILayout.Label("Active", GUILayout.Width(80f));
                        condition.boolValue = EditorGUILayout.Toggle(condition.boolValue);
                        break;
                    case GameObjectFilterMode.Missing:
                        GUILayout.Label("Missing", GUILayout.Width(80f));
                        condition.missingMode = (MissingMode)EditorGUILayout.EnumPopup(condition.missingMode);
                        break;
                    default:
                        break;
                }
                if (GUILayout.Button("-", GUILayout.Width(20f)))
                {
                    conditions.RemoveAt(i);
                    return;
                }
                GUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
        private void OnBottomGUI()
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("添加", "DropDownButton"))
            {
                GenericMenu gm = new GenericMenu();
                gm.AddItem(new GUIContent("Name"), false, () => conditions.Add(new GameObjectFilterCondition(GameObjectFilterMode.Name, "GameObject")));
                gm.AddItem(new GUIContent("Component"), false, () => conditions.Add(new GameObjectFilterCondition(GameObjectFilterMode.Component, typeof(Transform))));
                gm.AddItem(new GUIContent("Layer"), false, () => conditions.Add(new GameObjectFilterCondition(GameObjectFilterMode.Layer, 0)));
                gm.AddItem(new GUIContent("Tag"), false, () => conditions.Add(new GameObjectFilterCondition(GameObjectFilterMode.Tag, "Untagged")));
                gm.AddItem(new GUIContent("Active"), false, () => conditions.Add(new GameObjectFilterCondition(GameObjectFilterMode.Active, true)));
                gm.AddItem(new GUIContent("Missing / Material"), false, () => conditions.Add(new GameObjectFilterCondition(GameObjectFilterMode.Missing, MissingMode.Material)));
                gm.AddItem(new GUIContent("Missing / Mesh"), false, () => conditions.Add(new GameObjectFilterCondition(GameObjectFilterMode.Missing, MissingMode.Mesh)));
                gm.AddItem(new GUIContent("Missing / Script"), false, () => conditions.Add(new GameObjectFilterCondition(GameObjectFilterMode.Missing, MissingMode.Script)));
                gm.ShowAsContext();
            }
            if (GUILayout.Button("预设", "DropDownButton"))
            {
                GenericMenu gm = new GenericMenu();
                gm.AddItem(new GUIContent("导入"), false, () => 
                {
                    string path = EditorUtility.OpenFilePanel("导入", Application.dataPath, "asset");
                    if (string.IsNullOrEmpty(path)) return;
                    path = path.Replace(Application.dataPath, "Assets");
                    var preset = AssetDatabase.LoadAssetAtPath<GameObjectFilterConditionPreset>(path);
                    if (preset != null)
                    {
                        conditions.Clear();
                        for (int i = 0; i < preset.conditions.Length; i++)
                        {
                            conditions.Add(preset.conditions[i]);
                        }
                    }
                });
                gm.AddItem(new GUIContent("导出"), false, () =>
                {
                    if(conditions.Count > 0)
                    {
                        //选择保存的文件路径
                        string path = EditorUtility.SaveFilePanel("导出", Application.dataPath, "New HierarchyFilterConditionPreset", "asset");
                        if (string.IsNullOrEmpty(path)) return;
                        //最后一个'/'符号的位置
                        int lastIndex = path.LastIndexOf('/');
                        //文件夹的路径等于文件的路径截取lastIndex长度的结果
                        string folder = path.Substring(0, lastIndex);
                        folder = folder.Replace(Application.dataPath, "Assets");
                        if (AssetDatabase.IsValidFolder(folder))
                        {
                            path = path.Replace(Application.dataPath, "Assets");
                            GameObjectFilterConditionPreset preset = CreateInstance<GameObjectFilterConditionPreset>();
                            preset.conditions = new GameObjectFilterCondition[conditions.Count];
                            for (int i = 0; i < preset.conditions.Length; i++)
                            {
                                preset.conditions[i] = conditions[i];
                            }
                            AssetDatabase.CreateAsset(preset, path);
                            AssetDatabase.Refresh();
                            EditorGUIUtility.PingObject(preset);
                        }
                        else
                        {
                            Debug.Log("无效路径");
                        }
                    }
                });
                gm.ShowAsContext();
            }
            if (GUILayout.Button("清空"))
            {
                if (conditions.Count == 0) return;
                if (EditorUtility.DisplayDialog("提醒", "是否清空所有过滤条件？", "确定", "取消"))
                {
                    conditions.Clear();
                }
            }
            if (GUILayout.Button("过滤"))
            {
                if (conditions.Count == 0) return;
                List<GameObject> gos = new List<GameObject>();
                switch (target)
                {
                    case GameObjectFilterTarget.All:
                        Scene[] scenes = SceneManager.GetAllScenes();
                        for (int i = 0; i < scenes.Length; i++)
                        {
                            GameObject[] rootGos = scenes[i].GetRootGameObjects();
                            for (int j = 0; j < rootGos.Length; j++)
                            {
                                Transform[] childs = rootGos[j].GetComponentsInChildren<Transform>(true);
                                for(int k = 0; k < childs.Length; k++)
                                {
                                    EditorUtility.DisplayProgressBar("Filter", "过滤中...", (float)i / rootGos.Length);
                                    gos.Add(childs[k].gameObject);
                                }
                            }
                        }
                        EditorUtility.ClearProgressBar();
                        break;
                    case GameObjectFilterTarget.Specified:
                        Transform[] cs = specifiedTarget.GetComponentsInChildren<Transform>(true);
                        for(int i = 0;i < cs.Length; i++)
                        {
                            EditorUtility.DisplayProgressBar("Filter", "过滤中...", (float)i / cs.Length);
                            gos.Add(cs[i].gameObject);
                        }
                        EditorUtility.ClearProgressBar();
                        break;
                    default:
                        break;
                }
                List<GameObject> matched = new List<GameObject>();
                for (int i = 0; i < gos.Count; i++)
                {
                    GameObject go = gos[i];
                    bool isMatched = true;
                    for (int j = 0; j < conditions.Count; j++)
                    {
                        if (!conditions[j].IsMatch(go))
                        {
                            isMatched = false;
                            break;
                        }
                    }
                    EditorUtility.DisplayProgressBar("Filter", "过滤中...", (float)i / gos.Count);
                    if(isMatched) matched.Add(go);
                }
                EditorUtility.ClearProgressBar();
                Selection.objects = matched.ToArray();
                Debug.Log(string.Format("过滤出符合条件物体数量：{0}", matched.Count));
            }
        }
    }
}