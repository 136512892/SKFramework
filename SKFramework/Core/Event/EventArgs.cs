/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

namespace SK.Framework.Events
{
    public abstract class EventArgs
    {
        public abstract int ID { get; }

        public virtual void OnInvoke() { }
    }
}