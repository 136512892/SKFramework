/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework.Debugger
{
    [WindowTitle("Console")]
    public class ConsoleWindow : DebuggerWindow
    {
        private List<ConsoleWindowItem> m_Items;
        private Vector2 m_listScroll;
        private Vector2 m_DetailScroll;
        private int m_InfoCount;
        private int m_WarnCount;
        private int m_ErrorCount;

        private bool m_ShowInfo = true;
        private bool m_ShowWarn = true;
        private bool m_ShowError = true;

        private int m_MaxCacheCount = 999;
        private string m_SearchContent;
        private ConsoleWindowItem m_Selected;

        public int warnCount { get { return m_WarnCount; } }
        public int errorCount { get { return m_ErrorCount; } }

        public override void OnInitialized()
        {
            m_Items = new List<ConsoleWindowItem>();
            Application.logMessageReceived += OnLogMessageReceived;
        }

        public override void OnDestroy()
        {
            Application.logMessageReceived -= OnLogMessageReceived;
            m_Selected = null;
            if (m_Items != null)
            {
                m_Items.Clear();
                m_Items = null;
            }
        }

        private void OnLogMessageReceived(string condition, string stackTrace, LogType logType)
        {
            var item = new ConsoleWindowItem(logType, condition, stackTrace);
            switch (logType)
            {
                case LogType.Log: m_InfoCount++; break;
                case LogType.Warning: m_WarnCount++; break;
                default: m_ErrorCount++; break;
            }
            m_Items.Add(item);
            if (m_Items.Count > m_MaxCacheCount)
            {
                var remove = m_Items[0];
                switch (remove.logType)
                {
                    case LogType.Log: m_InfoCount--; break;
                    case LogType.Warning: m_WarnCount--; break;
                    default: m_ErrorCount--; break;
                }
                m_Items.Remove(remove);
            }
        }

        public override void OnGUI()
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear", GUILayout.Width(50f)))
            {
                m_Items.Clear();
                m_InfoCount = 0;
                m_WarnCount = 0;
                m_ErrorCount = 0;
                m_Selected = null;
            }
            m_SearchContent = GUILayout.TextField(m_SearchContent, GUILayout.ExpandWidth(true));
            GUI.contentColor = m_ShowInfo ? Color.white : Color.gray;
            m_ShowInfo = GUILayout.Toggle(m_ShowInfo, string.Format("Info [{0}]", m_InfoCount), GUILayout.Width(70f));
            GUI.contentColor = m_ShowWarn ? Color.white : Color.gray;
            m_ShowWarn = GUILayout.Toggle(m_ShowWarn, string.Format("Warn [{0}]", m_WarnCount), GUILayout.Width(70f));
            GUI.contentColor = m_ShowError ? Color.white : Color.gray;
            m_ShowError = GUILayout.Toggle(m_ShowError, string.Format("Error [{0}]", m_ErrorCount), GUILayout.Width(70f));
            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("Box", GUILayout.Height(Screen.height * .3f - 20f));
            m_listScroll = GUILayout.BeginScrollView(m_listScroll);
            if (m_Items.Count > 0)
            {
                for (int i = m_Items.Count - 1; i >= 0; i--)
                {
                    var item = m_Items[i];
                    if (!string.IsNullOrEmpty(m_SearchContent)
                        && !item.message.ToLower().Contains(
                            m_SearchContent.ToLower())) continue;
                    bool show;
                    switch (item.logType)
                    {
                        case LogType.Log:
                            show = m_ShowInfo;
                            break;
                        case LogType.Warning:
                            show = m_ShowWarn;
                            GUI.contentColor = Color.yellow;
                            break;
                        default:
                            show = m_ShowError;
                            GUI.contentColor = Color.red;
                            break;
                    }
                    if (show)
                    {
                        if (GUILayout.Toggle(m_Selected == item, item.message))
                            m_Selected = item;
                    }
                    GUI.contentColor = Color.white;
                }
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            GUILayout.BeginVertical("Box", GUILayout.ExpandHeight(true));
            m_DetailScroll = GUILayout.BeginScrollView(m_DetailScroll);
            if (m_Selected != null)
                GUILayout.Label(m_Selected.stackTrace);
            GUILayout.EndScrollView();
            GUILayout.FlexibleSpace();
            GUI.enabled = m_Selected != null;
            if (GUILayout.Button("Copy", GUILayout.Height(20f)))
                GUIUtility.systemCopyBuffer = m_Selected.stackTrace;
            GUILayout.EndVertical();
        }
    }
}