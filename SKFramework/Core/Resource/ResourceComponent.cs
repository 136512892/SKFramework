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
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/Resource")]
    public class ResourceComponent : MonoBehaviour, IResouceComponent
    {
        public enum MODE
        {
            EDITOR, //编辑器模式
            SIMULATIVE, //模拟模式（StreamingAssets）
            REALITY, //真实环境
        }

        [SerializeField] private MODE mode = MODE.EDITOR;

        [SerializeField] private string assetBundleUrl = Application.streamingAssetsPath;

        [SerializeField] private string assetBundleManifestName = "AssetBundles";

        private AssetBundleManifest assetBundleManifest;

        private Dictionary<string, AssetInfo> map = new Dictionary<string, AssetInfo>();

        private bool isMapLoading = true;

        private bool isAssetBundleManifestLoading;

        private readonly Dictionary<string, AssetBundle> assetBundlesDic = new Dictionary<string, AssetBundle>();

        private readonly Dictionary<string, Scene> sceneDic = new Dictionary<string, Scene>();

        private readonly Dictionary<string, UnityWebRequest> loadingDic = new Dictionary<string, UnityWebRequest>();

        private void Start()
        {
#if UNITY_EDITOR
            if (mode != MODE.EDITOR)
                StartCoroutine(LoadAssetsMapAsync());
#else
            StartCoroutine(LoadAssetsMapAsync());
#endif
        }

        private IEnumerator LoadAssetsMapAsync()
        {
            string url = (mode == MODE.REALITY ? assetBundleUrl : Application.streamingAssetsPath) + "/map.dat";
            //Main.Log.Info("资源下载路径：{0}", url);
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
                    string mapPath = Application.persistentDataPath + "/map.dat";
                    File.WriteAllBytes(mapPath, request.downloadHandler.data);
                    //打开文件
                    using (FileStream fs = new FileStream(mapPath, FileMode.Open))
                    {
                        //反序列化
                        BinaryFormatter bf = new BinaryFormatter();
                        var deserialize = bf.Deserialize(fs);
                        if (deserialize != null)
                        {
                            byte[] buffer = deserialize as byte[];
                            if (buffer != null && buffer.Length > 0)
                            {
                                map = new Dictionary<string, AssetInfo>();
                                string json = Encoding.Default.GetString(buffer);
                                var assetsInfo = JsonUtility.FromJson<AssetsInfo>(json);

                                int counter = 0;
                                for (int i = 0; i < assetsInfo.list.Count; i++)
                                {
                                    var info = assetsInfo.list[i];
                                    info.name = Path.GetFileNameWithoutExtension(info.path);
                                    map.Add(info.path, info);
                                    if (++counter == 100)
                                    {
                                        counter = 0;
                                        yield return null;
                                    }
                                }
                                isMapLoading = false;
                                Main.Log.Info("成功加载资源信息");
                            }
                        }
                    }
                }
                else
                {
                    Main.Log.Error("请求下载map.dat失败：{0} {1}", request.url, request.error);
                }
            }
        }

        private IEnumerator LoadAssetBundleManifestAsync()
        {
            using (UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle((mode == MODE.REALITY ? assetBundleUrl : Application.streamingAssetsPath) + "/" + assetBundleManifestName))
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
                        assetBundleManifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                        isAssetBundleManifestLoading = false;
                    }
                    else
                    {
                        Main.Log.Error("下载AssetBundleManifest失败：{0}", request.url);
                    }
                }
                else
                {
                    Main.Log.Error("请求下载AssetBundleManifest失败：{0} {1}", request.url, request.error);
                }
            }
        }

        private IEnumerator LoadAssetBundleAsync(string assetBundleName, Action<float> onLoading = null)
        {
            DateTime beginTime = DateTime.Now;

            if (loadingDic.TryGetValue(assetBundleName, out var target))
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
                yield return new WaitUntil(() => !loadingDic.ContainsKey(assetBundleName));
            }
            else
            {
                using (UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle((mode == MODE.REALITY ? assetBundleUrl : Application.streamingAssetsPath) + "/" + assetBundleName))
                {
                    loadingDic.Add(assetBundleName, request);
#if UNITY_2017_2_OR_NEWER
                    yield return request.SendWebRequest();
#else
                    yield return request.Send();
#endif
                    while (!request.isDone)
                    {
                        onLoading?.Invoke(request.downloadProgress);
                        yield return null;
                    }

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
                            assetBundlesDic.Add(assetBundleName, ab);
                            Main.Log.Info("于{0}发起下载AssetBundle请求 {1} 于{2}下载完成 耗时{3}毫秒（{4}秒）",
                                beginTime.ToString("T"), request.url, DateTime.Now.ToString("T"),
                                (DateTime.Now - beginTime).TotalMilliseconds, (DateTime.Now - beginTime).TotalSeconds);
                        }
                        else
                        {
                            Main.Log.Error("下载AssetBundle失败：{0}", request.url);
                        }
                    }
                    else
                    {
                        Main.Log.Error("请求下载AssetBundle失败：{0} {1}", request.url, request.error);
                    }
                    yield return null;
                    loadingDic.Remove(assetBundleName);
                }
            }
        }

        private IEnumerator LoadAssetBundleDependeciesAsync(string assetBundleName)
        {
            if (assetBundleManifest == null)
            {
                if (isAssetBundleManifestLoading)
                {
                    yield return new WaitUntil(() => assetBundleManifest != null);
                }
                else
                {
                    isAssetBundleManifestLoading = true;
                    yield return LoadAssetBundleManifestAsync();
                }
            }

            string[] dependencies = assetBundleManifest.GetAllDependencies(assetBundleName);
            for (int i = 0; i < dependencies.Length; i++)
            {
                string dep = dependencies[i];
                if (!assetBundlesDic.ContainsKey(dep))
                {
                    yield return LoadAssetBundleAsync(dep);
                }
            }
        }

        private IEnumerator LoadAssetAsyncCoroutine<T>(string assetPath, Action<bool, T> onCompleted, Action<float> onLoading) where T : Object
        {
            Object asset = null;

#if UNITY_EDITOR
            if (mode == MODE.EDITOR)
            {
                onLoading?.Invoke(1);
                yield return null;

                asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset == null)
                {
                    Main.Log.Error("加载资源失败：{0}", assetPath);
                }
            }
            else
            {
                if (isMapLoading)
                {
                    yield return new WaitUntil(() => isMapLoading == false);
                }

                if (!map.TryGetValue(assetPath, out var assetInfo))
                {
                    Main.Log.Error("加载资源失败：{0}", assetPath);
                    yield break;
                } 

                yield return LoadAssetBundleDependeciesAsync(assetInfo.abName);

                if (!assetBundlesDic.ContainsKey(assetInfo.abName))
                {
                    yield return LoadAssetBundleAsync(assetInfo.abName, onLoading);
                }
                else
                {
                    onLoading?.Invoke(1);
                    yield return null;
                }
                asset = assetBundlesDic[assetInfo.abName].LoadAsset<T>(assetInfo.name);
                if (asset == null)
                {
                    Main.Log.Error("加载资源失败：{0} {1}", assetInfo.abName, asset.name);
                }
            }
