/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SK.Framework.Resource
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AssetBundleEncryptStrategyAttribute : PropertyAttribute { }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(AssetBundleEncryptStrategyAttribute))]
    public class AssetBundleEncryptStrategyPropertyDrawer : PropertyDrawer
    {
        private GUIContent[] strategyArray;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                if (strategyArray == null)
                {
                    strategyArray = typeof(AssetBundleEncryptStrategy).Assembly.GetTypes()
                        .Where(m => m.IsSubclassOf(typeof(AssetBundleEncryptStrategy)))
                        .ToArray()
                        .Select(m => new GUIContent(m.FullName))
                        .ToArray();
                }
                int index = Array.FindIndex(strategyArray, m => m.text == property.stringValue);
                int target = EditorGUI.Popup(position, label, index, strategyArray);
                if (index != target)
                {
                    property.stringValue = strategyArray[target].text;
                }
            }
        }
    }
#endif
}