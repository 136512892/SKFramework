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
    public class ResourceComponent : MonoBehaviour
    {
        [SerializeField] private bool isEditorMode;

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
            if (!isEditorMode)
                StartCoroutine(LoadAssetsMapAsync());
        }

        private IEnumerator LoadAssetsMapAsync()
        {
            string url = assetBundleUrl + "/map.dat";
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
            using (UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(assetBundleUrl + "/" + assetBundleManifestName))
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
                using (UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(assetBundleUrl + "/" + assetBundleName))
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

        private IEnumerator LoadAssetAsyncCoroutine<T>(string assetPath, Action<float> onLoading, Action<bool, T> onCompleted) where T : Object
        {
            Object asset = null;

#if UNITY_EDITOR
            if (isEditorMode)
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
            if (map == null)
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

        private IEnumerator LoadSceneAsyncCoroutine(string sceneAssetPath, Action<float> onLoading, Action<bool> onCompleted)
        {
            if (sceneDic.ContainsKey(sceneAssetPath))
            {
                Main.Log.Warning("加载场景{0}失败：已加载", sceneAssetPath);
                onCompleted?.Invoke(false);
                yield break;
            }

#if UNITY_EDITOR
            if (isEditorMode)
            {
                Scene scene = SceneManager.GetSceneByPath(sceneAssetPath);
                sceneDic.Add(sceneAssetPath, scene);
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
                yield return LoadAssetBundleDependeciesAsync(assetInfo.abName);

                Scene scene = SceneManager.GetSceneByPath(sceneAssetPath);
                sceneDic.Add(assetInfo.name, scene);
                yield return LoadAssetBundleAsync(assetInfo.abName, onLoading);
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
            yield return LoadAssetBundleDependeciesAsync(assetInfo.abName);

            Scene scene = SceneManager.GetSceneByPath(sceneAssetPath);
            sceneDic.Add(assetInfo.name, scene);
            yield return LoadAssetBundleAsync(assetInfo.abName, onLoading);
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

        public void LoadAssetAsync<T>(string assetPath, Action<float> onLoading = null, Action<bool, T> onCompleted = null) where T : Object
        {
            StartCoroutine(LoadAssetAsyncCoroutine(assetPath, onLoading, onCompleted));
        }

        public void LoadAssetAsync<T>(MonoBehaviour executer, string assetPath, Action<float> onLoading = null, Action<bool, T> onCompleted = null) where T : Object
        {
            executer.StartCoroutine(LoadAssetAsyncCoroutine(assetPath, onLoading, onCompleted));
        }

        public void LoadSceneAsync(string sceneAssetPath, Action<float> onLoading = null, Action<bool> onCompleted = null)
        {
            StartCoroutine(LoadSceneAsyncCoroutine(sceneAssetPath, onLoading, onCompleted));
        }

        public void LoadSceneAsync(MonoBehaviour executer, string sceneAssetPath, Action<float> onLoading = null, Action<bool> onCompleted = null)
        {
            executer.StartCoroutine(LoadSceneAsyncCoroutine(sceneAssetPath, onLoading, onCompleted));
        }

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

        public void UnloadAllAsset(bool unloadAllLoadedObjects = false)
        {
            foreach (var kv in assetBundlesDic)
            {
                kv.Value.Unload(unloadAllLoadedObjects);
            }
            assetBundlesDic.Clear();
            AssetBundle.UnloadAllAssetBundles(unloadAllLoadedObjects);
        }

        public bool UnloadScene(string sceneAssetPath)
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

        /// <summary>
        /// Asset资产信息
        /// </summary>
        [Serializable]
        public class AssetInfo
        {
            /// <summary>
            /// Asset资产名称
            /// </summary>
            public string name;

            /// <summary>
            /// 资源路径
            /// </summary>
            public string path;

            /// <summary>
            /// AssetBundle包名称
            /// </summary>
            public string abName;

            public override string ToString()
            {
                return string.Format("AssetName:{0}  AssetBundleName:{1}  Path:{2}", name, abName, path);
            }
        }
        [Serializable]
        public class AssetsInfo
        {
            public List<AssetInfo> list = new List<AssetInfo>();
        }
    }
}