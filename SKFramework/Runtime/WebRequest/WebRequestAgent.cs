/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
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
    internal class WebRequestAgent : MonoBehaviour, IWebRequestAgent
    {
        private int m_MaxRetries;
        private int m_Timeout;
        private readonly WebRequestResponse m_Response = new WebRequestResponse();

        bool IWebRequestAgent.isIdle
        {
            get => m_IsIdle;
            set => m_IsIdle = value;
        }

        private bool m_IsIdle = true;
        private WebRequestMonitor m_Monitor;

        private void Awake()
        {
            m_Monitor = SKFramework.Module<WebRequest>().Monitor;
        }

        public void Send(WebRequestTask task, int retryCount, int timeout)
        {
            m_IsIdle = false;
            m_MaxRetries = retryCount;
            m_Timeout = timeout;
            StartCoroutine(SendWebRequest(task.url, task.method, 
                task.onSuccess, task.onError, task.headers, task.postData));
        }

        private IEnumerator SendWebRequest(string url, WebRequestMethod method, Action<WebRequestResponse> onSuccess,
            Action<WebRequestResponse> onError, Dictionary<string, string> headers, string postData)
        {
            int retryCounter = 0;
            while (retryCounter <= m_MaxRetries)
            {
                using (UnityWebRequest request = CreateWebRequest(url, method, headers, postData))
                {
                    m_Monitor.TrackStart(request);
                    yield return request.SendWebRequest();
                    m_Monitor.TrackEnd(request);
                    m_Response.code = request.responseCode;
                    m_Response.data = request.downloadHandler?.text;
                    m_Response.error = request.error;
                    m_Response.bytes = request.downloadHandler?.data;
                    if (request.result == UnityWebRequest.Result.Success)
                    {
                        onSuccess?.Invoke(m_Response);
                        m_IsIdle = true;
                        yield break;
                    }
                    if (RetryCheck(request))
                    {
                        retryCounter++;
                        yield return new WaitForSeconds(1 * retryCounter);
                        continue;
                    }
                    onError?.Invoke(m_Response);
                    m_IsIdle = true;
                    yield break;
                }
            }
        }

        private UnityWebRequest CreateWebRequest(string url, WebRequestMethod method,
            Dictionary<string, string> headers, string postData)
        {
            UnityWebRequest request = new UnityWebRequest(url, method.ToString())
            {
                downloadHandler = new DownloadHandlerBuffer(),
                timeout = m_Timeout,
            };
            switch (method)
            {
                case WebRequestMethod.POST:
                case WebRequestMethod.PUT:
                    request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(postData ?? string.Empty));
                    request.SetRequestHeader("Content-Type", "application/json");
                    break;
            }

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.SetRequestHeader(header.Key, header.Value);
                }
            }
            return request;
        }

        private bool RetryCheck(UnityWebRequest request)
        {
            switch (request.result)
            {
                case UnityWebRequest.Result.ConnectionError: 
                case UnityWebRequest.Result.ProtocolError when request.responseCode >= 500:
                    return true;
                default:
                    return false;
            }
        }
    }
}