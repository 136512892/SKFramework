/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Reflection;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace SK.Framework.ObjectPool
{
    [CustomEditor(typeof(ObjectPool))]
    public class ObjectPoolEditor : BaseEditor
    {
        private Dictionary<Type, IObjectPool> m_Dic;
        private Dictionary<Type, bool> m_FoldoutDic;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (EditorApplication.isPlaying)
            {
                m_Dic = target.GetType().GetField("m_Dic", BindingFlags.Instance | BindingFlags.NonPublic)
                    .GetValue(SKFramework.Module<ObjectPool>()) as Dictionary<Type, IObjectPool>;
                m_FoldoutDic = new Dictionary<Type, bool>();
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (!EditorApplication.isPlaying)
            {
                return;
            }

            foreach (var item in m_Dic)
            {
                Type type = item.Key;
                if (!m_FoldoutDic.ContainsKey(type))
                    m_FoldoutDic.Add(type, false);

                m_FoldoutDic[type] = EditorGUILayout.Foldout(m_FoldoutDic[type], item.Key.FullName, true);

                if (m_FoldoutDic[type])
                {
                    var pool = item.Value;
                    GUILayout.Label(string.Format("Current Cache Count: {0}", pool.currentCacheCount));
                    GUILayout.Label(string.Format("Max Cache Count: {0}", pool.maxCacheCount));
                    GUILayout.Label(string.Format("Max Keep Duration: {0}", pool.maxKeepDuration));
                }
            }   
        }
    }
}