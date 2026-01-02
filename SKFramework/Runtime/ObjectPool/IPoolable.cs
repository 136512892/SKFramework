/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;

namespace SK.Framework.ObjectPool
{
    public interface IPoolable
    {
        bool isRecycled { get; internal set; }

        DateTime entryTime { get; internal set; }

        void OnRecycled();
    }
}