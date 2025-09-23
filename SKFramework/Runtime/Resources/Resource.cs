/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

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
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/SKFramework.Resource")]
    public class Resource : ModuleBase
    {
        public enum MODE
        {
            EDITOR,
            SIMULATED, //StreamingAssets
            REALITY,
        }

        [SerializeField] private MODE m_Mode = MODE.EDITOR;

        [SerializeField] private string m_AssetBundleUrl = Application.streamingAssetsPath;

        [SerializeField] private bool m_EncryptEnable;

        [SerializeField] private string m_EncryptStrategy;

        [SerializeField] private string m_SecretKey;

        private AssetBundleEncryptStrategy m_Strategy;

        private AssetBundleManifest m_AssetBundleManifest;

        private Dictionary<string, AssetInfo> m_Map = new Dictionary<string, AssetInfo>();

        private string m_FullAssetBundleUrl;

        private bool m_IsMapLoading = true;

        private bool m_IsAssetBundleManifestLoading;

        private readonly Dictionary<string, AssetBundle> m_AssetBundlesDic = new Dictionary<string, AssetBundle>();

        private readonly Dictionary<string, Scene> m_SceneDic = new Dictionary<string, Scene>();

        private readonly Dictionary<string, UnityWebRequest> m_LoadingDic = new Dictionary<string, UnityWebRequest>();

        private ILogger m_Logger;

        protected internal override void OnInitialization()
        {
            base.OnInitialization();
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
                m_FullAssetBundleUrl = (m_Mode == MODE.REALITY ? m_AssetBundleUrl : Application.streamingAssetsPath) + "/AssetBundles";
                StartCoroutine(LoadAssetsMapAsync());
                EncryptStrategyInit();
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

        private IEnumerator LoadAssetsMapAsync()
        {
#if UNITY_WEBGL
            string url = m_FullAssetBundleUrl + "/map.json";
#else
            string url = m_FullAssetBundleUrl + "/map.dat";
#endif
            m_Logger.Info("[Resource] Begin load assets map from:{0}", url);
            if (!string.IsNullOrEmpty(url))
            {
                using (UnityWebRequest request = UnityWebRequest.Get(url))
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
                        AssetsInfo assetsInfo = null;
#if UNITY_WEBGL
                        assetsInfo = JsonUtility.FromJson<AssetsInfo>(request.downloadHandler.text);
#else
                        var mapPath = Application.persistentDataPath + "/map.dat";
                        File.WriteAllBytes(mapPath, request.downloadHandler.data);
                        using (var fs = new FileStream(mapPath, FileMode.Open))
                        {
                            var bf = new BinaryFormatter();
                            var deserialize = bf.Deserialize(fs);
                            if (deserialize is byte[] buffer && buffer.Length > 0)
                            {
                                m_Map = new Dictionary<string, AssetInfo>();
                                var json = Encoding.Default.GetString(buffer);
                                assetsInfo = JsonUtility.FromJson<AssetsInfo>(json);
                            }
                        }
#endif
                        if (assetsInfo != null)
                        {
                            int counter = 0;
                            for (int i = 0; i < assetsInfo.list.Count; i++)
                            {
                                var info = assetsInfo.list[i];
                                info.name = Path.GetFileNameWithoutExtension(info.path);
                                m_Map.Add(info.path, info);
                                if (++counter == 100)
                                {
                                    counter = 0;
                                    yield return null;
                                }
                            }
                            m_IsMapLoading = false;
                            m_Logger.Info("[Resource] Load assets map successed.");
                        }
                    }
                    else
                    {
                        m_Logger.Error("[Resource] Load assets map error: {0}", request.error);
                    }
                }
            }
        }

        private IEnumerator LoadAssetBundleManifestAsync()
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

        private IEnumerator LoadAssetBundleAsync(string assetBundleName, Action<float> onLoading = null)
        {
            DateTime beginTime = DateTime.Now;

            if (m_LoadingDic.TryGetValue(assetBundleName, out var target))
            {
                yield return null;
                if (target != null)
                {
                    while (!target.isDone)
                    {
                        onLoading?.Invoke(target.downloadProgress);
                        yield return null;
                    }
                }
                yield return new WaitUntil(() => !m_LoadingDic.ContainsKey(assetBundleName));
            }
            else
            {
                string url = m_FullAssetBundleUrl + "/" + assetBundleName;
                using (UnityWebRequest request = m_Strategy != null
                           ? UnityWebRequest.Get(url) : UnityWebRequestAssetBundle.GetAssetBundle(url))
                {
                    m_LoadingDic.Add(assetBundleName, request);
#if UNITY_2017_2_OR_NEWER
                    request.SendWebRequest();
#else
                    request.Send();
#endif
                    while (!request.isDone)
                    {
                        onLoading?.Invoke(request.downloadProgress);
                        yield return null;
                    }
                    onLoading?.Invoke(1f);

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
                        AssetBundle ab;
                        if (m_Strategy != null)
                        {
                            byte[] bytes = request.downloadHandler.data;
                            m_Strategy.Decrypt(ref bytes, m_SecretKey);
                            AssetBundleCreateRequest createRequest = AssetBundle.LoadFromMemoryAsync(bytes);
                            yield return createRequest;
                            ab = createRequest.assetBundle;
                        }
                        else ab = DownloadHandlerAssetBundle.GetContent(request);

                        if (ab)
                        {
                            m_AssetBundlesDic.Add(assetBundleName, ab);
                            m_Logger.Info("[Resource] The request for downloading the asset bundle {0} was sent at time {1}," +
                                "completed at time {2},taking {3} milliseconds({4} seconds).", request.url, beginTime.ToString("T"),
                                DateTime.Now.ToString("T"), (DateTime.Now - beginTime).TotalMilliseconds,
                                (DateTime.Now - beginTime).TotalSeconds);
                        }
                        else
                        {
                            m_Logger.Error("[Resource] Download asset bundle {0} failed", assetBundleName);
                        }
                    }
                    else
                    {
                        m_Logger.Error("[Resource] Download asset bundle {0} error:{1}", assetBundleName, request.error);
                    }
                    yield return null;
                    m_LoadingDic.Remove(assetBundleName);
                }
            }
        }
        
        private IEnumerator LoadAssetBundleDependenciesAsync(string[] dependencies, Action<float> onLoading)
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

        private IEnumerator LoadAssetAsyncCoroutine<T>(string assetPath, Action<bool, T> onCompleted, Action<float> onLoading) where T : Object
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

                if (!m_Map.TryGetValue(assetPath, out var assetInfo))
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

        private IEnumerator LoadSceneAsyncCoroutine(string sceneAssetPath, Action<bool> onCompleted, Action<float> onLoading)
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
                if (!m_Map.TryGetValue(sceneAssetPath, out var assetInfo))
                {
                    m_Logger.Error("[Resource] Load scene at path {0} failed.", sceneAssetPath);
                    yield break;
                }
                if (m_SceneDic.ContainsKey(assetInfo.name))
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
                m_SceneDic.Add(assetInfo.name, scene);
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

        public void LoadAssetAsync<T>(string assetPath, Action<bool, T> onCompleted = null, Action<float> onLoading = null) where T : Object
        {
            StartCoroutine(LoadAssetAsyncCoroutine(assetPath, onCompleted, onLoading));
        }

        public void LoadSceneAsync(string sceneAssetPath, Action<bool> onCompleted = null, Action<float> onLoading = null)
        {
            StartCoroutine(LoadSceneAsyncCoroutine(sceneAssetPath, onCompleted, onLoading));
        }

        public void UnloadAsset(string assetPath, bool unloadAllLoadedObjects = false)
        {
            if (m_Map.TryGetValue(assetPath, out var assetInfo))
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
                if (m_Map.TryGetValue(sceneAssetPath, out var assetInfo))
                {
                    if (m_SceneDic.ContainsKey(assetInfo.name))
                    {
                        m_SceneDic.Remove(assetInfo.name);
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

        public void InstantiateAsync<T>(string assetPath, Action<bool, T> onCompleted = null, Action<float> onProgress = null) where T : Object
        {
            StartCoroutine(LoadAssetAsyncCoroutine<T>(assetPath, (success, asset) =>
            {
                if (success)
                {
                    var t = Instantiate(asset);
                    onCompleted?.Invoke(true, t);
                }
                else
                {
                    onCompleted?.Invoke(false, null);
                }
            }, onProgress));
        }
    }
}