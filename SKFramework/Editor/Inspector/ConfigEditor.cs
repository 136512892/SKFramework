/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using UnityEditor;

namespace SK.Framework.Config
{
    [CustomEditor(typeof(Config))]
    public class ConfigEditor : BaseEditor
    {
        private string[] m_ConfigLoaderNames;
        private List<string> m_ConfigLoaderNameList;
        private FieldInfo m_ConfigLoaderNameListFieldInfo;
        private Dictionary<string, bool> m_ConfigLoaderDic;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_ConfigLoaderNameListFieldInfo = typeof(Config).GetField("m_Loaders", 
                BindingFlags.Instance | BindingFlags.NonPublic);
            m_ConfigLoaderNameList = m_ConfigLoaderNameListFieldInfo.GetValue(target) as List<string>;

            m_ConfigLoaderNames = Assembly.GetAssembly(typeof(Config)).GetTypes()
                .Where(m => !m.IsAbstract && typeof(IConfigLoader).IsAssignableFrom(m))
                .Select(m => m.FullName)
                .ToArray();

            m_ConfigLoaderDic = new Dictionary<string, bool>(m_ConfigLoaderNames.Length);
            for (int i = 0; i < m_ConfigLoaderNames.Length; i++)
            {
                string configLoaderName = m_ConfigLoaderNames[i];
                m_ConfigLoaderDic.Add(configLoaderName, m_ConfigLoaderNameList.Contains(configLoaderName));
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.LabelField("Config Loaders:", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            for (int i = 0;i < m_ConfigLoaderNames.Length; i++)
            {
                string configLoaderName = m_ConfigLoaderNames[i];
                bool isContains = m_ConfigLoaderNameList.Contains(configLoaderName);
                bool v = EditorGUILayout.Toggle(configLoaderName, isContains);
                if (v != isContains)
                {
                    if (v)
                        m_ConfigLoaderNameList.Add(configLoaderName);
                    else
                        m_ConfigLoaderNameList.Remove(configLoaderName);
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Config Loaders");
                m_ConfigLoaderNameListFieldInfo.SetValue(target, m_ConfigLoaderNameList);
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }
        }
    }
}