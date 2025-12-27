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

        private Log m_LogComponent;
        private string[] m_LoggerNames;
        private int m_CurrentLoggerIndex;
        private Dictionary<string, bool> m_LoggerDic;
        private List<string> m_OptionalLoggerNameList;
        private FieldInfo m_OptionalLoggerNameListFieldInfo;
        private Dictionary<string, bool> m_NecessaryLoggerDic;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_LogComponent = target as Log;
            m_LoggerNameProperty = serializedObject.FindProperty("m_LoggerName");
            m_OptionalLoggerNameListFieldInfo = typeof(Log).GetField("m_OptionalLoggerNameList", 
                BindingFlags.Instance | BindingFlags.NonPublic);
            m_OptionalLoggerNameList = m_OptionalLoggerNameListFieldInfo.GetValue(target) as List<string>;
            m_NecessaryLoggerDic = new Dictionary<string, bool>();
            var types = Assembly.GetAssembly(typeof(Log)).GetTypes()
                .Where(m => !m.IsAbstract && typeof(ILogger).IsAssignableFrom(m))
                .ToArray();
            foreach (var type in types)
                m_NecessaryLoggerDic.Add(type.FullName, type.GetCustomAttribute<NecessaryLoggerAttribute>() != null);
            m_LoggerNames = types
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
                if (m_OptionalLoggerNameList.Contains(m_LoggerNameProperty.stringValue))
                {
                   m_OptionalLoggerNameList.Remove(m_LoggerNameProperty.stringValue);
                   m_OptionalLoggerNameListFieldInfo.SetValue(target, m_OptionalLoggerNameList);
                   serializedObject.ApplyModifiedProperties();
                   EditorUtility.SetDirty(target);
                }
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }

            EditorGUILayout.Space();

            GUILayout.Label("Optional Logger：", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < m_LoggerNames.Length; i++)
            {
                string loggerName = m_LoggerNames[i];
                if (loggerName == m_LoggerNameProperty.stringValue)
                    continue;
                if (m_NecessaryLoggerDic[loggerName])
                {
                    GUI.enabled = false;
                    EditorGUILayout.Toggle(loggerName, true);
                    GUI.enabled = true;
                    continue;
                }
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
            
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Only logs that are greater than or equal to this level will be output.", MessageType.Info);
            var level = (LogLevel)EditorGUILayout.EnumPopup("Level", m_LogComponent.Level);
            if (level != m_LogComponent.Level)
            {
                Undo.RecordObject(target, "Log Level");
                m_LogComponent.Level = level;
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }
        }
    }
}