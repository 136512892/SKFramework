/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using UnityEngine;

namespace SK.Framework.Debugger
{
    public class ConsoleWindowItem
    {
        private readonly LogType m_LogType;
        private readonly DateTime m_LogTime;
        private readonly string m_Message;
        private readonly string m_StackTrace;

        public LogType logType { get { return m_LogType; } }
        public DateTime logTime { get { return m_LogTime; } }
        public string message { get { return m_Message; } }
        public string stackTrace { get { return m_StackTrace; } }

        public ConsoleWindowItem(LogType logType,
            string message, string stackTrace)
        {
            m_LogType = logType;
            m_LogTime = logTime;
            m_Message = string.Format("[{0}] {1}", DateTime.Now, message);
            m_StackTrace = string.Format("{0}\r\n{1}", message, stackTrace);
        }
    }
}