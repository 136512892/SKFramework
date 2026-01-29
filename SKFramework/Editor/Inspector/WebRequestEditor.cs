/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Linq;
using System.Reflection;

using UnityEngine;
using UnityEditor;
using SK.Framework.Networking;

namespace SK.Framework
{
    [CustomEditor(typeof(WebRequest))]
    public class WebRequestEditor : BaseEditor
    {
        private SerializedProperty m_WebRequestAgentName;
        private SerializedProperty m_MaxWebRequestAgentCount;
        private SerializedProperty m_Timeout;
        private SerializedProperty m_MaxRetries;
        private string[] m_AgentNames;
        private int m_CurrentAgentIndex;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_WebRequestAgentName = serializedObject.FindProperty("m_WebRequestAgentName");
            m_MaxWebRequestAgentCount = serializedObject.FindProperty("m_MaxWebRequestAgentCount");
            m_Timeout = serializedObject.FindProperty("m_Timeout");
            m_MaxRetries = serializedObject.FindProperty("m_MaxRetries");

            m_AgentNames = Assembly.GetAssembly(typeof(WebRequest)).GetTypes()
                .Where(m => !m.IsAbstract && typeof(IWebRequestAgent).IsAssignableFrom(m))
                .Select(m => m.FullName)
                .ToArray();
            m_CurrentAgentIndex = Array.IndexOf(m_AgentNames, m_WebRequestAgentName.stringValue);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUI.enabled = !EditorApplication.isPlaying;
            int index = EditorGUILayout.Popup("Web Request Agent", m_CurrentAgentIndex, m_AgentNames);
            if (index != m_CurrentAgentIndex)
            {
                Undo.RecordObject(target, "Agent");
                m_CurrentAgentIndex = index;
                m_WebRequestAgentName.stringValue = m_AgentNames[m_CurrentAgentIndex];
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }

            int agentCount = EditorGUILayout.IntSlider("Web Request Agent Count", m_MaxWebRequestAgentCount.intValue, 1, 8);
            if (agentCount != m_MaxWebRequestAgentCount.intValue)
            {
                Undo.RecordObject(target, "Web Request Agent Count");
                m_MaxWebRequestAgentCount.intValue = agentCount;
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }

            GUI.enabled = true;
            int timeout = EditorGUILayout.IntSlider("Web Request Timeout", m_Timeout.intValue, 5, 60);
            if (timeout != m_Timeout.intValue)
            {
                Undo.RecordObject(target, "Web Request Timeout");
                m_Timeout.intValue = timeout;
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }
            
            int maxRetries =  EditorGUILayout.IntSlider("Web Request Max Retries", m_MaxRetries.intValue, 0, 5);
            if (maxRetries != m_MaxRetries.intValue)
            {
                Undo.RecordObject(target, "Web Request Max Retries");
                m_MaxRetries.intValue = maxRetries;
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }

            if (EditorApplication.isPlaying)
            {
                var monitor = SKFramework.Module<WebRequest>().Monitor;
                EditorGUILayout.Space();
                GUILayout.Label("Web Request Monitor", EditorStyles.boldLabel);
                GUILayout.Label(string.Format("Success Rate:{0:P0}",
                    monitor.metrics.successCount 
                    / (float)(monitor.metrics.successCount + monitor.metrics.failureCount)));
                GUILayout.Label(string.Format("Average Latency:{0:F2}s", monitor.metrics.CalculateAverageLatency()));
                GUILayout.Label(string.Format("Total Download:{0:F2} KB", monitor.metrics.totalBytesDownloaded));
                GUILayout.Label(string.Format("Total Upload:{0:F2} KB", monitor.metrics.totalBytesUploaded));
            }
        }
    }
}