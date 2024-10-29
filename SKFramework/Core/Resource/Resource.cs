/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
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

namespace SK.Framework.Resource
{
    public class Resource : ModuleBase
    {
        public enum MODE
        {
            EDITOR, //编辑器模式
            SIMULATIVE, //模拟模式（StreamingAssets）
            REALITY, //真实环境
        }

        [SerializeField] private MODE m_Mode = MODE.EDITOR;

        [SerializeField] private string m_AssetBundleUrl = Application.streamingAssetsPath;

        [SerializeField] private string m_AssetBundleManifestName = "AssetBundles";

        private string m_Url;

        [AssetBundleEncryptStrategy]
        [SerializeField] private string m_EncryptStrategy;

        [SerializeField] private string m_SecretKey;

        private AssetBundleEncryptStrategy m_Strategy;

        private AssetBundleManifest m_AssetBundleManifest;

        private Dictionary<string, AssetInfo> m_Map = new Dictionary<string, AssetInfo>();

        private bool m_IsMapLoading = true;

        private bool m_IsAssetBundleManifestLoading;

        private readonly Dictionary<string, AssetBundle> m_AssetBundlesDic = new Dictionary<string, AssetBundle>();

        private readonly Dictionary<string, Scene> m_SceneDic = new Dictionary<string, Scene>();

        private readonly Dictionary<string, UnityWebRequest> m_LoadingDic = new Dictionary<string, UnityWebRequest>();

        public override void OnInitialization()
        {
            base.OnInitialization();
#if UNITY_EDITOR
            if (m_Mode != MODE.EDITOR)
                Init();
#else
            Init();
#endif
            void Init()
            {
                m_Url = m_Mode == MODE.REALITY
                    ? m_AssetBundleUrl
                    : Application.streamingAssetsPath;
                StartCoroutine(LoadAssetsMapAsync());
                EncryptStrategyInit();
            }
        }

        private void EncryptStrategyInit()
        {
            if (!string.IsNullOrEmpty(m_SecretKey) && !string.IsNullOrEmpty(m_EncryptStrategy))
            {
                Type type = Type.GetType(m_EncryptStrategy);
                if (type == null)
                    throw new ArgumentException();
                m_Strategy = Activator.CreateInstance(type) as AssetBundleEncryptStrategy;
            }
        }

