/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

namespace SK.Framework
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/SKFramework")]
    public class SKFramework : MonoBehaviour
    {
        private static Dictionary<Type, ModuleBase> m_ModuleDic;

        private void Awake()
        {
            Debug.Log("SKFramework Launching...");
            m_ModuleDic = new Dictionary<Type, ModuleBase>();
            var modules = GetComponentsInChildren<ModuleBase>(true);
            for (int i = 0; i < modules.Length; i++)
            {
                var module = modules[i];
                m_ModuleDic.Add(module.GetType(), module);
            }
            var dic = m_ModuleDic.OrderBy(m => m.Value.m_Priority);
            foreach (var module in dic)
            {
                module.Value.OnInitialization();
            }
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            foreach (var module in m_ModuleDic.Values)
            {
                module.OnUpdate();
            }
        }

        private void OnDestroy()
        {
            foreach (var module in m_ModuleDic.Values)
            {
                module.OnTermination();
            }
            m_ModuleDic.Clear();
            m_ModuleDic = null;
        }

        public static T Module<T>() where T : ModuleBase
        {
            if (m_ModuleDic.TryGetValue(typeof(T), out var module))
            {
                return module as T;
            }
            throw new Exception($"Module of type {typeof(T).FullName} is not exists.");
        }

        public static bool TryGetModule<T>(out T module) where T : ModuleBase
        {
            if (m_ModuleDic != null && m_ModuleDic.TryGetValue(typeof(T), out var target))
            {
                module = target as T;
                return true;
            }
            module = default;
            return false;
        }

        public static bool Has<T>() where T : ModuleBase
        {
            return m_ModuleDic != null && m_ModuleDic.ContainsKey(typeof(T));
        }
    }
}