/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

namespace SK.Framework.Logger
{
    public class Log : ModuleBase
    {
        private ILogger m_Logger = new Logger();

        public void Info<T>(T arg)
        {
            m_Logger.Info(arg);
        }
        public void Info<T>(string format, T arg)
        {
            m_Logger.Info(format, arg);
        }
        public void Info<T1, T2>(string format, T1 arg1, T2 arg2)
        {
            m_Logger.Info(format, arg1, arg2);
        }
        public void Info<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3)
        {
            m_Logger.Info(format, arg1, arg2, arg3);
        }
        public void Info<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            m_Logger.Info(format, arg1, arg2, arg3, arg4);
        }
        public void Info<T1, T2, T3, T4, T5>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            m_Logger.Info(format, arg1, arg2, arg3, arg4, arg5);
        }
        public void Info<T1, T2, T3, T4, T5, T6>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            m_Logger.Info(format, arg1, arg2, arg3, arg4, arg5, arg6);
        }
        public void Info<T1, T2, T3, T4, T5, T6, T7>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            m_Logger.Info(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
        public void Info<T1, T2, T3, T4, T5, T6, T7, T8>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            m_Logger.Info(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
        public void Info<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            m_Logger.Info(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }
        public void Info<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            m_Logger.Info(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }
        public void Info<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            m_Logger.Info(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10,arg11);
        }

        public void Warning<T>(T arg)
        {
            m_Logger.Warning(arg);
        }
        public void Warning<T>(string format, T arg)
        {
            m_Logger.Warning(format, arg);
        }
        public void Warning<T1, T2>(string format, T1 arg1, T2 arg2)
        {
            m_Logger.Warning(format, arg1, arg2);
        }
        public void Warning<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3)
        {
            m_Logger.Warning(format, arg1, arg2, arg3);
        }
        public void Warning<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            m_Logger.Warning(format, arg1, arg2, arg3, arg4);
        }
        public void Warning<T1, T2, T3, T4, T5>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            m_Logger.Warning(format, arg1, arg2, arg3, arg4, arg5);
        }
        public void Warning<T1, T2, T3, T4, T5, T6>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            m_Logger.Warning(format, arg1, arg2, arg3, arg4, arg5, arg6);
        }
        public void Warning<T1, T2, T3, T4, T5, T6, T7>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            m_Logger.Warning(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
        public void Warning<T1, T2, T3, T4, T5, T6, T7, T8>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            m_Logger.Warning(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
        public void Warning<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            m_Logger.Warning(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }
        public void Warning<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            m_Logger.Warning(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }
        public void Warning<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            m_Logger.Warning(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
        }

        public void Error<T>(T arg)
        {
            m_Logger.Error(arg);
        }
        public void Error<T>(string format, T arg)
        {
            m_Logger.Error(format, arg);
        }
        public void Error<T1, T2>(string format, T1 arg1, T2 arg2)
        {
            m_Logger.Error(format, arg1, arg2);
        }
        public void Error<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3)
        {
            m_Logger.Error(format, arg1, arg2, arg3);
        }
        public void Error<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            m_Logger.Error(format, arg1, arg2, arg3, arg4);
        }
        public void Error<T1, T2, T3, T4, T5>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            m_Logger.Error(format, arg1, arg2, arg3, arg4, arg5);
        }
        public void Error<T1, T2, T3, T4, T5, T6>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            m_Logger.Error(format, arg1, arg2, arg3, arg4, arg5, arg6);
        }
        public void Error<T1, T2, T3, T4, T5, T6, T7>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            m_Logger.Error(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
        public void Error<T1, T2, T3, T4, T5, T6, T7, T8>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            m_Logger.Error(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
        public void Error<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            m_Logger.Error(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }
        public void Error<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            m_Logger.Error(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }
        public void Error<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            m_Logger.Error(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
        }
    }
}