/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

namespace SK.Framework.Resource
{
    [CreateAssetMenu(menuName = "SKFramework/Resource/Sample Resource Agent")]
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
                        var cachePath = Path.Combine(Application.persistentDataPath, m_AssetBundleCache, "map.json");
                        if (File.Exists(cachePath))
                        {
                            using (var request2 = UnityWebRequest.Get(cachePath))
                            {
#if UNITY_2017_2_OR_NEWER
                                yield return request2.SendWebRequest();
#else
                                yield return request2.Send();
#endif
#if UNITY_2020_2_OR_NEWER
                                flag = request2.result == UnityWebRequest.Result.Success;
#elif UNITY_2017_1_OR_NEWER
                                flag = !(request2.isNetworkError || request2.isHttpError);
#else
                                flag = !request2.isError;
#endif
                                if (flag)
                                {
                                    var assetsInfoCache = JsonUtility.FromJson<AssetsInfo>(request2.downloadHandler.text);
                                    if (assetsInfoCache == null || assetsInfoCache.version == assetsInfo.version
                                        || MD5Utility.CalculateBytesMD5(request.downloadHandler.data) != MD5Utility.CalculateBytesMD5(request2.downloadHandler.data))
                                    {
                                        yield return CleanInvalidAssetBundleCache(m_AssetBundles, assetsInfoCache.assetBundles);
                                        yield return IOUtility.WriteBytesAsync(cachePath, request.downloadHandler.data, succeed =>
                                        {
                                            if (!succeed)
                                                m_Logger.Error("[Resource] Write to cache {0} failed.", cachePath);
                                        });
                                    }
                                }
                            }
                        }
                        else
                        {
                            yield return IOUtility.WriteBytesAsync(cachePath, request.downloadHandler.data, succeed =>
                            {
                                if (!succeed)
                                    m_Logger.Error("[Resource] Write to cache {0} failed.", cachePath);
                            });
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

        private IEnumerator CleanInvalidAssetBundleCache(Dictionary<string, AssetBundleInfo> dic, List<AssetBundleInfo> cache)
        {
            if (cache == null || cache.Count == 0)
                yield break;
            for (int i = 0; i < cache.Count; i++)
            {
                var cachePath = Path.Combine(Application.persistentDataPath, m_AssetBundleCache, cache[i].name);
                if (!dic.TryGetValue(cache[i].name, out var target) || cache[i].md5 != target.md5)
                {
                    if (File.Exists(cachePath))
                    {
                        try
                        {
                            File.Delete(cachePath);
                            m_Logger.Info("[Resource] Delete cache: {0}", cachePath);
                        }
                        catch (Exception e)
                        {
                            m_Logger.Error("[Resource] Delete cache {0} failed: {1}", cachePath, e.Message);
                        }
                    }
                }
                yield return null;
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
            bool loadFromCache = m_Mode == MODE.REALITY && File.Exists(cachePath);
            if (loadFromCache && m_AssetBundles.TryGetValue(assetBundleName, out var assetBundle)
                && (new FileInfo(cachePath).Length != assetBundle.size
                    || assetBundle.md5 != MD5Utility.CalculateFileMD5(cachePath)))
            {
                loadFromCache = false;
                File.Delete(cachePath);
            }
            string url = loadFromCache ? cachePath : (m_FullAssetBundleUrl + "/" + assetBundleName);
            using (var request = UnityWebRequest.Get(url))
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
                        byte[] decryptBytes = new byte[request.downloadHandler.data.Length];
                        Buffer.BlockCopy(request.downloadHandler.data, 0, decryptBytes, 0, decryptBytes.Length);
                        m_Strategy.Decrypt(ref decryptBytes, m_SecretKey);
                        AssetBundleCreateRequest createRequest = AssetBundle.LoadFromMemoryAsync(decryptBytes);
                        yield return createRequest;
                        ab = createRequest.assetBundle;
                    }
                    else
                    {
                        AssetBundleCreateRequest createRequest = AssetBundle.LoadFromMemoryAsync(request.downloadHandler.data);
                        yield return createRequest;
                        ab = createRequest.assetBundle;
                    }

                    if (ab)
                    {
                        m_AssetBundlesDic.Add(assetBundleName, ab);
                        m_Logger.Info("[Resource] The request for downloading the asset bundle {0} was sent at time {1}," +
                            "completed at time {2},taking {3} milliseconds({4} seconds).", request.url, beginTime.ToString("T"),
                            DateTime.Now.ToString("T"), (DateTime.Now - beginTime).TotalMilliseconds,
                            (DateTime.Now - beginTime).TotalSeconds);
                        if (m_Mode == MODE.REALITY && !loadFromCache)
                        {
                            yield return IOUtility.WriteBytesAsync(cachePath, request.downloadHandler.data, succeed =>
                            {
                                if (succeed)
                                    m_Logger.Info("[Resource] Save the asset bundle {0} to cache {1} succeed.", assetBundleName, cachePath);
                                else
                                    m_Logger.Warning("[Resource] Save the asset bundle {0} to cache {1} failed.", assetBundleName, cachePath);
                            });
                        }
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