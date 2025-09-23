/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Linq;

using UnityEditor;
using UnityEngine;

namespace SK.Framework.Resource
{
    [CustomEditor(typeof(Resource))]
    public class ResourceEditor : BaseEditor
    {
        private SerializedProperty m_Mode;
        private SerializedProperty m_AssetBundleUrl;
        private SerializedProperty m_EncryptEnable;
        private SerializedProperty m_EncryptStrategy;
        private SerializedProperty m_SecretKey;
        private string[] m_StrategyArray;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_Mode = serializedObject.FindProperty("m_Mode");
            m_AssetBundleUrl = serializedObject.FindProperty("m_AssetBundleUrl");
            m_EncryptEnable = serializedObject.FindProperty("m_EncryptEnable");
            m_EncryptStrategy = serializedObject.FindProperty("m_EncryptStrategy");
            m_SecretKey = serializedObject.FindProperty("m_SecretKey");
            m_StrategyArray = typeof(AssetBundleEncryptStrategy).Assembly.GetTypes()
                .Where(m => m.IsSubclassOf(typeof(AssetBundleEncryptStrategy)))
                .ToArray()
                .Select(m => m.FullName)
                .ToArray();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            switch (m_Mode.enumValueIndex)
            {
                case (int)Resource.MODE.EDITOR:
                    EditorGUILayout.HelpBox("Asset will be loaded through the API in the AssetDatabase.", MessageType.Info);
                    break;
                case (int)Resource.MODE.SIMULATED:
                    EditorGUILayout.HelpBox("Asset will be loaded from the StreamingAsset path:" +
                        " Assets/StreamingAssets/AssetBundles/", MessageType.Info);
                    break;
                case (int)Resource.MODE.REALITY:
                    EditorGUILayout.HelpBox(string.Format("Asset will be loaded from the path: {0}/AssetBundles/",
                        m_AssetBundleUrl.stringValue), MessageType.Info);
                    break;
            }
            
            var mode = EditorGUILayout.Popup("Mode", m_Mode.enumValueIndex, m_Mode.enumNames);
            if (mode != m_Mode.enumValueIndex)
            {
                Undo.RecordObject(target, "Mode");
                m_Mode.enumValueIndex = mode;
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }

            GUI.enabled = m_Mode.enumValueIndex == (int)Resource.MODE.REALITY;
            var url = EditorGUILayout.TextField("Asset Bundles Url", m_AssetBundleUrl.stringValue);
            if (url != m_AssetBundleUrl.stringValue)
            {
                Undo.RecordObject(target, "Asset Bundles Url");
                m_AssetBundleUrl.stringValue = url;
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }
            GUI.enabled = true;
            
            if (m_Mode.enumValueIndex != 0)
            {
                bool encryptEnable = EditorGUILayout.Toggle("Encrypt Enable", m_EncryptEnable.boolValue);
                if (encryptEnable != m_EncryptEnable.boolValue)
                {
                    Undo.RecordObject(target, "Encrypt Enable");
                    m_EncryptEnable.boolValue = encryptEnable;
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(target);
                }

                if (encryptEnable)
                {
                    int index = Array.FindIndex(m_StrategyArray, m => m == m_EncryptStrategy.stringValue);
                    int v = EditorGUILayout.Popup("Encrypt Strategy", index, m_StrategyArray);
                    if (index != v)
                    {
                        Undo.RecordObject(target, "Encrypt Strategy");
                        m_EncryptStrategy.stringValue = m_StrategyArray[v];
                        serializedObject.ApplyModifiedProperties();
                        EditorUtility.SetDirty(target);
                    }

                    string secretKey = EditorGUILayout.TextField("Secret Key", m_SecretKey.stringValue);
                    if (secretKey != m_SecretKey.stringValue)
                    {
                        Undo.RecordObject(target, "Secret Key");
                        m_SecretKey.stringValue = secretKey;
                        serializedObject.ApplyModifiedProperties();
                        EditorUtility.SetDirty(target);
                    }
                }
            }
        }
    }
}