/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System.Reflection;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace SK.Framework
{
    [CustomEditor(typeof(SKFramework))]
    public class SKFrameworkEditor : BaseEditor
    {
        private FieldInfo m_PriorityFieldInfo;
        private Dictionary<ModuleBase, int> m_PriorityDic;
        private readonly GUIContent m_PriorityGUIContent
            = new GUIContent("Priority：", "The modules in the framework are initialized in order of priority.");

        protected override void OnEnable()
        {
            base.OnEnable();
            m_PriorityFieldInfo = typeof(ModuleBase).GetField("m_Priority",
                BindingFlags.Instance | BindingFlags.NonPublic);
            m_PriorityDic = new Dictionary<ModuleBase, int>();
            var modules = (target as SKFramework).GetComponentsInChildren<ModuleBase>();
            for (int i = 0; i < modules.Length; i++)
            {
                var module = modules[i];
                m_PriorityDic.Add(module, (int)m_PriorityFieldInfo.GetValue(module));
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Label(m_PriorityGUIContent, EditorStyles.boldLabel);
            foreach (var item in m_PriorityDic)
            {
                int v = EditorGUILayout.IntSlider(item.Key.GetType().Name, item.Value, -1, 19);
                if (v != item.Value)
                {
                    Undo.RecordObject(item.Key, "Priority");
                    m_PriorityDic[item.Key] = v;
                    m_PriorityFieldInfo.SetValue(item.Key, v);
                    EditorUtility.SetDirty(item.Key);
                    break;
                }
            }
        }
    }
}