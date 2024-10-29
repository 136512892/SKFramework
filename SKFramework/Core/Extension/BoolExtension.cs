/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;

namespace SK.Framework
{
    public static class BoolExtension
    {
        public static bool Execute(this bool self, Action action)
        {
            if (self)
                action();
            return self;
        }

        public static bool Execute(this bool self, Action actionWhenTrue, Action actionWhenFalse)
        {
            if (self)
                actionWhenTrue();
            else
                actionWhenFalse();
            return self;
        }
    }
}