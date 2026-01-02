/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using SK.Framework.ObjectPool;

namespace SK.Framework.Actions
{
    public interface IAction : IPoolable
    {
        bool Invoke();

        void Reset();
        
        void Release();
    }
}