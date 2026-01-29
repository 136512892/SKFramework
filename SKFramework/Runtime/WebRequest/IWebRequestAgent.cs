/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

namespace SK.Framework.Networking
{
    public interface IWebRequestAgent
    {
        bool isIdle { get; set; }

        void Send(WebRequestTask task, int retryCount, int timeout);
    }
}