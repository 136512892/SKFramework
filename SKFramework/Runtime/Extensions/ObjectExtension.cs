/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;

namespace SK.Framework
{
    public static class ObjectExtension
    {
        public static T As<T>(this Object self) where T : Object
        {
            return self as T;
        }

        public static T Instantiate<T>(this T self) where T : Object
        {
            return Object.Instantiate(self);
        }

        public static T DontDestroyOnLoad<T>(this T self) where T : Object
        {
            Object.DontDestroyOnLoad(self);
            return self;
        }

        public static void Destroy<T>(this T self) where T : Object
        {
            if (self != null)
                Object.Destroy(self);
        }

        public static void Destroy<T>(this T self, float delay) where T : Object
        {
            if (self != null)
                Object.Destroy(self, delay);
        }
    }
}