using UnityEngine;

namespace SK.Framework.Log
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/Log")]
    public class LogComponent : MonoBehaviour, ILogComponent
    {
        [SerializeField]
        private ILogger logger = new Logger();

        public void Info<T>(T arg)
        {
            logger?.Info(arg);
        }
        public void Info<T>(string format, T arg)
        {
            logger?.Info(format, arg);
        }
        public void Info<T1, T2>(string format, T1 arg1, T2 arg2)
        {
            logger?.Info(format, arg1, arg2);
        }
        public void Info<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3)
        {
            logger?.Info(format, arg1, arg2, arg3);
        }
        public void Info<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            logger?.Info(format, arg1, arg2, arg3, arg4);
        }
        public void Info<T1, T2, T3, T4, T5>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            logger?.Info(format, arg1, arg2, arg3, arg4, arg5);
        }
        public void Info<T1, T2, T3, T4, T5, T6>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            logger?.Info(format, arg1, arg2, arg3, arg4, arg5, arg6);
        }
        public void Info<T1, T2, T3, T4, T5, T6, T7>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            logger?.Info(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
        public void Info<T1, T2, T3, T4, T5, T6, T7, T8>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            logger?.Info(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
        public void Info<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            logger?.Info(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }
        public void Info<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            logger?.Info(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        public void Warning<T>(T arg)
        {
            logger?.Warning(arg);
        }
        public void Warning<T>(string format, T arg)
        {
            logger?.Warning(format, arg);
        }
        public void Warning<T1, T2>(string format, T1 arg1, T2 arg2)
        {
            logger?.Warning(format, arg1, arg2);
        }
        public void Warning<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3)
        {
            logger?.Warning(format, arg1, arg2, arg3);
        }
        public void Warning<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            logger?.Warning(format, arg1, arg2, arg3, arg4);
        }
        public void Warning<T1, T2, T3, T4, T5>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            logger?.Warning(format, arg1, arg2, arg3, arg4, arg5);
        }
        public void Warning<T1, T2, T3, T4, T5, T6>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            logger?.Warning(format, arg1, arg2, arg3, arg4, arg5, arg6);
        }
        public void Warning<T1, T2, T3, T4, T5, T6, T7>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            logger?.Warning(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
        public void Warning<T1, T2, T3, T4, T5, T6, T7, T8>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            logger?.Warning(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
        public void Warning<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            logger?.Warning(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }
        public void Warning<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            logger?.Warning(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        public void Error<T>(T arg)
        {
            logger?.Error(arg);
        }
        public void Error<T>(string format, T arg)
        {
            logger?.Error(format, arg);
        }
        public void Error<T1, T2>(string format, T1 arg1, T2 arg2)
        {
            logger?.Error(format, arg1, arg2);
        }
        public void Error<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3)
        {
            logger?.Error(format, arg1, arg2, arg3);
        }
        public void Error<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            logger?.Error(format, arg1, arg2, arg3, arg4);
        }
        public void Error<T1, T2, T3, T4, T5>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            logger?.Error(format, arg1, arg2, arg3, arg4, arg5);
        }
        public void Error<T1, T2, T3, T4, T5, T6>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            logger?.Error(format, arg1, arg2, arg3, arg4, arg5, arg6);
        }
        public void Error<T1, T2, T3, T4, T5, T6, T7>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            logger?.Error(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
        public void Error<T1, T2, T3, T4, T5, T6, T7, T8>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            logger?.Error(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
        public void Error<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            logger?.Error(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }
        public void Error<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            logger?.Error(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        public T SwitchLogger<T>() where T : ILogger, new()
        {
            logger = new T();
            return (T)logger;
        }
    }
}