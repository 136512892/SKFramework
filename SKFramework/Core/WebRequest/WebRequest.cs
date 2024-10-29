/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

namespace SK.Framework.Networking
{
    public class WebRequest : ModuleBase
    {
        /// <summary>
        /// 发起网络请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="webRequestType">请求类型 GET/POST请求</param>
        /// <param name="headers">请求头</param>
        /// <param name="onSuccess">请求成功回调事件</param>
        /// <param name="onFailure">请求失败回调事件</param>
        public void Send(string url, WebRequestType webRequestType, Dictionary<string, string> headers = null, 
            Action<DownloadHandler> onSuccess = null, Action<string> onFailure = null)
        {
            StartCoroutine(SendWebRequestCoroutine(url,
                WebRequestData.Allocate(webRequestType, headers), onSuccess, onFailure));
        }

        /// <summary>
        /// 发起网络请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="webRequestType">请求类型 GET/POST请求</param>
        /// <param name="contentType">CONTENT-TYPE</param>
        /// <param name="headers">请求头</param>
        /// <param name="onSuccess">请求成功回调事件</param>
        /// <param name="onFailure">请求失败回调事件</param>
        public void Send(string url, WebRequestType webRequestType, ContentType contentType, 
            Dictionary<string, string> headers = null, Action<DownloadHandler> onSuccess = null, Action<string> onFailure = null)
        {
            StartCoroutine(SendWebRequestCoroutine(url,
                WebRequestData.Allocate(webRequestType, contentType, headers), onSuccess, onFailure));
        }

        /// <summary>
        /// 发起网络请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="webRequestType">请求类型 GET/POST请求</param>
        /// <param name="contentType">CONTENT-TYPE</param>
        /// <param name="postData">上传数据</param>
        /// <param name="headers">请求头</param>
        /// <param name="onSuccess">请求成功回调事件</param>
        /// <param name="onFailure">请求失败回调事件</param>
        public void Send(string url, WebRequestType webRequestType, ContentType contentType, byte[] postData, 
            Dictionary<string, string> headers = null, Action<DownloadHandler> onSuccess = null, Action<string> onFailure = null)
        {
            StartCoroutine(SendWebRequestCoroutine(url,
                WebRequestData.Allocate(webRequestType, postData, contentType, headers), onSuccess, onFailure));
        }

        /// <summary>
        /// 发起网络请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="webRequestType">请求类型 GET/POST请求</param>
        /// <param name="contentType">CONTENT-TYPE</param>
        /// <param name="form">表单</param>
        /// <param name="headers">请求头</param>
        /// <param name="onSuccess">请求成功回调事件</param>
        /// <param name="onFailure">请求失败回调事件</param>
        public void Send(string url, WebRequestType webRequestType, ContentType contentType, WWWForm form, 
            Dictionary<string, string> headers = null, Action<DownloadHandler> onSuccess = null, Action<string> onFailure = null)
        {
            StartCoroutine(SendWebRequestCoroutine(url, 
                WebRequestData.Allocate(webRequestType, form, contentType, headers), onSuccess, onFailure));
        }

        /// <summary>
        /// 发起网络请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">网络请求数据</param>
        /// <param name="onSuccess">请求成功回调事件</param>
        /// <param name="onFailure">请求失败回调事件</param>
        public void Send(string url, WebRequestData data, Action<DownloadHandler> onSuccess = null, Action<string> onFailure = null)
        {
            StartCoroutine(SendWebRequestCoroutine(url, 
                data, onSuccess, onFailure));
        }
        
        public string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = Encoding.UTF8.GetBytes(str);
            for (int i = 0; i < byStr.Length; i++)
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            return sb.ToString();
        }

        private IEnumerator SendWebRequestCoroutine(string url, WebRequestData data,
            Action<DownloadHandler> onSuccess, Action<string> onFailure)
        {
            using (UnityWebRequest request = data.requestType == WebRequestType.GET
                ? UnityWebRequest.Get(url)
                : data.wwwForm == null
                ? UnityWebRequest.Post(url, UnityWebRequest.kHttpVerbPOST)
                : UnityWebRequest.Post(url, data.wwwForm))
            {
                if (data.postData != null)
                {
                    request.uploadHandler = new UploadHandlerRaw(data.postData);
                    request.downloadHandler = new DownloadHandlerBuffer();
                }
                if (data.headers != null && data.headers.Count > 0)
                {
                    foreach (var kv in data.headers)
                    {
                        request.SetRequestHeader(kv.Key, kv.Value);
                    }
                }
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
                    onSuccess?.Invoke(request.downloadHandler);
                }
                else
                {
                    onFailure?.Invoke(request.error);
                }
            }
        }
    }
}