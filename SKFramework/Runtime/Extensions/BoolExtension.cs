/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;

namespace SK.Framework
{
    public static class BoolExtension
    {
        public static void OnTrue(this bool self, Action onTrue)
        {
            if (self)
                onTrue();
        }

        public static void Execute(this bool self, Action onTrue, Action onFalse)
        {
            if (self)
                onTrue();
            else
                onFalse();
        }
    }
}