/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.IO;
using System.Collections;

using UnityEngine;
using UnityEngine.Networking;

namespace SK.Framework.Resource
{
    [CreateAssetMenu(menuName = "SKFramework/Resource/Resource Agent（Sample）")]
    public class SampleResourceAgent : ResourceAgent
    {
        protected override IEnumerator LoadAssetsMapAsync()
        {
            string url = m_FullAssetBundleUrl + "/map.json";
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
#if UNITY_2020_2_OR_NEWER
                    bool flag = request.result == UnityWebRequest.Result.Success;
#elif UNITY_2017_1_OR_NEWER
                    bool flag = !(request.isNetworkError || request.isHttpError);
#else
                    bool flag = !request.isError;
#endif
                    if (flag)
                    {
                        var assetsInfo = JsonUtility.FromJson<AssetsInfo>(request.downloadHandler.text);
                        if (assetsInfo != null)
                        {
                            int counter = 0;
                            for (int i = 0; i < assetsInfo.assets.Count; i++)
                            {
                                var info = assetsInfo.assets[i];
                                info.name = Path.GetFileNameWithoutExtension(info.path);
                                m_Assets[info.path] = info;
                                if (++counter == 100)
                                {
                                    counter = 0;
                                    yield return null;
                                }
                            }
                            for (int i = 0; i < assetsInfo.assetBundles.Count; i++)
                            {
                                var info = assetsInfo.assetBundles[i];
                                m_AssetBundles[info.name] = info;
                                yield return null;
                            }
                            m_Logger.Info("[Resource] Version: {0}", assetsInfo.version);
                        }
                        m_IsMapLoading = false;
                    }
                    else
                    {
                        m_Logger.Error("[Resource] Load assets map error: {0}", request.error);
                    }
                }
            }
        }

        protected override IEnumerator LoadAssetBundleAsync(string assetBundleName, Action<float> onLoading = null)
        {
            DateTime beginTime = DateTime.Now;
            if (m_LoadingDic.TryGetValue(assetBundleName, out var target))
            {
                yield return null;
                while (m_LoadingDic.ContainsKey(assetBundleName))
                {
                    onLoading?.Invoke(target != null ? target.downloadProgress : 1f);
                    yield return null;
                }
                yield break;
            }
            var cachePath = Path.Combine(Application.persistentDataPath, m_AssetBundleCache, assetBundleName);
            string url = m_FullAssetBundleUrl + "/" + assetBundleName;
            using (var request = m_Strategy != null ? UnityWebRequest.Get(url) : UnityWebRequestAssetBundle.GetAssetBundle(url))
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

#if UNITY_2020_2_OR_NEWER
                bool flag = request.result == UnityWebRequest.Result.Success;
#elif UNITY_2017_1_OR_NEWER
                bool flag = !(request.isNetworkError || request.isHttpError);
#else
                bool flag = !request.isError;
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
}