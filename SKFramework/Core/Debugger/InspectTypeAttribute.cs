/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;

namespace SK.Framework.Debugger
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class InspectType : Attribute
    {
        public Type type { get; internal set; }

        public InspectType(Type type)
        {
            this.type = type;
        }
    }
}