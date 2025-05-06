/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;

namespace SK.Framework
{
    public static class IntUtility
    {
        public static char ToLetter(int v)
        {
            if (v < 1 || v > 26) return default;
            return Convert.ToChar('A' + v - 1);
        }

        public static int Fact(int v)
        {
            return v == 0 ? 1 : v * Fact(v - 1);
        }
    }
}