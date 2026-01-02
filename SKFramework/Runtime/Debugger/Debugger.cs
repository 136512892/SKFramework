/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;

namespace SK.Framework.Debugger
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/SKFramework.Debugger")]
    public class Debugger : ModuleBase
    {
        private static readonly int m_WindowID = typeof(Debug).GetHashCode();

        [SerializeField] private WorkingType m_WorkingType = WorkingType.ONLY_OPEN_WHEN_DEVELOPMENT_BUILD;

        private bool m_IsInitialized;
        private Rect m_ExpandRect, m_RetractRect, m_DragRect;
        private bool m_IsExpand;
        private int m_fps;
        private float m_Timer;
        private List<DebuggerWindow> m_Windows;
        private DebuggerWindow m_CurrentWindow;

        protected internal override void OnInitialization()
        {
            base.OnInitialization();
            bool flag = false;
            switch (m_WorkingType)
            {
                case WorkingType.ALWAYS_OPEN:
                    flag = true;
                    break;
                case WorkingType.ALWAYS_CLOSE:
                    flag = false;
                    break;
                case WorkingType.ONLY_OPEN_WHEN_DEVELOPMENT_BUILD:
                    flag = Debug.isDebugBuild;
                    break;
                case WorkingType.ONLY_OPEN_IN_EDITOR:
                    flag = Application.isEditor;
                    break;
            }
            if (flag)
            {
                m_IsInitialized = true;
                float posX = Screen.width * .7f;
                m_ExpandRect = new Rect(posX, 0f, Screen.width - posX, Screen.height * .5f);
                m_RetractRect = new Rect(Screen.width - 100f, 0f, 100f, 60f);
                m_DragRect = new Rect(0f, 0f, Screen.width - posX, 20f);

                m_Windows = new List<DebuggerWindow>();
                Type[] types = GetType().Assembly.GetTypes()
                    .Where(m => m.IsSubclassOf(typeof(DebuggerWindow)))
                    .ToArray();
                for (int i = 0; i < types.Length; i++)
                {
                    Type type = types[i];
                    var window = Activator.CreateInstance(type) as DebuggerWindow;
                    var wta = type.GetCustomAttribute<WindowTitleAttribute>();
                    window.title = wta != null ? wta.title : type.Name;
                    window.OnInitialized();
                    m_Windows.Add(window);
                }
            }
        }

        protected internal override void OnTermination()
        {
            base.OnTermination();
            for (int i = 0; i < m_Windows.Count; i++)
                m_Windows[i].OnDestroy();
            m_Windows.Clear();
            m_Windows = null;
            m_CurrentWindow = null;
        }

        private void OnGUI()
        {
            if (!m_IsInitialized)
                return;

            if (!m_IsExpand)
            {
                m_RetractRect = GUI.Window(m_WindowID, m_RetractRect, OnRetractGUI, "Debugger");
                m_RetractRect.x = Mathf.Clamp(m_RetractRect.x, 0f, Screen.width - 100f);
                m_RetractRect.y = Mathf.Clamp(m_RetractRect.y, 0f, Screen.height - 60f);
                m_DragRect = new Rect(0f, 0f, 100f, 20f);
            }
            else
            {
                m_ExpandRect = GUI.Window(m_WindowID, m_ExpandRect, OnExpandGUI, "Debugger");
                m_ExpandRect.x = Mathf.Clamp(m_ExpandRect.x, 0f, Screen.width * .7f);
                m_ExpandRect.y = Mathf.Clamp(m_ExpandRect.y, 0f, Screen.height * .5f);
                m_DragRect = new Rect(0f, 0f, Screen.width * .3f, 20f);
            }
            if (Time.realtimeSinceStartup - m_Timer >= 1f)
            {
                m_fps = Mathf.RoundToInt(1f / Time.deltaTime);
                m_Timer = Time.realtimeSinceStartup;
            }
        }

        private void OnRetractGUI(int windowId)
        {
            GUI.DragWindow(m_DragRect);
            if (GUILayout.Button(string.Format("FPS:{0}", m_fps), GUILayout.Height(30f)))
                m_IsExpand = true;
        }

        private void OnExpandGUI(int windowId)
        {
            GUI.DragWindow(m_DragRect);
            if (GUILayout.Button(string.Format("FPS:{0}", m_fps), GUILayout.Height(20f)))
                m_IsExpand = false;

            GUILayout.BeginHorizontal();
            for (int i = 0; i < m_Windows.Count; i++)
            {
                var window = m_Windows[i];
                GUI.contentColor = m_CurrentWindow == window ? Color.white : Color.gray;
                if (GUILayout.Button(window.title))
                {
                    m_CurrentWindow?.OnExit();
                    m_CurrentWindow = window;
                    m_CurrentWindow.OnEnter();
                }
            }
            GUILayout.EndHorizontal();
            m_CurrentWindow?.OnGUI();
        }

        public bool TryGet<T>(out T window) where T : DebuggerWindow
        {
            window = default;
            var index = m_Windows.FindIndex(m => m.GetType() == typeof(T));
            if (index != -1)
            {
                window = (T)m_Windows[index];
                return true;
            }
            return false;
        }
    }
}