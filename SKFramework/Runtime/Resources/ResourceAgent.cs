/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

using SK.Framework.Logger;
using ILogger = SK.Framework.Logger.ILogger;

namespace SK.Framework.Resource
{
    public abstract class ResourceAgent : ScriptableObject, IResourceAgent
    {
        public enum MODE
        {
            EDITOR,
            SIMULATED, //StreamingAssets
            REALITY,
        }

        [SerializeField] protected MODE m_Mode = MODE.EDITOR;

        [SerializeField] protected string m_AssetBundleUrl = Application.streamingAssetsPath;

        [SerializeField] protected bool m_EncryptEnable;

        [SerializeField] protected string m_EncryptStrategy;

        [SerializeField] protected string m_SecretKey;

        protected AssetBundleEncryptStrategy m_Strategy;

        protected AssetBundleManifest m_AssetBundleManifest;

        protected readonly Dictionary<string, AssetInfo> m_Assets = new Dictionary<string, AssetInfo>();

        protected readonly Dictionary<string, AssetBundleInfo> m_AssetBundles = new Dictionary<string, AssetBundleInfo>();

        protected const string m_AssetBundleCache = "AssetBundleCache";

        protected string m_FullAssetBundleUrl;

        protected bool m_IsMapLoading = true;

        protected bool m_IsAssetBundleManifestLoading;

        protected readonly Dictionary<string, AssetBundle> m_AssetBundlesDic = new Dictionary<string, AssetBundle>();

        protected readonly Dictionary<string, Scene> m_SceneDic = new Dictionary<string, Scene>();

        protected readonly Dictionary<string, UnityWebRequest> m_LoadingDic = new Dictionary<string, UnityWebRequest>();

        protected ILogger m_Logger;

        public virtual void OnInitialization()
        {
            m_Logger = SKFramework.Module<Log>().GetLogger<ModuleLogger>();
            m_Logger.Info("[Resource] Mode:{0}", m_Mode);
#if UNITY_EDITOR
            if (m_Mode != MODE.EDITOR)
            {
                Func();
            }
#else
            Func();
#endif
            void Func()
            {
                m_FullAssetBundleUrl = (m_Mode == MODE.REALITY ? m_AssetBundleUrl : IOUtility.streamingAssetsPath) + "/AssetBundles";
                SKFramework.Module<Resource>().StartCoroutine(LoadAssetsMapAsync());
                EncryptStrategyInit();
            }
        }
        public void LoadAssetAsync<T>(string assetPath, Action<bool, T> onCompleted = null, Action<float> onLoading = null) where T : Object
        {
            SKFramework.Module<Resource>().StartCoroutine(LoadAssetAsyncCoroutine(assetPath, onCompleted, onLoading));
        }
        public void LoadSceneAsync(string sceneAssetPath, Action<bool> onCompleted = null, Action<float> onLoading = null)
        {
            SKFramework.Module<Resource>().StartCoroutine(LoadSceneAsyncCoroutine(sceneAssetPath, onCompleted, onLoading));
        }
        public bool IsSceneLoaded(string sceneAssetPath)
        {
            return m_SceneDic.ContainsKey(sceneAssetPath);
        }
        public void UnloadAsset(string assetPath, bool unloadAllLoadedObjects = false)
        {
            if (m_Assets.TryGetValue(assetPath, out var assetInfo))
            {
                if (m_AssetBundlesDic.ContainsKey(assetInfo.abName))
                {
                    m_AssetBundlesDic[assetInfo.abName].Unload(unloadAllLoadedObjects);
                    m_AssetBundlesDic.Remove(assetInfo.abName);
                    m_Logger.Info("[Resource] Unload asset with path: {0}", assetPath);
                }
                else
                {
                    m_Logger.Warning("[Resource] Asset with path {0} does not loaded.", assetPath);
                }
            }
            else
            {
                m_Logger.Warning("[Resource] Asset with path {0} does not exists.", assetPath);
            }
        }
        public void UnloadAllAsset(bool unloadAllLoadedObjects = false)
        {
            foreach (var kv in m_AssetBundlesDic)
            {
                kv.Value.Unload(unloadAllLoadedObjects);
            }
            m_AssetBundlesDic.Clear();
            AssetBundle.UnloadAllAssetBundles(unloadAllLoadedObjects);
            m_Logger.Info("[Resource] Unload all assets...");
        }
        public bool UnloadScene(string sceneAssetPath)
        {
#if UNITY_EDITOR
            if (m_Mode != MODE.EDITOR)
                return Func();
            if (m_SceneDic.TryGetValue(sceneAssetPath, out Scene scene))
            {
                m_SceneDic.Remove(sceneAssetPath);
                SceneManager.UnloadSceneAsync(scene);
                m_Logger.Info("[Resource] Unload scene with path: {0}", sceneAssetPath);
                return true;
            }
            m_Logger.Warning("[Resource] Scene with path {0} does not loaded.", sceneAssetPath);
            return false;
#else
            return Func();
#endif
            bool Func()
            {
                if (m_Assets.TryGetValue(sceneAssetPath, out var assetInfo))
                {
                    if (m_SceneDic.ContainsKey(sceneAssetPath))
                    {
                        m_SceneDic.Remove(sceneAssetPath);
                        SceneManager.UnloadSceneAsync(assetInfo.name);
                        m_Logger.Info("[Resource] Unload scene with path: {0}", sceneAssetPath);
                        return true;
                    }
                    m_Logger.Warning("[Resource] Scene with path {0} does not loaded.", sceneAssetPath);
                    return false;
                }
                m_Logger.Warning("[Resource] Scene with path {0} does not exists.", sceneAssetPath);
                return false;
            }
        }

