using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace SK.Framework
{
    [CustomEditor(typeof(ScoreMaster))]
    [CSDNUrl("https://coderz.blog.csdn.net/article/details/124960835?spm=1001.2014.3001.5502")]
    public class ScoreMasterInspector : AbstractEditor<ScoreMaster>
    {
        private Dictionary<string, ScoreGroup> groups;
        private Dictionary<ScoreGroup, bool> groupFoldout;

        protected override bool IsEnableBaseInspectorGUI
        {
            get
            {
                return true;
            }
        }

        protected override void OnRuntimeInspectorGUI()
        {
            if (groupFoldout == null)
            {
                groups = typeof(ScoreMaster).GetField("groups", BindingFlags.Instance | BindingFlags.NonPublic)
                    .GetValue(ScoreMaster.Instance) as Dictionary<string, ScoreGroup>;
                groupFoldout = new Dictionary<ScoreGroup, bool>();
            }

            foreach (var kv in groups)
            {
                if (!groupFoldout.ContainsKey(kv.Value))
                {
                    groupFoldout.Add(kv.Value, false);
                }

                ScoreGroup group = kv.Value;
                groupFoldout[group] = EditorGUILayout.Foldout(groupFoldout[group], group.Description);
                if (groupFoldout[group])
                {
                    GUILayout.Label($"计分模式： {(group.ValueMode == ValueMode.Additive ? "累加" : "互斥")}");
                    for (int i = 0; i < group.Scores.Count; i++)
                    {
                        ScoreItem score = group.Scores[i];
                        GUILayout.BeginVertical("Box");
                        GUI.color = score.IsObtained ? Color.green : Color.cyan;
                        GUILayout.Label($"描述： {score.Description}");
                        GUILayout.Label($"标识： {score.Flag}");
                        GUILayout.BeginHorizontal();
                        GUILayout.Label($"分值： {score.Value}   {(score.IsObtained ? "√" : "")}");
                        GUI.color = Color.cyan;
                        GUILayout.FlexibleSpace();
                        GUI.color = Color.yellow;
                        if (GUILayout.Button("Obtain", "ButtonLeft", GUILayout.Width(50f)))
                        {
                            Score.Obtain(group.Description, score.Flag);
                        }
                        if (GUILayout.Button("Cancle", "ButtonRight", GUILayout.Width(50f)))
                        {
                            Score.Cancle(group.Description, score.Flag);
                        }
                        GUI.color = Color.cyan;
                        GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                    }
                }
            }
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label($"总分： {Score.GetSum()}", "LargeLabel");
            GUILayout.Space(50f);
            GUILayout.EndHorizontal();
        }
    }
}