using System;
using UnityEngine;

namespace SK.Framework.Debugger
{
    public class ConsoleItem
    {
        public LogType type;

        public DateTime time;

        public string message;

        public string stackTrace;

        public string brief;

        public string detail;

        public ConsoleItem(DateTime time, LogType type, string message, string stackTrace)
        {
            this.type = type;
            this.time = time;
            this.message = message;
            this.stackTrace = stackTrace;
            brief = string.Format("[{0}] {1}", time, message);
            detail = string.Format("[{0}] {1}\r\n{2}", time, message, stackTrace);
        }
    }
}