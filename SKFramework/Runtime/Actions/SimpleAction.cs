/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;

namespace SK.Framework.Actions
{
    public class SimpleAction : ActionBase
    {
        protected override void OnInvoke()
        {
            m_IsCompleted = true;
        }

        protected override void Release()
        {
            base.Release();
            SKFramework.Module<ObjectPool.ObjectPool>().Get<SimpleAction>().Recycle(this);
        }

        public static SimpleAction Allocate(Action action)
        {
            var instance = SKFramework.Module<ObjectPool.ObjectPool>().Get<SimpleAction>().Allocate();
            instance.m_OnCompleted = action;
            return instance;
        }
    }
}