        private void EncryptStrategyInit()
        {
            if (m_EncryptEnable)
            {
                if (!string.IsNullOrEmpty(m_SecretKey) && !string.IsNullOrEmpty(m_EncryptStrategy))
                {
                    Type type = Type.GetType(m_EncryptStrategy) ?? throw new ArgumentException();
                    m_Strategy = Activator.CreateInstance(type) as AssetBundleEncryptStrategy;
                }
            }
        }

        protected IEnumerator LoadAssetBundleManifestAsync()
        {
            m_Logger.Info("[Resource] Begin load asset bundle minifest...");
            using (UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(m_FullAssetBundleUrl + "/AssetBundles"))
            {
#if UNITY_2017_2_OR_NEWER
                yield return request.SendWebRequest();
#else
                yield return request.Send();
#endif
                bool flag = false;
#if UNITY_2020_2_OR_NEWER
                flag = request.result == UnityWebRequest.Result.Success;
#elif UNITY_2017_1_OR_NEWER
                flag = !(request.isNetworkError || request.isHttpError);
#else
                flag = !request.isError;
#endif
                if (flag)
                {
                    AssetBundle ab = DownloadHandlerAssetBundle.GetContent(request);
                    if (ab)
                    {
                        m_AssetBundleManifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                        m_IsAssetBundleManifestLoading = false;
                    }
                    else
                    {
                        m_Logger.Error("[Resource] Load asset bundle manifest failed.");
                    }
                }
                else
                {
                    m_Logger.Error("[Resource] Load asset bundle manifest error: {0}", request.error);
                }
            }
        }
        protected IEnumerator LoadAssetBundleDependenciesAsync(string[] dependencies, Action<float> onLoading)
        {
            for (int i = 0; i < dependencies.Length; i++)
            {
                var dep = dependencies[i];
                if (!m_AssetBundlesDic.ContainsKey(dep))
                {
                    yield return LoadAssetBundleAsync(dep, onLoading);
                }
            }
        }
        protected IEnumerator LoadAssetAsyncCoroutine<T>(string assetPath, Action<bool, T> onCompleted, Action<float> onLoading) where T : Object
        {
            Object asset = null;
#if UNITY_EDITOR
            if (m_Mode == MODE.EDITOR)
            {
                onLoading?.Invoke(1f);

                asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset == null)
                {
                    m_Logger.Error("[Resource] Load asset at path {0} failed.", assetPath);
                }
            }
            else yield return Func();
#else
            yield return Func();
#endif
            IEnumerator Func()
            {
                if (m_IsMapLoading)
                {
                    yield return new WaitUntil(() => m_IsMapLoading == false);
                }

                if (!m_Assets.TryGetValue(assetPath, out var assetInfo))
                {
                    m_Logger.Error("[Resource] Load asset at path {0} failed.", assetPath);
                    yield break;
                }

                if (!m_AssetBundleManifest)
                {
                    if (m_IsAssetBundleManifestLoading)
                    {
                        yield return new WaitUntil(() => m_AssetBundleManifest);
                    }
                    else
                    {
                        m_IsAssetBundleManifestLoading = true;
                        yield return LoadAssetBundleManifestAsync();
                    }
                }

                var dependencies = m_AssetBundleManifest.GetAllDependencies(assetInfo.abName);
                bool flag = m_AssetBundleManifest;
                if (flag)
                {
                    for (int i = 0; i < dependencies.Length; i++)
                    {
                        if (!m_AssetBundlesDic.ContainsKey(dependencies[i]))
                        {
                            flag = false;
                            break;
                        }
                    }
                }
                if (!flag)
                {
                    yield return LoadAssetBundleDependenciesAsync(dependencies, onLoading);
                }

                if (!m_AssetBundlesDic.ContainsKey(assetInfo.abName))
                {
                    yield return LoadAssetBundleAsync(assetInfo.abName, onLoading);
                }
                else
                {
                    onLoading?.Invoke(1f);
                }
                asset = m_AssetBundlesDic[assetInfo.abName].LoadAsset<T>(assetInfo.name);
                if (asset == null)
                {
                    m_Logger.Error("[Resource] Load asset at path {0} failed.", assetPath);
                }
            }
            if (asset != null)
                onCompleted?.Invoke(true, asset as T);
            else
                onCompleted?.Invoke(false, null);
        }
        protected IEnumerator LoadSceneAsyncCoroutine(string sceneAssetPath, Action<bool> onCompleted, Action<float> onLoading)
        {
#if UNITY_EDITOR
            if (m_Mode == MODE.EDITOR)
            {
                if (m_SceneDic.ContainsKey(sceneAssetPath))
                {
                    m_Logger.Warning("[Resource] Scene {0} already loaded.", sceneAssetPath);
                    onCompleted?.Invoke(false);
                    yield break;
                }

                m_SceneDic.Add(sceneAssetPath, new Scene());
                var asyncOperation = EditorSceneManager.LoadSceneAsyncInPlayMode(sceneAssetPath,
                    new LoadSceneParameters()
                    {
                        loadSceneMode = LoadSceneMode.Additive,
                        localPhysicsMode = LocalPhysicsMode.None
                    });
                if (asyncOperation == null)
                {
                    onCompleted?.Invoke(false);
                    m_Logger.Error("[Resource] Load scene at path {0} failed.", sceneAssetPath);
                    yield break;
                }
                while (!asyncOperation.isDone)
                {
                    onLoading?.Invoke(asyncOperation.progress);
                    yield return null;
                }
                onLoading?.Invoke(1f);
                var scene = SceneManager.GetSceneByPath(sceneAssetPath);
                m_SceneDic[sceneAssetPath] = scene;
            }
            else yield return Func();
#else
            yield return Func();
#endif
            IEnumerator Func()
            {
                if (m_IsMapLoading)
                {
                    yield return new WaitUntil(() => m_IsMapLoading == false);
                }
                if (!m_Assets.TryGetValue(sceneAssetPath, out var assetInfo))
                {
                    m_Logger.Error("[Resource] Load scene at path {0} failed.", sceneAssetPath);
                    yield break;
                }
                if (m_SceneDic.ContainsKey(sceneAssetPath))
                {
                    m_Logger.Warning("[Resource] Scene {0} already loaded.", sceneAssetPath);
                    onCompleted?.Invoke(false);
                    yield break;
                }

                if (!m_AssetBundleManifest)
                {
                    if (m_IsAssetBundleManifestLoading)
                    {
                        yield return new WaitUntil(() => m_AssetBundleManifest);
                    }
                    else
                    {
                        m_IsAssetBundleManifestLoading = true;
                        yield return LoadAssetBundleManifestAsync();
                    }
                }

                var dependencies = m_AssetBundleManifest.GetAllDependencies(assetInfo.abName);
                yield return LoadAssetBundleDependenciesAsync(dependencies, onLoading);

                var scene = SceneManager.GetSceneByPath(sceneAssetPath);
                m_SceneDic.Add(sceneAssetPath, scene);
                if (!m_AssetBundlesDic.ContainsKey(assetInfo.abName))
                {
                    yield return LoadAssetBundleAsync(assetInfo.abName, onLoading);
                }
                var asyncOperation = SceneManager.LoadSceneAsync(assetInfo.name, LoadSceneMode.Additive);
                if (asyncOperation == null)
                {
                    onCompleted?.Invoke(false);
                    m_Logger.Error("[Resource] Load scene {0} failed.", assetInfo.name);
                    yield break;
                }
                while (!asyncOperation.isDone)
                {
                    onLoading?.Invoke(asyncOperation.progress);
                    yield return null;
                }
                onLoading?.Invoke(1f);
            }
            onCompleted?.Invoke(true);
        }

        protected abstract IEnumerator LoadAssetsMapAsync();
        protected abstract IEnumerator LoadAssetBundleAsync(string assetBundleName, Action<float> onLoading = null);
    }
}