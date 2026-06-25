/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace SK.Framework.Resource
{
    public class AssetBundleRenamePopupWindowContent : PopupWindowContent
    {
        private readonly AssetBundleEditorInfo m_AssetBundleInfo;
        private readonly AssetBundleConfigure m_Configure;
        private string newName;

        public AssetBundleRenamePopupWindowContent(AssetBundleEditorInfo info, AssetBundleConfigure configure)
        {
            m_AssetBundleInfo = info;
            m_Configure = configure;
            newName = info.name;
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(250f, EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
        }

        public override void OnGUI(Rect rect)
        {
            EditorGUILayout.BeginHorizontal();
            newName = EditorGUILayout.TextField(newName);
            if (GUILayout.Button("Ok", GUILayout.Width(40f)))
            {
                if (!string.IsNullOrEmpty(newName) && newName != m_AssetBundleInfo.name)
                    m_Configure.RenameBundle(m_AssetBundleInfo.name, newName.ToLower());
                editorWindow.Close();
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    public class AssetBundleTagEditPopupContent : PopupWindowContent
    {
        private readonly string m_BundleName;
        private readonly AssetBundleProfile m_Profile;
        private readonly List<string> m_Tags;
        private string m_NewTag = "";

        public AssetBundleTagEditPopupContent(string bundleName, AssetBundleProfile profile)
        {
            m_BundleName = bundleName;
            m_Profile = profile;
            m_Tags = profile.GetTags(bundleName);
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(200f, (m_Tags.Count + 2) * (EditorGUIUtility.singleLineHeight
                + EditorGUIUtility.standardVerticalSpacing) + 10f);
        }

        public override void OnGUI(Rect rect)
        {
            EditorGUILayout.LabelField($"Edit Tags: {m_BundleName}", EditorStyles.boldLabel);
            for (int i = 0; i < m_Tags.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(m_Tags[i]);
                if (GUILayout.Button("-", GUILayout.Width(20f)))
                {
                    m_Tags.RemoveAt(i);
                    m_Profile.SetTags(m_BundleName, m_Tags);
                    editorWindow.Repaint();
                    break;
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.BeginHorizontal();
            m_NewTag = EditorGUILayout.TextField(m_NewTag);
            if (GUILayout.Button("+", GUILayout.Width(20f)) && !string.IsNullOrEmpty(m_NewTag))
            {
                if (!m_Tags.Contains(m_NewTag))
                {
                    m_Tags.Add(m_NewTag);
                    m_Profile.SetTags(m_BundleName, m_Tags);
                    m_NewTag = "";
                    editorWindow.Repaint();
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}