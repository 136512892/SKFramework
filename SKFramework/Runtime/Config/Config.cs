/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;
using SK.Framework.Logger;
using ILogger = SK.Framework.Logger.ILogger;

namespace SK.Framework.Config
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/SKFramework.Config")]
    public class Config : ModuleBase
    {
        [SerializeField] private List<string> m_Loaders = new List<string>();
        private readonly Dictionary<Type, IConfigLoader> m_LoaderDic = new Dictionary<Type, IConfigLoader>();
        private readonly Dictionary<string, object> m_ConfigDic = new Dictionary<string, object>();
        private ILogger m_Logger;

        protected internal override void OnInitialization()
        {
            base.OnInitialization();
            m_Logger = SKFramework.Module<Log>().GetLogger<ModuleLogger>();
            for (int i = 0; i < m_Loaders.Count; i++)
            {
                Type type = Type.GetType(m_Loaders[i]);
                if (type == null)
                    continue;
                var ctors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
                int index = Array.FindIndex(ctors, m => m.GetParameters().Length == 0);
                if (index != -1)
                {
                    IConfigLoader loader = Activator.CreateInstance(type) as IConfigLoader;
                    m_LoaderDic.Add(type, loader);
                    m_Logger.Info("[Config] Create loader:{0}", type.FullName);
                }
                else
                {
                    m_Logger.Error("[Config] A constructor with 0 arguments does not exist.");
                }
            }
        }

        public void Load<L, T>(string filePath) where L : IConfigLoader where T : class
        {
            if (!m_ConfigDic.ContainsKey(filePath))
            {
                if (m_LoaderDic.TryGetValue(typeof(L), out IConfigLoader loader))
                {
                    var dic = loader.Load<T>(filePath);
                    m_ConfigDic.Add(filePath, dic);
                    m_Logger.Info("[Config] Load config with type {0} from path: {1}", typeof(T).FullName, filePath);
                    return;
                }
                m_Logger.Error("[Config] The config loader of type {0} does not exist.", typeof(L).FullName);
            }
            else
            {
                m_Logger.Warning("[Config] Config with path {0} already loaded.", filePath);
            }
        }

        public void LoadAsync<L, T>(string filePath, Action onCompleted = null) where L : IConfigLoader where T : class
        {
            if (!m_ConfigDic.ContainsKey(filePath))
            {
                if (m_LoaderDic.TryGetValue(typeof(L), out IConfigLoader loader))
                {
                    loader.LoadAsync<T>(filePath, (success, dic) =>
                    {
                        if (success)
                        {
                            m_ConfigDic.Add(filePath, dic);
                            onCompleted?.Invoke();
                            m_Logger.Info("[Config] Load config with type {0} from path: {1}", typeof(T).FullName, filePath);
                        }
                        else
                        {
                            m_Logger.Error("[Config] Load config fail from path: {0}", filePath);
                        }
                    });
                    return;
                }
                m_Logger.Error("[Config] The config loader of type {0} does not exist.", typeof(L).FullName);
            }
            else
            {
                m_Logger.Warning("[Config] Config with path {0} already loaded.", filePath);
            }
        }

        public bool Unload(string filePath)
        {
            if (m_ConfigDic.ContainsKey(filePath))
            {
                m_ConfigDic.Remove(filePath);
                m_Logger.Info("[Config] Unload config with path: {0}", filePath);
                return true;
            }
            m_Logger.Info("[Config] Config with path {0} does not exists.", filePath);
            return false;
        }

        public T Get<T>(string filePath, int id) where T : class
        {
            if (m_ConfigDic.TryGetValue(filePath, out var config))
            {
                var dic = config as Dictionary<int, T>;
                if (dic.TryGetValue(id, out T t))
                {
                    return t;
                }
            }
            m_Logger.Error("[Config] Config not found:{0} ID={1}", typeof(T).FullName, id);
            return null;
        }

        public Dictionary<int, T> Get<T>(string filePath) where T : class
        {
            if (m_ConfigDic.TryGetValue(filePath, out var config))
            {
                return config as Dictionary<int, T>;
            }
            m_Logger.Error("[Config] Config not found:{0} FilePath:{1}", typeof(T).FullName, filePath);
            return null;
        }
    }
}