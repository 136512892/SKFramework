/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System.Diagnostics;

namespace SK.Framework.Logger
{
    [NecessaryLogger]
    public class ModuleLogger : ILogger
    {
        [SymbolDefine] private const string m_Conditional = "ENABLE_LOG_MODULE";

        private const string format_Debug = "<color=#6A6A6A><b>[SKFramework]</b> {0}</color>";
        private const string format_Info = "<color=#00FFFF><b>[SKFramework]</b> {0}</color>";
        private const string format_Warning = "<color=#FFFF00><b>[SKFramework]</b></color> {0}";
        private const string format_Error = "<color=#FF0000><b>[SKFramework]</b></color> {0}";

        public LogLevel Level { get; set; } = LogLevel.Debug;

        void ILogger.Debug<T>(T arg) => Debug(arg);
        void ILogger.Debug<T>(string format, T arg) => Debug(format, arg);
        void ILogger.Debug<T1, T2>(string format, T1 arg1, T2 arg2) => Debug(format, arg1, arg2);
        void ILogger.Debug<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3) => Debug(format, arg1, arg2, arg3);
        void ILogger.Debug<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => Debug(format, arg1, arg2, arg3, arg4);
        void ILogger.Debug<T1, T2, T3, T4, T5>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => Debug(format, arg1, arg2, arg3, arg4, arg5);
        void ILogger.Debug<T1, T2, T3, T4, T5, T6>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => Debug(format, arg1, arg2, arg3, arg4, arg5, arg6);
        void ILogger.Debug<T1, T2, T3, T4, T5, T6, T7>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) => Debug(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        void ILogger.Debug<T1, T2, T3, T4, T5, T6, T7, T8>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) => Debug(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        void ILogger.Debug<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) => Debug(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        void ILogger.Debug<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) => Debug(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        void ILogger.Debug<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) => Debug(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);

        void ILogger.Info<T>(T arg) => Info(arg);
        void ILogger.Info<T>(string format, T arg) => Info(format, arg);
        void ILogger.Info<T1, T2>(string format, T1 arg1, T2 arg2) => Info(format, arg1, arg2);
        void ILogger.Info<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3) => Info(format, arg1, arg2, arg3);
        void ILogger.Info<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => Info(format, arg1, arg2, arg3, arg4);
        void ILogger.Info<T1, T2, T3, T4, T5>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => Info(format, arg1, arg2, arg3, arg4, arg5);
        void ILogger.Info<T1, T2, T3, T4, T5, T6>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => Info(format, arg1, arg2, arg3, arg4, arg5, arg6);
        void ILogger.Info<T1, T2, T3, T4, T5, T6, T7>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) => Info(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        void ILogger.Info<T1, T2, T3, T4, T5, T6, T7, T8>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) => Info(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        void ILogger.Info<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) => Info(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        void ILogger.Info<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) => Info(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        void ILogger.Info<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) => Info(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);

        void ILogger.Warning<T>(T arg) => Warning(arg);
        void ILogger.Warning<T>(string format, T arg) => Warning(format, arg);
        void ILogger.Warning<T1, T2>(string format, T1 arg1, T2 arg2) => Warning(format, arg1, arg2);
        void ILogger.Warning<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3) => Warning(format, arg1, arg2, arg3);
        void ILogger.Warning<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => Warning(format, arg1, arg2, arg3, arg4);
        void ILogger.Warning<T1, T2, T3, T4, T5>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => Warning(format, arg1, arg2, arg3, arg4, arg5);
        void ILogger.Warning<T1, T2, T3, T4, T5, T6>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => Warning(format, arg1, arg2, arg3, arg4, arg5, arg6);
        void ILogger.Warning<T1, T2, T3, T4, T5, T6, T7>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) => Warning(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        void ILogger.Warning<T1, T2, T3, T4, T5, T6, T7, T8>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) => Warning(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        void ILogger.Warning<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) => Warning(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        void ILogger.Warning<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) => Warning(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        void ILogger.Warning<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) => Warning(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);

        void ILogger.Error<T>(T arg) => Error(arg);
        void ILogger.Error<T>(string format, T arg) => Error(format, arg);
        void ILogger.Error<T1, T2>(string format, T1 arg1, T2 arg2) => Error(format, arg1, arg2);
        void ILogger.Error<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3) => Error(format, arg1, arg2, arg3);
        void ILogger.Error<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => Error(format, arg1, arg2, arg3, arg4);
        void ILogger.Error<T1, T2, T3, T4, T5>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => Error(format, arg1, arg2, arg3, arg4, arg5);
        void ILogger.Error<T1, T2, T3, T4, T5, T6>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => Error(format, arg1, arg2, arg3, arg4, arg5, arg6);
        void ILogger.Error<T1, T2, T3, T4, T5, T6, T7>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) => Error(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        void ILogger.Error<T1, T2, T3, T4, T5, T6, T7, T8>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) => Error(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        void ILogger.Error<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) => Error(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        void ILogger.Error<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) => Error(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        void ILogger.Error<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) => Error(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);

        [Conditional(m_Conditional)]
        private void Debug<T>(T arg)
        {
            if (Level > LogLevel.Debug)
                return;
            UnityEngine.Debug.Log(string.Format(format_Debug, arg));
        }

        [Conditional(m_Conditional)]
        private void Debug<T>(string format, T arg)
        {
            if (Level > LogLevel.Debug)
                return;
            UnityEngine.Debug.Log(string.Format(format_Debug, string.Format(format, arg)));
        }

        [Conditional(m_Conditional)]
        private void Debug<T1, T2>(string format, T1 arg1, T2 arg2)
        {
            if (Level > LogLevel.Debug)
                return;
            UnityEngine.Debug.Log(string.Format(format_Debug, string.Format(format, arg1, arg2)));
        }

        [Conditional(m_Conditional)]
        private void Debug<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3)
        {
            if (Level > LogLevel.Debug)
                return;
            UnityEngine.Debug.Log(string.Format(format_Debug, string.Format(format, arg1, arg2, arg3)));
        }

        [Conditional(m_Conditional)]
        private void Debug<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (Level > LogLevel.Debug)
                return;
            UnityEngine.Debug.Log(string.Format(format_Debug, string.Format(format, arg1, arg2, arg3, arg4)));
        }

        [Conditional(m_Conditional)]
        private void Debug<T1, T2, T3, T4, T5>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (Level > LogLevel.Debug)
                return;
            UnityEngine.Debug.Log(string.Format(format_Debug, string.Format(format, arg1, arg2, arg3, arg4, arg5)));
        }

        [Conditional(m_Conditional)]
        private void Debug<T1, T2, T3, T4, T5, T6>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            if (Level > LogLevel.Debug)
                return;
            UnityEngine.Debug.Log(string.Format(format_Debug, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6)));
        }

        [Conditional(m_Conditional)]
        private void Debug<T1, T2, T3, T4, T5, T6, T7>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            if (Level > LogLevel.Debug)
                return;
            UnityEngine.Debug.Log(string.Format(format_Debug, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7)));
        }

        [Conditional(m_Conditional)]
        private void Debug<T1, T2, T3, T4, T5, T6, T7, T8>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            if (Level > LogLevel.Debug)
                return;
            UnityEngine.Debug.Log(string.Format(format_Debug, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8)));
        }

        [Conditional(m_Conditional)]
        private void Debug<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            if (Level > LogLevel.Debug)
                return;
            UnityEngine.Debug.Log(string.Format(format_Debug, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9)));
        }

        [Conditional(m_Conditional)]
        private void Debug<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            if (Level > LogLevel.Debug)
                return;
            UnityEngine.Debug.Log(string.Format(format_Debug, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10)));
        }

        [Conditional(m_Conditional)]
        private void Debug<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            if (Level > LogLevel.Debug)
                return;
            UnityEngine.Debug.Log(string.Format(format_Debug, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11)));
        }

        [Conditional(m_Conditional)]
        private void Info<T>(T arg)
        {
            if (Level > LogLevel.Info)
                return;
            UnityEngine.Debug.Log(string.Format(format_Info, arg));
        }

        [Conditional(m_Conditional)]
        private void Info<T>(string format, T arg)
        {
            if (Level > LogLevel.Info)
                return;
            UnityEngine.Debug.Log(string.Format(format_Info, string.Format(format, arg)));
        }

        [Conditional(m_Conditional)]
        private void Info<T1, T2>(string format, T1 arg1, T2 arg2)
        {
            if (Level > LogLevel.Info)
                return;
            UnityEngine.Debug.Log(string.Format(format_Info, string.Format(format, arg1, arg2)));
        }

        [Conditional(m_Conditional)]
        private void Info<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3)
        {
            if (Level > LogLevel.Info)
                return;
            UnityEngine.Debug.Log(string.Format(format_Info, string.Format(format, arg1, arg2, arg3)));
        }

        [Conditional(m_Conditional)]
        private void Info<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (Level > LogLevel.Info)
                return;
            UnityEngine.Debug.Log(string.Format(format_Info, string.Format(format, arg1, arg2, arg3, arg4)));
        }

        [Conditional(m_Conditional)]
        private void Info<T1, T2, T3, T4, T5>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (Level > LogLevel.Info)
                return;
            UnityEngine.Debug.Log(string.Format(format_Info, string.Format(format, arg1, arg2, arg3, arg4, arg5)));
        }

        [Conditional(m_Conditional)]
        private void Info<T1, T2, T3, T4, T5, T6>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            if (Level > LogLevel.Info)
                return;
            UnityEngine.Debug.Log(string.Format(format_Info, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6)));
        }

        [Conditional(m_Conditional)]
        private void Info<T1, T2, T3, T4, T5, T6, T7>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            if (Level > LogLevel.Info)
                return;
            UnityEngine.Debug.Log(string.Format(format_Info, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7)));
        }

        [Conditional(m_Conditional)]
        private void Info<T1, T2, T3, T4, T5, T6, T7, T8>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            if (Level > LogLevel.Info)
                return;
            UnityEngine.Debug.Log(string.Format(format_Info, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8)));
        }

        [Conditional(m_Conditional)]
        private void Info<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            if (Level > LogLevel.Info)
                return;
            UnityEngine.Debug.Log(string.Format(format_Info, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9)));
        }

        [Conditional(m_Conditional)]
        private void Info<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            if (Level > LogLevel.Info)
                return;
            UnityEngine.Debug.Log(string.Format(format_Info, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10)));
        }

        [Conditional(m_Conditional)]
        private void Info<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            if (Level > LogLevel.Info)
                return;
            UnityEngine.Debug.Log(string.Format(format_Info, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11)));
        }

        [Conditional(m_Conditional)]
        private void Warning<T>(T arg)
        {
            if (Level > LogLevel.Warning)
                return;
            UnityEngine.Debug.LogWarning(string.Format(format_Warning, arg));
        }

        [Conditional(m_Conditional)]
        private void Warning<T>(string format, T arg)
        {
            if (Level > LogLevel.Warning)
                return;
            UnityEngine.Debug.LogWarning(string.Format(format_Warning, string.Format(format, arg)));
        }

        [Conditional(m_Conditional)]
        private void Warning<T1, T2>(string format, T1 arg1, T2 arg2)
        {
            if (Level > LogLevel.Warning)
                return;
            UnityEngine.Debug.LogWarning(string.Format(format_Warning, string.Format(format, arg1, arg2)));
        }

        [Conditional(m_Conditional)]
        private void Warning<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3)
        {
            if (Level > LogLevel.Warning)
                return;
            UnityEngine.Debug.LogWarning(string.Format(format_Warning, string.Format(format, arg1, arg2, arg3)));
        }

        [Conditional(m_Conditional)]
        private void Warning<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (Level > LogLevel.Warning)
                return;
            UnityEngine.Debug.LogWarning(string.Format(format_Warning, string.Format(format, arg1, arg2, arg3, arg4)));
        }

        [Conditional(m_Conditional)]
        private void Warning<T1, T2, T3, T4, T5>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (Level > LogLevel.Warning)
                return;
            UnityEngine.Debug.LogWarning(string.Format(format_Warning, string.Format(format, arg1, arg2, arg3, arg4, arg5)));
        }

        [Conditional(m_Conditional)]
        private void Warning<T1, T2, T3, T4, T5, T6>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            if (Level > LogLevel.Warning)
                return;
            UnityEngine.Debug.LogWarning(string.Format(format_Warning, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6)));
        }

        [Conditional(m_Conditional)]
        private void Warning<T1, T2, T3, T4, T5, T6, T7>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            if (Level > LogLevel.Warning)
                return;
            UnityEngine.Debug.LogWarning(string.Format(format_Warning, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7)));
        }

        [Conditional(m_Conditional)]
        private void Warning<T1, T2, T3, T4, T5, T6, T7, T8>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            if (Level > LogLevel.Warning)
                return;
            UnityEngine.Debug.LogWarning(string.Format(format_Warning, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8)));
        }

        [Conditional(m_Conditional)]
        private void Warning<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            if (Level > LogLevel.Warning)
                return;
            UnityEngine.Debug.LogWarning(string.Format(format_Warning, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9)));
        }

        [Conditional(m_Conditional)]
        private void Warning<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            if (Level > LogLevel.Warning)
                return;
            UnityEngine.Debug.LogWarning(string.Format(format_Warning, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10)));
        }

        [Conditional(m_Conditional)]
        private void Warning<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            if (Level > LogLevel.Warning)
                return;
            UnityEngine.Debug.LogWarning(string.Format(format_Warning, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11)));
        }

        [Conditional(m_Conditional)]
        private void Error<T>(T arg)
        {
            UnityEngine.Debug.LogError(string.Format(format_Error, arg));
        }

        [Conditional(m_Conditional)]
        private void Error<T>(string format, T arg)
        {
            UnityEngine.Debug.LogError(string.Format(format_Error, string.Format(format, arg)));
        }

        [Conditional(m_Conditional)]
        private void Error<T1, T2>(string format, T1 arg1, T2 arg2)
        {
            UnityEngine.Debug.LogError(string.Format(format_Error, string.Format(format, arg1, arg2)));
        }

        [Conditional(m_Conditional)]
        private void Error<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3)
        {
            UnityEngine.Debug.LogError(string.Format(format_Error, string.Format(format, arg1, arg2, arg3)));
        }

        [Conditional(m_Conditional)]
        private void Error<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            UnityEngine.Debug.LogError(string.Format(format_Error, string.Format(format, arg1, arg2, arg3, arg4)));
        }

        [Conditional(m_Conditional)]
        private void Error<T1, T2, T3, T4, T5>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            UnityEngine.Debug.LogError(string.Format(format_Error, string.Format(format, arg1, arg2, arg3, arg4, arg5)));
        }

        [Conditional(m_Conditional)]
        private void Error<T1, T2, T3, T4, T5, T6>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            UnityEngine.Debug.LogError(string.Format(format_Error, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6)));
        }

        [Conditional(m_Conditional)]
        private void Error<T1, T2, T3, T4, T5, T6, T7>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            UnityEngine.Debug.LogError(string.Format(format_Error, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7)));
        }

        [Conditional(m_Conditional)]
        private void Error<T1, T2, T3, T4, T5, T6, T7, T8>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            UnityEngine.Debug.LogError(string.Format(format_Error, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8)));
        }

        [Conditional(m_Conditional)]
        private void Error<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            UnityEngine.Debug.LogError(string.Format(format_Error, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9)));
        }

        [Conditional(m_Conditional)]
        private void Error<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            UnityEngine.Debug.LogError(string.Format(format_Error, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10)));
        }

        [Conditional(m_Conditional)]
        private void Error<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            UnityEngine.Debug.LogError(string.Format(format_Error, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11)));
        }
    }
}