        private IEnumerator LoadAssetsMapAsync()
        {
#if UNITY_WEBGL
            string url = m_Url + "/map.json";
#else
            string url = m_Url + "/map.dat";
#endif
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
                    string mapPath = Application.persistentDataPath + "/map.dat";
                    File.WriteAllBytes(mapPath, request.downloadHandler.data);
                    using (FileStream fs = new FileStream(mapPath, FileMode.Open))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        var deserialize = bf.Deserialize(fs);
                        if (deserialize != null)
                        {
                            byte[] buffer = deserialize as byte[];
                            if (buffer != null && buffer.Length > 0)
                            {
                                m_Map = new Dictionary<string, AssetInfo>();
                                string json = Encoding.Default.GetString(buffer);
                                assetsInfo = JsonUtility.FromJson<AssetsInfo>(json);
                            }
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
                        Debug.Log("成功加载资源信息");
                    }
                }
                else
                {
                    Debug.LogError(string.Format("请求下载map失败：{0} {1}", request.url, request.error));
                }
            }
        }

        private IEnumerator LoadAssetBundleManifestAsync()
        {
            string url = m_Url + "/" + m_AssetBundleManifestName;
            using (UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url))
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
                    if (ab != null)
                    {
                        m_AssetBundleManifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                        m_IsAssetBundleManifestLoading = false;
                    }
                    else
                    {
                        Debug.LogError(string.Format("下载AssetBundleManifest失败：{0}", request.url));
                    }
                }
                else
                {
                    Debug.LogError(string.Format("请求下载AssetBundleManifest失败：{0} {1}", request.url, request.error));
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
                string url = m_Url + "/" + assetBundleName;
                using (UnityWebRequest request = m_Strategy != null ? UnityWebRequest.Get(url)
                           : UnityWebRequestAssetBundle.GetAssetBundle(url))
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

                        if (ab != null)
                        {
                            m_AssetBundlesDic.Add(assetBundleName, ab);
                            Debug.Log(string.Format("于{0}发起下载AssetBundle请求 {1} 于{2}下载完成 耗时{3}毫秒（{4}秒）",
                                beginTime.ToString("T"), request.url, DateTime.Now.ToString("T"),
                                (DateTime.Now - beginTime).TotalMilliseconds, (DateTime.Now - beginTime).TotalSeconds));
                        }
                        else
                        {
                            Debug.LogError(string.Format("下载AssetBundle失败：{0}", request.url));
                        }
                    }
                    else
                    {
                        Debug.LogError(string.Format("请求下载AssetBundle失败：{0} {1}", request.url, request.error));
                    }
                    yield return null;
                    m_LoadingDic.Remove(assetBundleName);
                }
            }
        }

        private IEnumerator LoadAssetBundleDependeciesAsync(string assetBundleName, Action<float> onLoading)
        {
            if (m_AssetBundleManifest == null)
            {
                if (m_IsAssetBundleManifestLoading)
                {
                    yield return new WaitUntil(() => m_AssetBundleManifest != null);
                }
                else
                {
                    m_IsAssetBundleManifestLoading = true;
                    yield return LoadAssetBundleManifestAsync();
                }
            }

            string[] dependencies = m_AssetBundleManifest.GetAllDependencies(assetBundleName);
            for (int i = 0; i < dependencies.Length; i++)
            {
                string dep = dependencies[i];
                if (!m_AssetBundlesDic.ContainsKey(dep))
                {
                    yield return LoadAssetBundleAsync(dep, onLoading);
                }
            }
        }

        private IEnumerator LoadAssetAsyncCoroutine<T>(string assetPath, Action<float> onLoading, Action<bool, T> onCompleted) where T : Object
        {
            Object asset = null;

#if UNITY_EDITOR
            if (m_Mode == MODE.EDITOR)
            {
                onLoading?.Invoke(1f);
                yield return null;

                asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset == null)
                {
                    Debug.LogError(string.Format("加载资源失败：{0}", assetPath));
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
                    Debug.LogError(string.Format("加载资源失败：{0}", assetPath));
                    yield break;
                }

                yield return LoadAssetBundleDependeciesAsync(assetInfo.abName, onLoading);

                if (!m_AssetBundlesDic.ContainsKey(assetInfo.abName))
                {
                    yield return LoadAssetBundleAsync(assetInfo.abName, onLoading);
                }
                else
                {
                    onLoading?.Invoke(1f);
                    yield return null;
                }
                asset = m_AssetBundlesDic[assetInfo.abName].LoadAsset<T>(assetInfo.name);
                if (asset == null)
                {
                    Debug.LogError(string.Format("加载资源失败：{0} {1}", assetInfo.abName, asset.name));
                }
            }
            
            if (asset != null)
                onCompleted?.Invoke(true, asset as T);
            else
                onCompleted?.Invoke(false, null);
        }

        private IEnumerator LoadSceneAsyncCoroutine(string sceneAssetPath, Action<float> onLoading, Action<bool> onCompleted)
        {
#if UNITY_EDITOR
            if (m_Mode == MODE.EDITOR)
            {
                if (m_SceneDic.ContainsKey(sceneAssetPath))
                {
                    Debug.LogWarning(string.Format("场景{0}已加载", sceneAssetPath));
                    onCompleted?.Invoke(false);
                    yield break;
                }

                m_SceneDic.Add(sceneAssetPath, new Scene());
                AsyncOperation asyncOperation = EditorSceneManager.LoadSceneAsyncInPlayMode(sceneAssetPath,
                    new LoadSceneParameters()
                    {
                        loadSceneMode = LoadSceneMode.Additive,
                        localPhysicsMode = LocalPhysicsMode.None
                    });
                while (!asyncOperation.isDone)
                {
                    onLoading?.Invoke(asyncOperation.progress);
                    yield return null;
                }
                onLoading?.Invoke(1f);
                Scene scene = EditorSceneManager.GetSceneByPath(sceneAssetPath);
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
                    Debug.LogError(string.Format("加载场景失败：{0}", sceneAssetPath));
                    yield break;
                }
                if (m_SceneDic.ContainsKey(assetInfo.name))
                {
                    Debug.LogWarning(string.Format("场景{0}已加载", sceneAssetPath));
                    onCompleted?.Invoke(false);
                    yield break;
                }

                yield return LoadAssetBundleDependeciesAsync(assetInfo.abName, onLoading);

                Scene scene = SceneManager.GetSceneByPath(sceneAssetPath);
                m_SceneDic.Add(assetInfo.name, scene);
                if (!m_AssetBundlesDic.ContainsKey(assetInfo.abName))
                {
                    yield return LoadAssetBundleAsync(assetInfo.abName, onLoading);
                }
                AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(assetInfo.name, LoadSceneMode.Additive);
                while (!asyncOperation.isDone)
                {
                    onLoading?.Invoke(asyncOperation.progress);
                    yield return null;
                }
                onLoading?.Invoke(1f);
            }
            onCompleted?.Invoke(true);
        }

        /// <summary>
        /// 异步加载资产
        /// </summary>
        /// <typeparam name="T">资产类型</typeparam>
        /// <param name="assetPath">资产路径</param>
        /// <param name="onLoading">加载进度回调事件</param>
        /// <param name="onCompleted">加载完成回调事件</param>
        public void LoadAssetAsync<T>(string assetPath, Action<float> onLoading = null, Action<bool, T> onCompleted = null) where T : Object
        {
            StartCoroutine(LoadAssetAsyncCoroutine(assetPath, onLoading, onCompleted));
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneAssetPath">场景资产路径</param>
        /// <param name="onLoading">加载过程回调事件</param>
        /// <param name="onCompleted">加载完成回调事件</param>
        public void LoadSceneAsync(string sceneAssetPath, Action<float> onLoading = null, Action<bool> onCompleted = null)
        {
            StartCoroutine(LoadSceneAsyncCoroutine(sceneAssetPath, onLoading, onCompleted));
        }

        /// <summary>
        /// 卸载资产
        /// </summary>
        /// <param name="assetPath">资产路径</param>
        /// <param name="unloadAllLoadedObjects">是否卸载相关实例化对象</param>
        public void UnloadAsset(string assetPath, bool unloadAllLoadedObjects = false)
        {
            if (m_Map.TryGetValue(assetPath, out var assetInfo))
            {
                if (m_AssetBundlesDic.ContainsKey(assetInfo.abName))
                {
                    m_AssetBundlesDic[assetInfo.abName].Unload(unloadAllLoadedObjects);
                    m_AssetBundlesDic.Remove(assetInfo.abName);
                }
            }
        }

        /// <summary>
        /// 卸载所有资产
        /// </summary>
        /// <param name="unloadAllLoadedObjects">是否卸载相关实例化对象</param>
        public void UnloadAllAsset(bool unloadAllLoadedObjects = false)
        {
            foreach (var kv in m_AssetBundlesDic)
            {
                kv.Value.Unload(unloadAllLoadedObjects);
            }
            m_AssetBundlesDic.Clear();
            AssetBundle.UnloadAllAssetBundles(unloadAllLoadedObjects);
        }

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneAssetPath">场景资产路径</param>
        /// <returns>true：卸载成功  false：卸载失败</returns>
        public bool UnloadScene(string sceneAssetPath)
        {
#if UNITY_EDITOR
            if (m_Mode == MODE.EDITOR)
            {
                if (m_SceneDic.TryGetValue(sceneAssetPath, out Scene scene))
                {
                    m_SceneDic.Remove(sceneAssetPath);
                    EditorSceneManager.UnloadSceneAsync(scene);
                    return true;
                }
                return false;
            }
            return Func();
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
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 异步实例化
        /// </summary>
        /// <typeparam name="T">资产类型</typeparam>
        /// <param name="assetPath">资产路径</param>
        /// <param name="onLoading">加载过程回调事件</param>
        /// <param name="onCompleted">结果回调事件</param>
        public void InstantiateAsync<T>(string assetPath, Action<float> onLoading = null, Action<bool, T> onCompleted = null) where T : Object
        {
            //异步加载资产
            StartCoroutine(LoadAssetAsyncCoroutine<T>(assetPath, onLoading, (success, asset) =>
            {
                //资产加载成功
                if (success)
                {
                    //实例化
                    T t = Instantiate(asset);
                    //执行回调
                    onCompleted?.Invoke(true, t);
                }
                //资产加载失败
                else
                {
                    //执行回调
                    onCompleted?.Invoke(false, null);
                }
            }));
        }
    }
}