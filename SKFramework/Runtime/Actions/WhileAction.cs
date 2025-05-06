/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;

namespace SK.Framework.Actions
{
    public class WhileAction : ActionBase
    {
        private Func<bool> m_Predicate;
        private Action m_Action;

        protected override void OnInvoke()
        {
            m_Action?.Invoke();
            m_IsCompleted = !m_Predicate();
        }

        protected override void Release()
        {
            base.Release();
            SKFramework.Module<ObjectPool.ObjectPool>().Get<WhileAction>().Recycle(this);
        }

        protected override void OnRecycled()
        {
            base.OnRecycled();
            m_Predicate = null;
            m_Action = null;
        }

        public static WhileAction Allocate(Func<bool> predicate, Action action)
        {
            var instance = SKFramework.Module<ObjectPool.ObjectPool>().Get<WhileAction>().Allocate();
            instance.m_Predicate = predicate;
            instance.m_Action = action;
            return instance;
        }
    }
}