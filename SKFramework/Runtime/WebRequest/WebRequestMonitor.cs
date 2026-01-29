/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace SK.Framework.Networking
{
    public class WebRequestMonitor
    {
        private readonly WebRequestMetrics m_Metrics = new WebRequestMetrics();

        private readonly Dictionary<UnityWebRequest, float> m_RequestStartTimeDic =
            new Dictionary<UnityWebRequest, float>();
        
        public WebRequestMetrics metrics
        {
            get { return m_Metrics; }
        }

        public void TrackStart(UnityWebRequest webRequest)
        {
            m_RequestStartTimeDic.Add(webRequest, Time.realtimeSinceStartup);
        }

        public void TrackEnd(UnityWebRequest webRequest)
        {
            if (m_RequestStartTimeDic.TryGetValue(webRequest, out float startTime))
            {
                float latency = Time.realtimeSinceStartup - startTime;
                if (m_Metrics.recentLatencies.Count == 100)
                    m_Metrics.recentLatencies.RemoveAt(0);
                m_Metrics.recentLatencies.Add(latency);
                m_RequestStartTimeDic.Remove(webRequest);
            }
            
            m_Metrics.totalBytesDownloaded += webRequest.downloadedBytes / 1024f;
            m_Metrics.totalBytesUploaded += webRequest.uploadedBytes / 1024f;

            if (webRequest.result == UnityWebRequest.Result.Success)
                m_Metrics.successCount++;
            else
                m_Metrics.failureCount++;
        }
    }
}