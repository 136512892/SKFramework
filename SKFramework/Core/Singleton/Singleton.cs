/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Reflection;

using UnityEngine;

namespace SK.Framework.Singleton
{
    public class Singleton<T> where T : ISingleton
    {
        private static T m_Instance;

        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    if (typeof(T).IsSubclassOf(typeof(MonoBehaviour)))
                    {
                        m_Instance = (T)(UnityEngine.Object.FindObjectOfType(typeof(T)) as object);
                        if (m_Instance == null)
                            m_Instance = (T)(new GameObject(string.Format(
                                "[Singleton.{0}]", typeof(T).Name)).AddComponent(typeof(T)) as object);
                        m_Instance.OnInit();
                    }
                    else
                    {
                        ConstructorInfo[] ctors = typeof(T).GetConstructors(
                            BindingFlags.Instance | BindingFlags.Public);
                        if (Array.FindIndex(ctors, m => m.GetParameters().Length == 0) != -1)
                        {
                            m_Instance = Activator.CreateInstance<T>();
                            m_Instance.OnInit();
                        }
                        else throw new ArgumentException();
                    }
                }
                return m_Instance;
            }
        }
    }
}