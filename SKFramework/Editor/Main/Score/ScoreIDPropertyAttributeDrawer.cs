using System;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace SK.Framework
{
    [CustomPropertyDrawer(typeof(ScoreIDAttribute))]
    public class ScoreIDPropertyAttributeDrawer : PropertyDrawer
    {
        private int[] scoreIDArray;
        private GUIContent[] scoreIDConstArray;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (scoreIDConstArray == null)
            {
                List<FieldInfo> list = new List<FieldInfo>();
                FieldInfo[] fieldInfos = typeof(ScoreIDConstant).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                for (int i = 0; i < fieldInfos.Length; i++)
                {
                    var fi = fieldInfos[i];
                    if (fi.IsLiteral && !fi.IsInitOnly) list.Add(fi);
                }
                scoreIDArray = new int[list.Count];
                scoreIDConstArray = new GUIContent[list.Count];
                for (int i = 0; i < list.Count; i++)
                {
                    scoreIDConstArray[i] = new GUIContent(list[i].Name);
                    scoreIDArray[i] = (int)list[i].GetValue(null);
                }
            }
            var index = Array.IndexOf(scoreIDArray, property.intValue);
            index = Mathf.Clamp(index, 0, scoreIDArray.Length);
            index = EditorGUI.Popup(position, label, index, scoreIDConstArray);
            property.intValue = scoreIDArray[index];
        }
    }
}