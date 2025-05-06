/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Collections.Generic;

using UnityEngine;

namespace SK.Framework.Networking
{
    [CreateAssetMenu(menuName = "SKFramework/WebRequest/Profile")]
    public class WebRequestProfile : ScriptableObject
    {
        public string server;
        public List<WebRequestAPI> APIList = new List<WebRequestAPI>(0);

        public void Send(string apiName, Action<WebRequestResponse> onSuccess = null, 
            Action<WebRequestResponse> onError = null, Dictionary<string, string> headers = null, 
            string postData = null)
        {
            int index = APIList.FindIndex(m => m.name == apiName);
            if (index != -1)
            {
                var api = APIList[index];
                SKFramework.Module<WebRequest>().Send(server + api.url, api.method, 
                    onSuccess, onError, headers, postData, api.priority);
            }
        }
    }

    [Serializable]
    public class WebRequestAPI
    {
        public string name;
        public string url;
        public WebRequestMethod method;
        public int priority;
    }
}