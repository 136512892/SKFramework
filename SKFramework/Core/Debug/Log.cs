using UnityEngine;

namespace SK.Framework 
{
    /// <summary>
    /// 日志系统
    /// </summary>
    public class Log
    {
        public static void Info(object message)
        {
            Debug.Log(message);
        }
        public static void Info(object message, Object context)
        {
            Debug.Log(message, context);
        }
        public static void Info(string format, params object[] args)
        {
            Debug.Log(string.Format(format, args));
        }
        public static void Info(string fomat, Object context, params object[] args)
        {
            Debug.Log(string.Format(fomat, args), context);
        }

        public static void Warn(object message)
        {
            Debug.LogWarning(message);
        }
        public static void Warn(object message, Object context)
        {
            Debug.LogWarning(message, context);
        }
        public static void Warn(string format, params object[] args)
        {
            Debug.LogWarning(string.Format(format, args));
        }
        public static void Warn(string format, Object context, params object[] args)
        {
            Debug.LogWarning(string.Format(format, args), context);
        }

        public static void Error(object message)
        {
            Debug.LogError(message);
        }
        public static void Error(object message, Object context)
        {
            Debug.LogError(message, context);
        }
        public static void Error(string format, params object[] args)
        {
            Debug.LogError(string.Format(format, args));
        }
        public static void Error(string format, Object context, params object[] args)
        {
            Debug.LogError(string.Format(format, args), context);
        }
    }
}