using System;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Reflection;

namespace SK.Framework
{
    [CustomPropertyDrawer(typeof(AxisInput))]
    public class AxisInputPropertyDrawer : PropertyDrawer
    {
        private string[] axisNames;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.LabelField(new Rect(position.x, position.y, position.width * .4f, position.height), label);
            var keyProperty = property.FindPropertyRelative("key");

            if (axisNames == null)
            {
                axisNames = typeof(AxisNames).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                    .Where(m => m.IsLiteral && !m.IsInitOnly).Select(m => m.Name).ToArray();
            }
            int index = Array.IndexOf(axisNames, keyProperty.stringValue);
            int newIndex = EditorGUI.Popup(
                new Rect(position.x + position.width * .4f, position.y, position.width * .6f, position.height),
                index, axisNames);
            if (newIndex != index)
            {
                keyProperty.stringValue = axisNames[newIndex];
            }
        }
    }
}