#else
            if (isMapLoading)
            {
                yield return new WaitUntil(() => isMapLoading == false);
            }

            if (!map.TryGetValue(assetPath, out var assetInfo))
            {
                Main.Log.Error("加载资源失败：{0}", assetPath);
                yield break;
            }

            yield return LoadAssetBundleDependeciesAsync(assetInfo.abName);

            if (!assetBundlesDic.ContainsKey(assetInfo.abName))
            {
                yield return LoadAssetBundleAsync(assetInfo.abName, onLoading);
            }
            else
            {
                onLoading?.Invoke(1);
                yield return null;
            }
            asset = assetBundlesDic[assetInfo.abName].LoadAsset<T>(assetInfo.name);
            if (asset == null)
            {
                Main.Log.Error("加载资源失败：{0} {1}", assetInfo.abName, asset.name);
            }

#endif
            if (asset != null)
            {
                onCompleted?.Invoke(true, asset as T);
            }
            else
            {
                onCompleted?.Invoke(false, null);
            }
        }

        private IEnumerator LoadSceneAsyncCoroutine(string sceneAssetPath, Action<bool> onCompleted, Action<float> onLoading)
        {
#if UNITY_EDITOR
            if (mode == MODE.EDITOR)
            {
                if (sceneDic.ContainsKey(sceneAssetPath))
                {
                    Main.Log.Warning("场景{0}已加载", sceneAssetPath);
                    onCompleted?.Invoke(false);
                    yield break;
                }

                sceneDic.Add(sceneAssetPath, new Scene());
                AsyncOperation asyncOperation = EditorSceneManager.LoadSceneAsyncInPlayMode(sceneAssetPath, new LoadSceneParameters()
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
                sceneDic[sceneAssetPath] = scene;
            }
            else
            {
                if (isMapLoading)
                {
                    yield return new WaitUntil(() => isMapLoading == false);
                }
                if (!map.TryGetValue(sceneAssetPath, out var assetInfo))
                {
                    Main.Log.Error("加载场景失败：{0}", sceneAssetPath);
                    yield break;
                }
                if (sceneDic.ContainsKey(assetInfo.name))
                {
                    Main.Log.Warning("场景{0}已加载", sceneAssetPath);
                    onCompleted?.Invoke(false);
                    yield break;
                }

                yield return LoadAssetBundleDependeciesAsync(assetInfo.abName);

                Scene scene = SceneManager.GetSceneByPath(sceneAssetPath);
                sceneDic.Add(assetInfo.name, scene);
                if (!assetBundlesDic.ContainsKey(assetInfo.abName))
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
#else
            if (isMapLoading)
            {
                yield return new WaitUntil(() => isMapLoading == false);
            }
            if (!map.TryGetValue(sceneAssetPath, out var assetInfo))
            {
                Main.Log.Error("加载场景失败：{0}", sceneAssetPath);
                yield break;
            }
            if (sceneDic.ContainsKey(assetInfo.name))
            {
                Main.Log.Warning("场景{0}已加载", sceneAssetPath);
                onCompleted?.Invoke(false);
                yield break;
            }

            yield return LoadAssetBundleDependeciesAsync(assetInfo.abName);

            Scene scene = SceneManager.GetSceneByPath(sceneAssetPath);
            sceneDic.Add(assetInfo.name, scene);
            if (!assetBundlesDic.ContainsKey(assetInfo.abName))
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
#endif
            onCompleted?.Invoke(true);
        }

        /// <summary>
        /// 异步加载资产
        /// </summary>
        /// <typeparam name="T">资产类型</typeparam>
        /// <param name="assetPath">资产路径</param>
        /// <param name="onCompleted">加载完成回调事件</param>
        /// <param name="onLoading">加载进度回调事件</param>
        public void LoadAssetAsync<T>(string assetPath, Action<bool, T> onCompleted = null, Action<float> onLoading = null) where T : Object
        {
            StartCoroutine(LoadAssetAsyncCoroutine(assetPath, onCompleted, onLoading));
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneAssetPath">场景资产路径</param>
        /// <param name="onCompleted">加载完成回调事件</param>
        /// <param name="onLoading">加载过程回调事件</param>
        public void LoadSceneAsync(string sceneAssetPath, Action<bool> onCompleted = null, Action<float> onLoading = null)
        {
            StartCoroutine(LoadSceneAsyncCoroutine(sceneAssetPath, onCompleted, onLoading));
        }

        /// <summary>
        /// 卸载资产
        /// </summary>
        /// <param name="assetPath">资产路径</param>
        /// <param name="unloadAllLoadedObjects">是否卸载相关实例化对象</param>
        public void UnloadAsset(string assetPath, bool unloadAllLoadedObjects = false)
        {
            if (map.TryGetValue(assetPath, out var assetInfo))
            {
                if (assetBundlesDic.ContainsKey(assetInfo.abName))
                {
                    assetBundlesDic[assetInfo.abName].Unload(unloadAllLoadedObjects);
                    assetBundlesDic.Remove(assetInfo.abName);
                }
            }
        }

        /// <summary>
        /// 卸载所有资产
        /// </summary>
        /// <param name="unloadAllLoadedObjects">是否卸载相关实例化对象</param>
        public void UnloadAllAsset(bool unloadAllLoadedObjects = false)
        {
            foreach (var kv in assetBundlesDic)
            {
                kv.Value.Unload(unloadAllLoadedObjects);
            }
            assetBundlesDic.Clear();
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
            if (mode == MODE.EDITOR)
            {
                if (sceneDic.TryGetValue(sceneAssetPath, out Scene scene))
                {
                    sceneDic.Remove(sceneAssetPath);
                    EditorSceneManager.UnloadSceneAsync(scene);
                    return true;
                }
                return false;
            }
            else
            {
                if (map.TryGetValue(sceneAssetPath, out var assetInfo))
                {
                    if (sceneDic.ContainsKey(assetInfo.name))
                    {
                        sceneDic.Remove(assetInfo.name);
                        SceneManager.UnloadSceneAsync(assetInfo.name);
                        return true;
                    }
                }
                return false;
            }
#else
            if (map.TryGetValue(sceneAssetPath, out var assetInfo))
            {
                if (sceneDic.ContainsKey(assetInfo.name))
                {
                    sceneDic.Remove(assetInfo.name);
                    SceneManager.UnloadSceneAsync(assetInfo.name);
                    return true;
                }
            }
            return false;
#endif
        }

        /// <summary>
        /// 异步实例化
        /// </summary>
        /// <typeparam name="T">资产类型</typeparam>
        /// <param name="assetPath">资产路径</param>
        /// <param name="onCompleted">结果回调事件</param>
        /// <param name="onProgress">进度回调事件</param>
        public void InstantiateAsync<T>(string assetPath, Action<bool, T> onCompleted = null, Action<float> onProgress = null) where T : Object
        {
            //异步加载资产
            StartCoroutine(LoadAssetAsyncCoroutine<T>(assetPath, (success, asset) =>
            {
                //资产加载成功
                if (success)
                {
                    //实例化
                    T t = Instantiate(asset);
                    //获取或创建引用 并申请引用
                    Main.Refer.GetOrCreate<ResourceReference>(assetPath).Apply(t);
                    //执行回调
                    onCompleted?.Invoke(true, t);
                }
                //资产加载失败
                else
                {
                    //执行回调
                    onCompleted?.Invoke(false, null);
                }
            }, onProgress));
        }
    }
}