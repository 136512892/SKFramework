/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using SK.Framework.Logger;
using ILogger = SK.Framework.Logger.ILogger;

namespace SK.Framework
{
    [CustomEditor(typeof(Log))]
    public class LogEditor : BaseEditor
    {
        private SerializedProperty m_LoggerNameProperty;

        private string[] m_LoggerNames;
        private int m_CurrentLoggerIndex;
        private Dictionary<string, bool> m_LoggerDic;
        private List<string> m_OptionalLoggerNameList;
        private FieldInfo m_OptionalLoggerNameListFieldInfo;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_LoggerNameProperty = serializedObject.FindProperty("m_LoggerName");
            m_OptionalLoggerNameListFieldInfo = typeof(Log).GetField("m_OptionalLoggerNameList", 
                BindingFlags.Instance | BindingFlags.NonPublic);
            m_OptionalLoggerNameList = m_OptionalLoggerNameListFieldInfo.GetValue(target) as List<string>;

            m_LoggerNames = Assembly.GetAssembly(typeof(Log)).GetTypes()
                .Where(m => !m.IsAbstract && typeof(ILogger).IsAssignableFrom(m)
                    && m.GetCustomAttribute<NecessaryLoggerAttribute>() == null)
                .Select(m => m.FullName)
                .ToArray();
            m_CurrentLoggerIndex = Array.IndexOf(m_LoggerNames, m_LoggerNameProperty.stringValue);

            m_LoggerDic = new Dictionary<string, bool>(m_LoggerNames.Length);
            for (int i = 0; i < m_LoggerNames.Length; i++)
            {
                string loggerName = m_LoggerNames[i];
                m_LoggerDic.Add(loggerName, m_OptionalLoggerNameList.Contains(loggerName));
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            int index = EditorGUILayout.Popup("Default Logger", m_CurrentLoggerIndex, m_LoggerNames);
            if (index != m_CurrentLoggerIndex)
            {
                Undo.RecordObject(target, "Default Logger");
                m_CurrentLoggerIndex = index;
                m_LoggerNameProperty.stringValue = m_LoggerNames[m_CurrentLoggerIndex];
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }

            EditorGUILayout.Space();

            GUILayout.Label("Optional Logger：", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < m_LoggerNames.Length; i++)
            {
                string loggerName = m_LoggerNames[i];
                bool isContains = m_OptionalLoggerNameList.Contains(loggerName);
                bool v = EditorGUILayout.Toggle(loggerName, isContains);
                if (v != isContains)
                {
                    if(v) 
                        m_OptionalLoggerNameList.Add(loggerName);
                    else
                        m_OptionalLoggerNameList.Remove(loggerName);
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Optional");
                m_OptionalLoggerNameListFieldInfo.SetValue(target, m_OptionalLoggerNameList);
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }
        }
    }
}