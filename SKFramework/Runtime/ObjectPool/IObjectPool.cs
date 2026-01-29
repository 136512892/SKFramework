/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

namespace SK.Framework.ObjectPool
{
    public interface IObjectPool
    {
        int currentCacheCount { get; }

        int maxCacheCount { get; set; }

        float maxKeepDuration { get; set; }

        void Update();

        bool Recycle<T>(T obj) where T : IPoolable; 
    }
}