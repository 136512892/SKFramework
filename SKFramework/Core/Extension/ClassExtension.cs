/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;

namespace SK.Framework
{
    public static class ClassExtension
    {
        public static T Invoke<T>(this T self, Action<T> action) where T : class
        {
            if (self != null)
                action(self);
            return self;
        }
    }
}