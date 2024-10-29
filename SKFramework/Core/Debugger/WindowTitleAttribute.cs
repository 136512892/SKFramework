/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;

namespace SK.Framework.Debugger
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class WindowTitleAttribute : Attribute
    {
        public string title { get; private set; }

        public WindowTitleAttribute(string title)
        {
            this.title = title;
        }
    }
}