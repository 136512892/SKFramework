/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;

namespace SK.Framework.Actions
{
    public class UntilAction : ActionBase
    {
        private Func<bool> m_Predicate;

        protected override void OnInvoke()
        {
            m_IsCompleted = m_Predicate.Invoke();
        }

        protected override void Release()
        {
            base.Release();
            SKFramework.Module<ObjectPool.ObjectPool>().Get<UntilAction>().Recycle(this);
        }

        protected override void OnRecycled()
        {
            base.OnRecycled();
            m_Predicate = null;
        }

        public static UntilAction Allocate(Func<bool> predicate, Action action = null)
        {
            var instance = SKFramework.Module<ObjectPool.ObjectPool>().Get<UntilAction>().Allocate();
            instance.m_Predicate = predicate;
            instance.m_OnCompleted = action;
            return instance;
        }
    }
}