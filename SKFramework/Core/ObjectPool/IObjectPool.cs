/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;

namespace SK.Framework.ObjectPool
{
    public interface IObjectPool<T>
    {
        int currentCacheCount { get; }

        int maxCacheCount { get; }

        void CreateBy(Func<T> createMethod);

        T Allocate();

        bool Recycle(T obj);

        void Release();
    }
}