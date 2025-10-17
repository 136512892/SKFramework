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
    public class TimeAttribute : PropertyAttribute 
    {
        public readonly bool showMilliseconds;
        public bool hideHours;

        public TimeAttribute(bool showMilliseconds = false, bool hideHours = false)
        {
            this.showMilliseconds = showMilliseconds;
            this.hideHours = hideHours;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(TimeAttribute))]
    public class TimePropertyDrawer : PropertyDrawer
    {
        private const float m_MinLabelWidth = 90f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                case SerializedPropertyType.Float:
                    if (attribute is TimeAttribute ta)
                    {
                        var labelWidth = Mathf.Max(position.width * .2f, m_MinLabelWidth);
                        var inputWidth = position.width - labelWidth;
                        var inputRect = new Rect(position.x, position.y, inputWidth, position.height);
                        var labelRcct = new Rect(position.x + inputWidth, position.y, labelWidth, position.height);
                        double v = property.propertyType switch
                        {
                            SerializedPropertyType.Integer => property.intValue,
                            SerializedPropertyType.Float => property.floatValue,
                            _ => 0
                        };
                        if (v < 0)
                        {
                            v = 0;
                            SetProperyValue(property, v);
                        }

                        var newValue = 0d;
                        switch (property.propertyType)
                        {
                            case SerializedPropertyType.Integer:
                                newValue = EditorGUI.IntField(inputRect, label, (int)v);
                                break;
                            case SerializedPropertyType.Float:
                                newValue = EditorGUI.FloatField(inputRect, label, (float)v);
                                break;
                        }
                        if (newValue < 0)
                            newValue = 0;
                        SetProperyValue(property, newValue);
                        EditorGUI.LabelField(labelRcct, GetTimeFormat(newValue, ta.showMilliseconds, ta.hideHours));
                    }
                    break;
                default:
                    EditorGUI.HelpBox(position, "Unsupported type", MessageType.Error);
                    break;
            }
        }

        private void SetProperyValue(SerializedProperty property, double v)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    property.intValue = (int)v;
                    break;
                case SerializedPropertyType.Float:
                    property.floatValue = (float)v;
                    break;
            }
        }

        private string GetTimeFormat(double totalSeconds, bool showMilliseconds, bool hideHours)
        {
            int hours = (int)(totalSeconds / 3600);
            double remaining = totalSeconds % 3600;
            int minutes = (int)(remaining / 60);
            double seconds = remaining % 60;

            int milliseconds = showMilliseconds ? (int)((seconds - Math.Floor(seconds)) * 1000) : 0;
            int wholeSeconds = (int)Math.Floor(seconds);

            if (hideHours)
            {
                return showMilliseconds
                    ? $"({minutes:D2}:{wholeSeconds:D2}.{milliseconds:D3})"
                    : $"({minutes:D2}:{wholeSeconds:D2})";
            }
            else
            {
                return showMilliseconds
                    ? $"({hours:D2}:{minutes:D2}:{wholeSeconds:D2}.{milliseconds:D3})"
                    : $"({hours:D2}:{minutes:D2}:{wholeSeconds:D2})";
            }
        }
    }
#endif
}