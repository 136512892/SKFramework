/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
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
    public class TextureAttribute : PropertyAttribute
    {
        public readonly Type textureType;

        public TextureAttribute(Type textureType)
        {
            this.textureType = textureType;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(TextureAttribute))]
    public class TexturePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (attribute is TextureAttribute ta)
            {
                label = EditorGUI.BeginProperty(position, label, property);
                position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
                EditorGUI.ObjectField(new Rect(position.x, position.y, 110f, 100f), property, ta.textureType, GUIContent.none);
                if (property.objectReferenceValue != null)
                {
                    Rect nameRect = new Rect(position.x + 105f, position.y + 35f, position.width - 105f, position.height);
                    EditorGUI.LabelField(nameRect, property.objectReferenceValue.name);
                }
                EditorGUI.EndProperty();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 110f;
        }
    }
#endif
}