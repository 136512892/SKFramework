using System;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Collections;

namespace SK.Framework
{
    [CustomPropertyDrawer(typeof(ScoreIDAttribute))]
    public class ScoreIDPropertyAttributeDrawer : PropertyDrawer
    {
        private int[] scoreIDArray;
        private GUIContent[] scoreIDConstArray;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (scoreIDConstArray == null)
            {
                ArrayList constants = new ArrayList();
                FieldInfo[] fieldInfos = typeof(ScoreIDConstant).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                for (int i = 0; i < fieldInfos.Length; i++)
                {
                    var fi = fieldInfos[i];
                    if (fi.IsLiteral && !fi.IsInitOnly) constants.Add(fi);
                }
                FieldInfo[] fieldInfoArray = (FieldInfo[])constants.ToArray(typeof(FieldInfo));
                scoreIDArray = new int[fieldInfoArray.Length];
                scoreIDConstArray = new GUIContent[fieldInfoArray.Length];
                for (int i = 0; i < fieldInfoArray.Length; i++)
                {
                    scoreIDConstArray[i] = new GUIContent(fieldInfoArray[i].Name);
                    scoreIDArray[i] = (int)fieldInfoArray[i].GetValue(null);
                }
            }
            var index = Array.IndexOf(scoreIDArray, property.intValue);
            index = Mathf.Clamp(index, 0, scoreIDArray.Length);
            index = EditorGUI.Popup(position, label, index, scoreIDConstArray);
            property.intValue = scoreIDArray[index];
        }
    }
}