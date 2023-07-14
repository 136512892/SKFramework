using UnityEngine;

namespace SK.Framework.Log
{
    public class SimpleLogger : ILogger
    {
        public void Info<T>(T arg)
        {
#if LOGGER
            Debug.Log(arg);
#endif
        }
        public void Info<T>(string format, T arg)
        {
#if LOGGER
            Debug.Log(string.Format(format, arg));
#endif
        }
        public void Info<T1, T2>(string format, T1 arg1, T2 arg2)
        {
#if LOGGER
            Debug.Log(string.Format(format, arg1, arg2) );
#endif
        }
        public void Info<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3)
        {
#if LOGGER
            Debug.Log(string.Format(format, arg1, arg2, arg3));
#endif
        }
        public void Info<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
#if LOGGER
            Debug.Log(string.Format(format, arg1, arg2, arg3, arg4));
#endif
        }
        public void Info<T1, T2, T3, T4, T5>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
#if LOGGER
            Debug.Log(string.Format(format, arg1, arg2, arg3, arg4, arg5));
#endif
        }
        public void Info<T1, T2, T3, T4, T5, T6>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
#if LOGGER
            Debug.Log(string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6));
#endif
        }
        public void Info<T1, T2, T3, T4, T5, T6, T7>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
#if LOGGER
            Debug.Log(string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7));
#endif
        }
        public void Info<T1, T2, T3, T4, T5, T6, T7, T8>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
#if LOGGER
            Debug.Log(string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));
#endif
        }
        public void Info<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
#if LOGGER
            Debug.Log(string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
#endif
        }
        public void Info<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
#if LOGGER
            Debug.Log(string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10));
#endif
        }

        public void Warning<T>(T arg)
        {
#if LOGGER
            Debug.LogWarning(arg);
#endif
        }
        public void Warning<T>(string format, T arg)
        {
#if LOGGER
            Debug.LogWarning(string.Format(format, arg));
#endif
        }
        public void Warning<T1, T2>(string format, T1 arg1, T2 arg2)
        {
#if LOGGER
            Debug.LogWarning(string.Format(format, arg1, arg2));
#endif
        }
        public void Warning<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3)
        {
#if LOGGER
            Debug.LogWarning(string.Format(format, arg1, arg2, arg3));
#endif
        }
        public void Warning<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
#if LOGGER
            Debug.LogWarning(string.Format(format, arg1, arg2, arg3, arg4));
#endif
        }
        public void Warning<T1, T2, T3, T4, T5>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
#if LOGGER
            Debug.LogWarning(string.Format(format, arg1, arg2, arg3, arg4, arg5));
#endif
        }
        public void Warning<T1, T2, T3, T4, T5, T6>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
#if LOGGER
            Debug.LogWarning(string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6));
#endif
        }
        public void Warning<T1, T2, T3, T4, T5, T6, T7>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
#if LOGGER
            Debug.LogWarning(string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7));
#endif
        }
        public void Warning<T1, T2, T3, T4, T5, T6, T7, T8>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
#if LOGGER
            Debug.LogWarning(string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));
#endif
        }
        public void Warning<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
#if LOGGER
            Debug.LogWarning(string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
#endif
        }
        public void Warning<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
#if LOGGER
            Debug.LogWarning(string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10));
#endif
        }

        public void Error<T>(T arg)
        {
#if LOGGER
            Debug.LogError(arg);
#endif
        }
        public void Error<T>(string format, T arg)
        {
#if LOGGER
            Debug.LogError(string.Format(format, arg));
#endif
        }
        public void Error<T1, T2>(string format, T1 arg1, T2 arg2)
        {
#if LOGGER
            Debug.LogError(string.Format(format, arg1, arg2));
#endif
        }
        public void Error<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3)
        {
#if LOGGER
            Debug.LogError(string.Format(format, arg1, arg2, arg3));
#endif
        }
        public void Error<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
#if LOGGER
            Debug.LogError(string.Format(format, arg1, arg2, arg3, arg4));
#endif
        }
        public void Error<T1, T2, T3, T4, T5>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
#if LOGGER
            Debug.LogError(string.Format(format, arg1, arg2, arg3, arg4, arg5));
#endif
        }
        public void Error<T1, T2, T3, T4, T5, T6>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
#if LOGGER
            Debug.LogError(string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6));
#endif
        }
        public void Error<T1, T2, T3, T4, T5, T6, T7>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
#if LOGGER
            Debug.LogError(string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7));
#endif
        }
        public void Error<T1, T2, T3, T4, T5, T6, T7, T8>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
#if LOGGER
            Debug.LogError(string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));
#endif
        }
        public void Error<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
#if LOGGER
            Debug.LogError(string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
#endif
        }
        public void Error<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
#if LOGGER
            Debug.LogError(string.Format(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10));
#endif
        }
    }
}