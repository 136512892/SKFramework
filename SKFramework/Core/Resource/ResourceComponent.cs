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

        private bool isAssetBundleManifestLoading;

        private readonly Dictionary<string, AssetBundle> assetBundlesDic = new Dictionary<string, AssetBundle>();

        private readonly Dictionary<string, Scene> sceneDic = new Dictionary<string, Scene>();

        private readonly Dictionary<string, UnityWebRequest> loadingDic = new Dictionary<string, UnityWebRequest>();

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

            if (loadingDic.ContainsKey(assetBundleName))
            {
                UnityWebRequest request = loadingDic[assetBundleName];
                while (!request.isDone)
                {
                    onLoading?.Invoke(request.downloadProgress);
                    yield return null;
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
                            Main.Log.Info("于{0}发起下载AssetBundle请求 {1} 于{2}下载完成 耗时{3}毫秒", beginTime.ToString("hh:mm:fff"), request.url, DateTime.Now.ToString("hh:mm:fff"), (DateTime.Now - beginTime).TotalMilliseconds);
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

        private IEnumerator LoadAssetAsyncCoroutine<T>(string assetName, string assetPath, string assetBundleName, Action<float> onLoading, Action<bool, T> onCompleted) where T : Object
        {
            if (!isEditorMode) 
                yield return LoadAssetBundleDependeciesAsync(assetBundleName);

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
                if (!assetBundlesDic.ContainsKey(assetBundleName))
                {
                    yield return LoadAssetBundleAsync(assetBundleName, onLoading);
                }
                else
                {
                    onLoading?.Invoke(1);
                    yield return null;
                }
                asset = assetBundlesDic[assetBundleName].LoadAsset<T>(assetName);
                if (asset == null)
                {
                    Main.Log.Error("加载资源失败：{0} {1}", assetBundleName, assetName);
                }
            }
#else
            if (!assetBundlesDic.ContainsKey(assetBundleName))
            {
                yield return LoadAssetBundleAsync(assetBundleName, onLoading);
            }
            else
            {
                onLoading?.Invoke(1);
                yield return null;
            }
            asset = assetBundlesDic[assetBundleName].LoadAsset<T>(assetName);
            if (asset == null)
            {
                Main.Log.Error("加载资源失败：{0} {1}", assetBundleName, assetName);
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

        private IEnumerator LoadSceneAsyncCoroutine(string sceneName, string assetPath, string assetBundleName, Action<float> onLoading, Action onCompleted)
        {
            if (sceneDic.ContainsKey(sceneName))
            {
                Main.Log.Warning("加载场景{0}失败：已加载", sceneName);
                yield break;
            }

            if (!isEditorMode)
                yield return LoadAssetBundleDependeciesAsync(assetBundleName);

#if UNITY_EDITOR
            if (isEditorMode)
            {
                Scene scene = SceneManager.GetSceneByPath(assetPath);
                sceneDic.Add(sceneName, scene);
                AsyncOperation asyncOperation = EditorSceneManager.LoadSceneAsyncInPlayMode(assetPath, new LoadSceneParameters()
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
                Scene scene = SceneManager.GetSceneByPath(assetPath);
                sceneDic.Add(sceneName, scene);
                yield return LoadAssetBundleAsync(assetBundleName, onLoading);
                AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                while (!asyncOperation.isDone)
                {
                    onLoading?.Invoke(asyncOperation.progress);
                    yield return null;
                }
                onLoading?.Invoke(1f);
            }
#else
            Scene scene = SceneManager.GetSceneByPath(assetPath);
            sceneDic.Add(sceneName, scene);
            yield return LoadAssetBundleAsync(assetBundleName, onLoading);
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (!asyncOperation.isDone)
            {
                onLoading?.Invoke(asyncOperation.progress);
                yield return null;
            }
            onLoading?.Invoke(1f);
#endif
            onCompleted?.Invoke();
        }   

        public Coroutine LoadAssetAsync<T>(AssetInfo assetInfo, Action<float> onLoading = null, Action<bool, T> onCompleted = null) where T : Object
        {
            return StartCoroutine(LoadAssetAsyncCoroutine(assetInfo.AssetName, assetInfo.AssetPath, assetInfo.AssetBundleName, onLoading, onCompleted));
        }

        public Coroutine LoadAssetAsync<T>(MonoBehaviour executer, AssetInfo assetInfo, Action<float> onLoading = null, Action<bool, T> onCompleted = null) where T : Object
        {
            return executer.StartCoroutine(LoadAssetAsyncCoroutine(assetInfo.AssetName, assetInfo.AssetPath, assetInfo.AssetBundleName, onLoading, onCompleted));
        }

        public Coroutine LoadSceneAsync(SceneInfo sceneInfo, Action<float> onLoading = null, Action onCompleted = null)
        {
            return StartCoroutine(LoadSceneAsyncCoroutine(sceneInfo.SceneName, sceneInfo.AssetPath, sceneInfo.AssetBundleName, onLoading, onCompleted));
        }

        public Coroutine LoadSceneAsync(MonoBehaviour executer, SceneInfo sceneInfo, Action<float> onLoading = null, Action onCompleted = null)
        {
            return executer.StartCoroutine(LoadSceneAsyncCoroutine(sceneInfo.SceneName, sceneInfo.AssetPath, sceneInfo.AssetBundleName, onLoading, onCompleted));
        }

        public void UnloadAsset(AssetInfo assetInfo, bool unloadAllLoadedObjects = false)
        {
            if (assetBundlesDic.ContainsKey(assetInfo.AssetBundleName))
            {
                assetBundlesDic[assetInfo.AssetBundleName].Unload(unloadAllLoadedObjects);
                assetBundlesDic.Remove(assetInfo.AssetBundleName);
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

        public bool UnloadScene(SceneInfo sceneInfo)
        {
            if (sceneDic.ContainsKey(sceneInfo.SceneName))
            {
                sceneDic.Remove(sceneInfo.SceneName);
                SceneManager.UnloadSceneAsync(sceneInfo.SceneName);
                return true;
            }
            return false;
        }
    }
}