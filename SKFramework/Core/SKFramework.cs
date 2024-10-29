/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Collections.Generic;

using UnityEngine;

namespace SK.Framework
{
    public class SKFramework : MonoBehaviour
    {
        private static Dictionary<Type, IModule> m_ModuleDic;

        private void Awake()
        {
            m_ModuleDic = new Dictionary<Type, IModule>();
            IModule[] modules = GetComponentsInChildren<IModule>();
            for (int i = 0; i < modules.Length; i++)
            {
                IModule module = modules[i];
                module.OnInitialization();
                m_ModuleDic.Add(module.GetType(), module);
            }
            DontDestroyOnLoad(this);
        }

        private void OnDestroy()
        {
            foreach (IModule module in m_ModuleDic.Values)
            {
                module.OnTermination();
            }
            m_ModuleDic.Clear();
            m_ModuleDic = null;
        }

        public static T Module<T>() where T : IModule
        {
            if (m_ModuleDic.TryGetValue(typeof(T), out IModule module))
            {
                return (T)module;
            }
            return default;
        }

        public static bool HasModule<T>() where T : IModule
        {
            return m_ModuleDic != null && m_ModuleDic.ContainsKey(typeof(T));
        }
    }
}