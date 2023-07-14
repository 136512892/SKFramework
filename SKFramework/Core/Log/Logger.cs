using UnityEngine;

namespace SK.Framework.Log
{
    public class Logger : ILogger
    {
        private readonly string format_Info = "<color=#00FFFF><b>[SKFramework.Info]</b> {0}</color>";
        private readonly string format_Warning = "<color=#FFFF00><b>[SKFramework.Warning]</b></color> {0}";
        private readonly string format_Error = "<color=#FF0000><b>[SKFramework.Error]</b></color> {0}";

        public void Info<T>(T arg)
        {
            Debug.Log(string.Format(format_Info, arg));
        }
        public void Info<T>(string format, T arg)
        {
            Debug.Log(string.Format(format_Info, string.Format(format, arg)));
        }
        public void Info<T1, T2>(string format, T1 arg1, T2 arg2)
        {
            Debug.Log(string.Format(format_Info, string.Format(format, arg1, arg2)));
        }
        public void Info<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3)
        {
            Debug.Log(string.Format(format_Info, string.Format(format, arg1, arg2, arg3)));
        }
        public void Info<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            Debug.Log(string.Format(format_Info, string.Format(format, arg1, arg2, arg3, arg4)));
        }
        public void Info<T1, T2, T3, T4, T5>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            Debug.Log(string.Format(format_Info, string.Format(format, arg1, arg2, arg3, arg4, arg5)));
        }
        public void Info<T1, T2, T3, T4, T5, T6>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            Debug.Log(string.Format(format_Info, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6)));
        }
        public void Info<T1, T2, T3, T4, T5, T6, T7>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            Debug.Log(string.Format(format_Info, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7)));
        }
        public void Info<T1, T2, T3, T4, T5, T6, T7, T8>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            Debug.Log(string.Format(format_Info, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8)));
        }
        public void Info<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            Debug.Log(string.Format(format_Info, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9)));
        }
        public void Info<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            Debug.Log(string.Format(format_Info, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10)));
        }

        public void Warning<T>(T arg)
        {
            Debug.LogWarning(string.Format(format_Warning, arg));
        }
        public void Warning<T>(string format, T arg)
        {
            Debug.LogWarning(string.Format(format_Warning, string.Format(format, arg)));
        }
        public void Warning<T1, T2>(string format, T1 arg1, T2 arg2)
        {
            Debug.LogWarning(string.Format(format_Warning, string.Format(format, arg1, arg2)));
        }
        public void Warning<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3)
        {
            Debug.LogWarning(string.Format(format_Warning, string.Format(format, arg1, arg2, arg3)));
        }
        public void Warning<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            Debug.LogWarning(string.Format(format_Warning, string.Format(format, arg1, arg2, arg3, arg4)));
        }
        public void Warning<T1, T2, T3, T4, T5>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            Debug.LogWarning(string.Format(format_Warning, string.Format(format, arg1, arg2, arg3, arg4, arg5)));
        }
        public void Warning<T1, T2, T3, T4, T5, T6>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            Debug.LogWarning(string.Format(format_Warning, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6)));
        }
        public void Warning<T1, T2, T3, T4, T5, T6, T7>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            Debug.LogWarning(string.Format(format_Warning, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7)));
        }
        public void Warning<T1, T2, T3, T4, T5, T6, T7, T8>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            Debug.LogWarning(string.Format(format_Warning, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8)));
        }
        public void Warning<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            Debug.LogWarning(string.Format(format_Warning, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9)));
        }
        public void Warning<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            Debug.LogWarning(string.Format(format_Warning, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10)));
        }

        public void Error<T>(T arg)
        {
            Debug.LogError(string.Format(format_Error, arg));
        }
        public void Error<T>(string format, T arg)
        {
            Debug.LogError(string.Format(format_Error, string.Format(format, arg)));
        }
        public void Error<T1, T2>(string format, T1 arg1, T2 arg2)
        {
            Debug.LogError(string.Format(format_Error, string.Format(format, arg1, arg2)));
        }
        public void Error<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3)
        {
            Debug.LogError(string.Format(format_Error, string.Format(format, arg1, arg2, arg3)));
        }
        public void Error<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            Debug.LogError(string.Format(format_Error, string.Format(format, arg1, arg2, arg3, arg4)));
        }
        public void Error<T1, T2, T3, T4, T5>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            Debug.LogError(string.Format(format_Error, string.Format(format, arg1, arg2, arg3, arg4, arg5)));
        }
        public void Error<T1, T2, T3, T4, T5, T6>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            Debug.LogError(string.Format(format_Error, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6)));
        }
        public void Error<T1, T2, T3, T4, T5, T6, T7>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            Debug.LogError(string.Format(format_Error, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7)));
        }
        public void Error<T1, T2, T3, T4, T5, T6, T7, T8>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            Debug.LogError(string.Format(format_Error, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8)));
        }
        public void Error<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            Debug.LogError(string.Format(format_Error, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9)));
        }
        public void Error<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            Debug.LogError(string.Format(format_Error, string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10)));
        }
    }
}