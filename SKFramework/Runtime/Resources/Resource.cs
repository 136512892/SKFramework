/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SK.Framework.Resource
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/SKFramework.Resource")]
    public class Resource : ModuleBase
    {
        [SerializeField] private ResourceAgent m_Agent;

        protected internal override void OnInitialization()
        {
            base.OnInitialization();
            m_Agent.OnInitialization();
        }

        public void LoadAssetAsync<T>(string assetPath, Action<bool, T> onCompleted = null, Action<float> onLoading = null) where T : Object
        {
            m_Agent.LoadAssetAsync(assetPath, onCompleted, onLoading);
        }

        public void UnloadAsset(string assetPath, bool unloadAllLoadedObject = false)
        {
            m_Agent.UnloadAsset(assetPath, unloadAllLoadedObject);
        }

        public void UnloadAllAsset(bool unloadAllLoadedObject = false)
        {
            m_Agent.UnloadAllAsset(unloadAllLoadedObject);
        }

        public void LoadSceneAsync(string sceneAssetPath, Action<bool> onCompleted = null, Action<float> onLoading = null)
        {
            m_Agent.LoadSceneAsync(sceneAssetPath, onCompleted, onLoading);
        }

        public bool IsSceneLoaded(string sceneAssetPath)
        {
            return m_Agent.IsSceneLoaded(sceneAssetPath);
        }

        public bool UnloadScene(string sceneAssetPath)
        {
            return m_Agent.UnloadScene(sceneAssetPath);
        }
    }
}