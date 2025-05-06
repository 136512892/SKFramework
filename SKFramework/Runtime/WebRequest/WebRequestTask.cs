/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using SK.Framework.ObjectPool;
using System.Collections.Generic;

namespace SK.Framework.Networking
{
    public class WebRequestTask : IPoolable
    {
        private bool m_IsRecycled;
        private DateTime m_EntryTime;

        bool IPoolable.isRecycled
        {
            get => m_IsRecycled;
            set => m_IsRecycled = value;
        }

        DateTime IPoolable.entryTime
        {
            get => m_EntryTime;
            set => m_EntryTime = value;
        }

        public string url { get; private set; }
        public WebRequestMethod method { get; private set; }
        public Action<WebRequestResponse> onSuccess { get; private set; }
        public Action<WebRequestResponse> onError { get; private set; }
        public Dictionary<string, string> headers { get; private set; }
        public string postData { get; private set; }
        public int priority { get; private set; }

        public static WebRequestTask Allocate(string url, WebRequestMethod method, Action<WebRequestResponse> onSuccess,
            Action<WebRequestResponse> onError, Dictionary<string, string> headers = null, 
            string postData = null, int priority = 0)
        {
            var instance = SKFramework.Module<ObjectPool.ObjectPool>().Get<WebRequestTask>().Allocate();
            instance.url = url;
            instance.method = method;
            instance.onSuccess = onSuccess;
            instance.onError = onError;
            instance.headers = headers;
            instance.postData = postData;
            instance.priority = priority;
            return instance;
        }
        
        public void OnRecycled()
        {
            url = null;
            onSuccess = null;
            onError = null;
            headers = null;
            postData = null;
        }
    }
}