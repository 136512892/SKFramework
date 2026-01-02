/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

namespace SK.Framework.Networking
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/SKFramework.WebRequest")]
    public class WebRequest : ModuleBase
    {
        [SerializeField] private string m_WebRequestAgentName;
        [SerializeField] private int m_MaxWebRequestAgentCount = 4;
        [SerializeField] private int m_Timeout = 30;
        [SerializeField] private int m_MaxRetries = 2;
        private List<IWebRequestAgent> m_Agents;
        private List<WebRequestTask> m_TaskList;
        public WebRequestCache Cache { get; private set; }
        public WebRequestMonitor Monitor { get; private set; }

        protected internal override void OnInitialization()
        {
            base.OnInitialization();

            m_Agents = new List<IWebRequestAgent>(m_MaxWebRequestAgentCount);
            m_TaskList = new List<WebRequestTask>();
            Cache = new WebRequestCache();
            Monitor = new WebRequestMonitor();
            Type webRequestAgentType = Type.GetType(m_WebRequestAgentName, true);
            for (int i = 0; i < m_MaxWebRequestAgentCount; i++)
            {
                var agent = new GameObject(string.Format("{0}-{1}", 
                    nameof(WebRequestAgent), i + 1)).AddComponent(webRequestAgentType);
                agent.transform.SetParent(transform);
                m_Agents.Add(agent as IWebRequestAgent);
            }
        }

        protected internal override void OnUpdate()
        {
            base.OnUpdate();
            if (m_TaskList.Count > 0)
            {
                int index = m_Agents.FindIndex(m => m.isIdle);
                if (index != -1)
                {
                    var agent = m_Agents[index];
                    m_TaskList = m_TaskList.OrderBy(m => m.priority).ToList();
                    var task = m_TaskList.First();
                    m_TaskList.Remove(task);
                    agent.Send(task, m_MaxRetries, m_Timeout);
                    SKFramework.Module<ObjectPool.ObjectPool>().Get<WebRequestTask>().Recycle(task);
                }
            }
        }

        protected internal override void OnTermination()
        {
            base.OnTermination();
            m_Agents.Clear();
            m_Agents = null;
            m_TaskList.Clear();
            m_TaskList = null;
            Cache = null;
            Monitor = null;
        }

        public void Send(string url, WebRequestMethod method)
        {
            var task = WebRequestTask.Allocate(url, method, null, null, null, null, 0);
            m_TaskList.Add(task);
        }

        public void Send(string url, WebRequestMethod method, Action<WebRequestResponse> onSuccess)
        {
            var task = WebRequestTask.Allocate(url, method, onSuccess, null, null, null, 0);
            m_TaskList.Add(task);
        }

        public void Send(string url, WebRequestMethod method, Action<WebRequestResponse> onSuccess, Action<WebRequestResponse> onError)
        {
            var task = WebRequestTask.Allocate(url, method, onSuccess, onError, null, null, 0);
            m_TaskList.Add(task);
        }

        public void Send(string url, WebRequestMethod method, Action<WebRequestResponse> onSuccess, Action<WebRequestResponse> onError,
            Dictionary<string, string> header)
        {
            var task = WebRequestTask.Allocate(url, method, onSuccess, onError, header, null, 0);
            m_TaskList.Add(task);
        }

        public void Send(string url, WebRequestMethod method, Action<WebRequestResponse> onSuccess,
            Action<WebRequestResponse> onError,
            Dictionary<string, string> header, string postData)
        {
            var task = WebRequestTask.Allocate(url, method, onSuccess, onError, header, postData, 0);
            m_TaskList.Add(task);
        }

        public void Send(string url, WebRequestMethod method, Action<WebRequestResponse> onSuccess,
            Action<WebRequestResponse> onError, string postData)
        {
            var task = WebRequestTask.Allocate(url, method, onSuccess, onError, null, postData, 0);
            m_TaskList.Add(task);
        }

        public void Send(string url, WebRequestMethod method, Action<WebRequestResponse> onSuccess, string postData)
        {
            var task = WebRequestTask.Allocate(url, method, onSuccess, null, null, postData, 0);
            m_TaskList.Add(task);
        }
        
        public void Send(string url, WebRequestMethod method, Action<WebRequestResponse> onSuccess,
            Action<WebRequestResponse> onError, Dictionary<string, string> headers,
            string postData, int priority)
        {
            var task = WebRequestTask.Allocate(url, method, onSuccess, onError, headers, postData, priority);
            m_TaskList.Add(task);
        }
    }
}