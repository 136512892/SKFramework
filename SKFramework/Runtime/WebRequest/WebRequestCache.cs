/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework.Networking
{
    public class WebRequestCache
    {
        private readonly Dictionary<string, Entry> m_Dic = new Dictionary<string, Entry>();

        public void Add(string key, WebRequestResponse response, float expireTime)
        {
            m_Dic.Add(key, new Entry()
            {
                response = new WebRequestResponse().Copy(response),
                expiration = Time.realtimeSinceStartup + expireTime
            });
        }

        public bool TryGet(string key, out WebRequestResponse response)
        {
            if (m_Dic.TryGetValue(key, out Entry entry))
            {
                if (Time.realtimeSinceStartup < entry.expiration)
                {
                    response = entry.response;
                    return true;
                }
                m_Dic.Remove(key);
            }
            response = null;
            return false;
        }
        
        public class Entry
        {
            public WebRequestResponse response;
            public float expiration;
        }
    }
}