/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SK.Framework
{
    [AttributeUsage(AttributeTargets.Field)]
    public class LabelAttribute : PropertyAttribute
    {
        public readonly string text;

        public LabelAttribute(string text)
        {
            this.text = text;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(LabelAttribute))]
    public class LabelPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (attribute is LabelAttribute la)
            {
                label.tooltip = label.text;
                label.text = la.text;
            }
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
#endif
}