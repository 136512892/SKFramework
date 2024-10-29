/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace SK.Framework.Resource
{
    internal class AssetBundleRenamePopupWindowContent : PopupWindowContent
    {
        private readonly AssetBundleInfo m_AssetBundleInfo;
        private string m_NameInput;

        public AssetBundleRenamePopupWindowContent(AssetBundleInfo info)
        {
            m_AssetBundleInfo = info;
            m_NameInput = info.name;
        }

        public override void OnGUI(Rect rect)
        {
            m_NameInput = EditorGUI.TextField(rect, m_NameInput);
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(150f, 20f);
        }

        public override void OnClose()
        {
            if (!string.IsNullOrEmpty(m_NameInput) && m_NameInput != m_AssetBundleInfo.name)
            {
                m_AssetBundleInfo.Rename(m_NameInput);
                AssetDatabase.RemoveUnusedAssetBundleNames();
            }
        }
    }
}
#endif