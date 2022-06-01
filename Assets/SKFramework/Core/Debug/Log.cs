using UnityEngine;
using System.Text;

namespace SK.Framework 
{
    /// <summary>
    /// 日志系统
    /// </summary>
    public class Log 
    {
        public static void Info(Module module, object message)
        {
            Debug.Log(string.Format("<color=cyan><b>[SKFramework.{0}.Info]</b></color> {1}", module, message));
        }
        public static void Info(Module module, object message, params object[] args)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(message);
            if (args.Length > 0)
            {
                sb.Append("\r\n参数信息: ");
                for (int i = 0; i < args.Length; i++)
                {
                    sb.Append(string.Format("[{0}]{1}", i + 1, args[i]));
                }
            }
            Debug.Log(string.Format("<color=cyan><b>[SKFramework.{0}.Info]</b></color> {1}", module, sb.ToString()));
        }

        public static void Warn(Module module, object message)
        {
            Debug.LogWarning(string.Format("<color=yellow><b>[SKFramework.{0}.Warn]</b></color> {1}", module, message));
        }
        public static void Warn(Module module, object message, params object[] args)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(message);
            if (args.Length > 0)
            {
                sb.Append("\r\n参数信息: ");
                for (int i = 0; i < args.Length; i++)
                {
                    sb.Append(string.Format("[{0}]{1}", i + 1, args[i]));
                }
            }
            Debug.LogWarning(string.Format("<color=yellow><b>[SKFramework.{0}.Warn]</b></color> {1}", module, sb.ToString()));
        }

        public static void Error(Module module, object message)
        {
            Debug.LogError(string.Format("<color=red><b>[SKFramework.{0}.Error]</b></color> {1}", module, message));
        }
        public static void Error(Module module, object message, params object[] args)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(message);
            if (args.Length > 0)
            {
                sb.Append("\r\n参数信息: ");
                for (int i = 0; i < args.Length; i++)
                {
                    sb.Append(string.Format("[{0}]{1}", i + 1, args[i]));
                }
            }
            Debug.LogError(string.Format("<color=red><b>[SKFramework.{0}.Error]</b></color> {1}", module, sb.ToString()));
        }
    }
}