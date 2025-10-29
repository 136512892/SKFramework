/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using Object = UnityEngine.Object;

namespace SK.Framework.Resource
{
    public interface IResourceAgent
    {
        void OnInitialization();

        void LoadAssetAsync<T>(string assetPath, Action<bool, T> onCompleted = null, Action<float> onLoading = null) where T : Object;

        void UnloadAsset(string assetPath, bool unloadAllLoadedObject = false);

        void UnloadAllAsset(bool unloadAllLoadedObject = false);

        void LoadSceneAsync(string sceneAssetPath, Action<bool> onCompleted = null, Action<float> onLoading = null);

        bool IsSceneLoaded(string sceneAssetPath);

        bool UnloadScene(string sceneAssetPath);
    }
}