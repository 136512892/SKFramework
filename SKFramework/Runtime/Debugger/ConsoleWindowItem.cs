/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using UnityEngine;
using SK.Framework.ObjectPool;

namespace SK.Framework.Debugger
{
    public class ConsoleWindowItem : IPoolable
    {
        public LogType logType { get; private set; }
        public DateTime logTime { get; private set; }
        public string message { get; private set; }
        public string stackTrace { get; private set; }

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

        public static ConsoleWindowItem Allocate(LogType logType, string message, string stackTrace)
        {
            var instance = SKFramework.Module<ObjectPool.ObjectPool>().Get<ConsoleWindowItem>().Allocate();
            instance.logTime = DateTime.Now;
            instance.logType = logType;
            instance.message = string.Format("[{0}] {1}", DateTime.Now, message);
            instance.stackTrace = string.Format("{0}\r\n{1}", message, stackTrace);
            return instance;
        }

        private void OnRecycled()
        {
            message = null;
            stackTrace = null;
        }

        void IPoolable.OnRecycled() => OnRecycled();
    }
}