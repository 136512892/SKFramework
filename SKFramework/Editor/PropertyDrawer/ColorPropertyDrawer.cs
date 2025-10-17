/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEditor;
using UnityEngine;
using System.Globalization;

namespace SK.Framework
{
    [CustomPropertyDrawer(typeof(Color))]
    public class ColorPropertyDrawer : PropertyDrawer
    {
        private const float m_Spacing = 5f;
        private const float m_HexWidth = 60f;
        private const float m_AlphaWidth = 32f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            float colorWidth = position.width - m_HexWidth - m_Spacing - m_AlphaWidth - m_Spacing;
            var newColor = EditorGUI.ColorField(new Rect(position.x, position.y, colorWidth, position.height), property.colorValue);
            if (!newColor.Equals(property.colorValue))
                property.colorValue = newColor;
            var hex = EditorGUI.TextField(new Rect(position.x + colorWidth + m_Spacing, position.y, m_HexWidth, position.height),
                UnityEngine.ColorUtility.ToHtmlStringRGB(property.colorValue));
            try
            {
                newColor = FromHex(hex, property.colorValue.a);
                if (!newColor.Equals(property.colorValue))
                    property.colorValue = newColor;
            }
            finally { }

            var newAlpha = EditorGUI.Slider(new Rect(position.x + colorWidth + m_HexWidth + (m_Spacing * 2f),
                position.y, m_AlphaWidth, position.height), property.colorValue.a, 0f, 1f);
            if (!newAlpha.Equals(property.colorValue.a))
                property.colorValue = new Color(property.colorValue.r, property.colorValue.g, property.colorValue.b, newAlpha);

            EditorGUI.EndProperty();
        }

        private static Color FromHex(string hexValue, float alpha)
        {
            if (string.IsNullOrEmpty(hexValue)) return Color.clear;
            if (hexValue[0] == '#') hexValue = hexValue.TrimStart('#');
            if (hexValue.Length > 6) hexValue = hexValue.Remove(6, hexValue.Length - 6);
            int value = int.Parse(hexValue, NumberStyles.HexNumber);
            int r = value >> 16 & 255;
            int g = value >> 8 & 255;
            int b = value & 255;
            return new Color(r / 255f, g / 255f, b / 255f, alpha);
        }
    }
}