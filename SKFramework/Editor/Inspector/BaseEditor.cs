/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEditor;
using UnityEngine;

namespace SK.Framework
{
    public class BaseEditor : Editor
    {
        private SerializedProperty m_ScriptProperty;

        protected virtual void OnEnable()
        {
            m_ScriptProperty = serializedObject.FindProperty("m_Script");
        }

        public override void OnInspectorGUI()
        {
            GUI.enabled = false;
            EditorGUILayout.PropertyField(m_ScriptProperty);
            GUI.enabled = true;
        }
    }
}