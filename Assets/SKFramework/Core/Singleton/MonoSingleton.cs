﻿using UnityEngine;

namespace SK.Framework
{
    /// <summary>
    /// Mono类型单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class MonoSingleton<T> where T : MonoBehaviour, IMonoSingleton
    {
        private static T instance;

        private static readonly object _lock = new object();

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (null == instance)
                    {
                        instance = Object.FindObjectOfType<T>() ?? new GameObject($"[{typeof(T).Name}]").AddComponent<T>();
                        instance.OnInit();
                        if (instance.IsDontDestroyOnLoad)
                        {
                            Object.DontDestroyOnLoad(instance);
                        }
                    }
                }
                return instance;
            }
        }
        public static void Dispose()
        {
            instance = null;
        }
    }
}