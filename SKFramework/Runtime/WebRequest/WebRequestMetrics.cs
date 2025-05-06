/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System.Collections.Generic;

namespace SK.Framework.Networking
{
    public class WebRequestMetrics
    {
        public float totalBytesDownloaded; //kb
        public float totalBytesUploaded; //kb
        public int successCount;
        public int failureCount;
        //retain the delay of the last 100 requests.
        public List<float> recentLatencies = new List<float>(100);

        public float CalculateAverageLatency()
        {
            if (recentLatencies.Count == 0)
                return 0f;
            float sum = 0;
            for (int i = 0; i < recentLatencies.Count; i++)
            {
                sum += recentLatencies[i];
            }
            return sum / recentLatencies.Count;
        }
        
        public override string ToString()
        {
            return string.Format("===WebRequest Metrics===\r\n" +
                                 "Success Rate:{0:P0}\r\n" +
                                 "Average Latency:{1:F2}s\r\n" +
                                 "Total Download:{2:F2} KB\r\n" +
                                 "Total Upload:{3:F2} KB",
                successCount / (float)(successCount + failureCount),
                CalculateAverageLatency(),
                totalBytesDownloaded,
                totalBytesUploaded);
        }
    }
}