using System.IO;
using UnityEditor;
using UnityEngine;
using System.Text;
using System.Collections.Generic;

namespace SK.Framework
{
    [CustomEditor(typeof(BatchBuildProfile))]
    public class BatchBuildProfileInspector : Editor
    {
        //配置文件
        private BatchBuildProfile profile;
        //折叠栏
        private Dictionary<BuildTask, bool> foldoutMap;

        private void OnEnable()
        {
            profile = target as BatchBuildProfile;
            foldoutMap = new Dictionary<BuildTask, bool>();
        }
        public override void OnInspectorGUI()
        {
            OnMenuGUI();
            OnListGUI();
            serializedObject.ApplyModifiedProperties();
            if (GUI.changed) EditorUtility.SetDirty(profile);
        }

        private void OnMenuGUI()
        {
            if (GUILayout.Button("新建"))
            {
                Undo.RecordObject(profile, "Create");
                var task = new BuildTask(PlayerSettings.productName, BuildTarget.StandaloneWindows64,
                    Directory.GetParent(Application.dataPath).FullName);
                profile.tasks.Add(task);
            }
            GUI.color = Color.yellow;
            if (GUILayout.Button("清空"))
            {
                Undo.RecordObject(profile, "Clear");
                if (EditorUtility.DisplayDialog("提醒", "是否确认清空所有打包工作项?", "确定", "取消"))
                {
                    profile.tasks.Clear();
                }
            }
            GUI.color = Color.cyan;
            if (GUILayout.Button("打包"))
            {
                if (EditorUtility.DisplayDialog("提醒", "打包需要耗费一定时间,是否确定开始?", "确定", "取消"))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("打包报告:\r\n");
                    for (int i = 0; i < profile.tasks.Count; i++)
                    {
                        var task = profile.tasks[i];
                        List<EditorBuildSettingsScene> buildScenes = new List<EditorBuildSettingsScene>();
                        for (int j = 0; j < task.sceneAssets.Count; j++)
                        {
                            var scenePath = AssetDatabase.GetAssetPath(task.sceneAssets[j]);
                            if (!string.IsNullOrEmpty(scenePath))
                            {
                                buildScenes.Add(new EditorBuildSettingsScene(scenePath, true));
                            }
                        }
                        EditorUtility.DisplayProgressBar("正在打包", profile.tasks[i].productName, (float)i + 1 / profile.tasks.Count);

                        string locationPathName = $"{task.buildPath}/{task.productName}";
                        switch (task.buildTarget)
                        {
                            case BuildTarget.StandaloneWindows64: locationPathName += ".exe"; break;
                            default: break;
                        }
                        var report = BuildPipeline.BuildPlayer(buildScenes.ToArray(), locationPathName, task.buildTarget, BuildOptions.None);
                        sb.Append($"{task.productName} 打包结果: {report.summary.result}\r\n");
                        Application.OpenURL(task.buildPath);
                    }
                    EditorUtility.ClearProgressBar();
                    Debug.Log(sb.ToString());
                }
                return;
            }
            GUI.color = Color.white;
        }
        private void OnListGUI()
        {
            for (int i = 0; i < profile.tasks.Count; i++)
            {
                var task = profile.tasks[i];
                if (!foldoutMap.ContainsKey(task))
                {
                    foldoutMap.Add(task, true);
                }
                GUILayout.BeginHorizontal("Badge");
                GUILayout.Space(12);
                foldoutMap[task] = EditorGUILayout.Foldout(foldoutMap[task], task.productName, true);
                if (GUILayout.Button(EditorGUIUtility.IconContent("TreeEditor.Trash"), "IconButton", GUILayout.Width(20)))
                {
                    Undo.RecordObject(profile, "Delete Task");
                    foldoutMap.Remove(task);
                    profile.tasks.Remove(task);
                    break;
                }
                GUILayout.EndHorizontal();
                if (foldoutMap[task])
                {
                    GUILayout.BeginVertical("Box");
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("打包场景：", GUILayout.Width(70));
                    if (GUILayout.Button("+", GUILayout.Width(20f)))
                    {
                        task.sceneAssets.Add(null);
                    }
                    GUILayout.EndHorizontal();
                    if (task.sceneAssets.Count > 0)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(75);
                        GUILayout.BeginVertical("Badge");
                        for (int j = 0; j < task.sceneAssets.Count; j++)
                        {
                            var sceneAsset = task.sceneAssets[j];
                            GUILayout.BeginHorizontal();
                            GUILayout.Label($"{j + 1}.", GUILayout.Width(20));
                            task.sceneAssets[j] = EditorGUILayout.ObjectField(sceneAsset, typeof(SceneAsset), false) as SceneAsset;
                            if (GUILayout.Button("↑", "MiniButtonLeft", GUILayout.Width(20)))
                            {
                                if (j > 0)
                                {
                                    Undo.RecordObject(profile, "Move Up Scene Assets");
                                    var temp = task.sceneAssets[j - 1];
                                    task.sceneAssets[j - 1] = sceneAsset;
                                    task.sceneAssets[j] = temp;
                                }
                            }
                            if (GUILayout.Button("↓", "MiniButtonMid", GUILayout.Width(20)))
                            {
                                if (j < task.sceneAssets.Count - 1)
                                {
                                    Undo.RecordObject(profile, "Move Down Scene Assets");
                                    var temp = task.sceneAssets[j + 1];
                                    task.sceneAssets[j + 1] = sceneAsset;
                                    task.sceneAssets[j] = temp;
                                }
                            }
                            if (GUILayout.Button("+", "MiniButtonMid", GUILayout.Width(20)))
                            {
                                Undo.RecordObject(profile, "Add Scene Assets");
                                task.sceneAssets.Insert(j + 1, null);
                                break;
                            }
                            if (GUILayout.Button("-", "MiniButtonMid", GUILayout.Width(20)))
                            {
                                Undo.RecordObject(profile, "Delete Scene Assets");
                                task.sceneAssets.RemoveAt(j);
                                break;
                            }
                            GUILayout.EndHorizontal();
                        }
                        GUILayout.EndVertical();
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("产品名称：", GUILayout.Width(70));
                    var newPN = GUILayout.TextField(task.productName);
                    if (task.productName != newPN)
                    {
                        Undo.RecordObject(profile, "Product Name");
                        task.productName = newPN;
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("打包平台：", GUILayout.Width(70));
                    var newBT = (BuildTarget)EditorGUILayout.EnumPopup(task.buildTarget);
                    if (task.buildTarget != newBT)
                    {
                        Undo.RecordObject(profile, "Build Target");
                        task.buildTarget = newBT;
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("打包路径：", GUILayout.Width(70));
                    GUILayout.TextField(task.buildPath);
                    if (GUILayout.Button("浏览", GUILayout.Width(40f)))
                    {
                        task.buildPath = EditorUtility.SaveFolderPanel("Build Path", task.buildPath, "");
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                }
            }
        }
    }
}