/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace SK.Framework.Procedure
{
    [CustomEditor(typeof(Procedure))]
    public class ProcedureEditor : BaseEditor
    {
        private string[] m_ProcedureNames;
        private List<string> m_ValidProcedureNames;
        private SerializedProperty m_ProcedureEntry;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_ProcedureNames = typeof(Procedure).Assembly.GetTypes()
                .Where(m => !m.IsAbstract && m.IsSubclassOf(typeof(ProcedureBase)))
                .Select(m => m.FullName)
                .ToArray();
            var fieldInfo = typeof(Procedure).GetField("m_ProcedureNames", BindingFlags.Instance | BindingFlags.NonPublic);
            m_ValidProcedureNames = fieldInfo.GetValue(target) as List<string>;
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (var i = 0; i < m_ValidProcedureNames.Count; i++)
            {
                var procedureName = m_ValidProcedureNames[i];
                if (assemblies.Any(m => m.GetType(procedureName) != null))
                    continue;
                m_ValidProcedureNames.RemoveAt(i);
                i--;
                Debug.LogWarning($"Remove invalid procedure type: {procedureName}");
                fieldInfo.SetValue(target as Procedure, m_ValidProcedureNames);
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }
            m_ProcedureEntry = serializedObject.FindProperty("m_ProcedureEntry");
            if (!string.IsNullOrEmpty(m_ProcedureEntry.stringValue) && !assemblies.Any(m => m.GetType(m_ProcedureEntry.stringValue) != null))
            {
                m_ProcedureEntry.stringValue = string.Empty;
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.LabelField("Procedures:", EditorStyles.boldLabel);
            for (int i = 0; i < m_ProcedureNames.Length; i++)
            {
                var procedureName = m_ProcedureNames[i];
                var flag = m_ValidProcedureNames.Contains(procedureName);
                var modified = EditorGUILayout.Toggle(procedureName, flag);
                if (modified != flag)
                {
                    Undo.RecordObject(target, "Procedure Names");
                    if (modified)
                        m_ValidProcedureNames.Add(procedureName);
                    else
                        m_ValidProcedureNames.Remove(procedureName);
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(target);
                }
            }

            EditorGUILayout.Space();

            var index = m_ValidProcedureNames.FindIndex(m => m == m_ProcedureEntry.stringValue);
            var newIndex = EditorGUILayout.Popup("Entry", index, m_ValidProcedureNames.ToArray());
            if (index != newIndex)
            {
                Undo.RecordObject(target, "Procedure Entry");
                m_ProcedureEntry.stringValue = m_ValidProcedureNames[newIndex];
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }

            if (EditorApplication.isPlaying)
                EditorGUILayout.LabelField("Current", SKFramework.Module<Procedure>().current.GetType().FullName);
        }
    }
}