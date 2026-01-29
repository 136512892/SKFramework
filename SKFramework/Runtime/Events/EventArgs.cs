/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using SK.Framework.ObjectPool;

namespace SK.Framework.Events
{
    public abstract class EventArgs : IPoolable
    {
        public abstract int ID { get; }

        bool IPoolable.isRecycled { get; set; }

        DateTime IPoolable.entryTime { get; set; }

        void IPoolable.OnRecycled() => OnRecycled();

        protected abstract void OnRecycled();
    }
}