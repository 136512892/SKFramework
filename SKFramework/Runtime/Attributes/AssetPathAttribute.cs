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
    public class AssetPathAttribute : PropertyAttribute
    {
        public readonly Type assetType;

        public AssetPathAttribute(Type assetType)
        {
            this.assetType = assetType;
        }
    }
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(AssetPathAttribute))]
    public class AssetPathPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.stringValue = EditorGUI.TextField(new Rect(position.x, position.y,
                position.width * .8f, position.height), label, property.stringValue);

            if (GUI.Button(new Rect(position.x + position.width * .8f + 5f, position.y, position.width * .1f - 5f, position.height), "PING"))
            {
                if (attribute is AssetPathAttribute apa)
                {
                    var obj = AssetDatabase.LoadAssetAtPath(property.stringValue, apa.assetType);
                    if (obj != null)
                        EditorGUIUtility.PingObject(obj);
                    else Debug.LogError(string.Format("Load asset at path {0} failed.", property.stringValue));
                }
            }
            if (GUI.Button(new Rect(position.x + position.width * .9f + 5f, position.y, position.width * .1f - 5f, position.height), "SELECT"))
            {
                EditorApplication.delayCall += () =>
                {
                    string path = EditorUtility.OpenFilePanel("Select Asset", Application.dataPath, string.Empty);
                    if (!string.IsNullOrEmpty(path))
                    {
                        property.stringValue = path.Replace(Application.dataPath, "Assets");
                        property.serializedObject.ApplyModifiedProperties();
                        EditorUtility.SetDirty(property.serializedObject.targetObject);
                    }
                };
            }
        }
    }
#endif
}