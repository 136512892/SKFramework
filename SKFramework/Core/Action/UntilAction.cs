/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;

namespace SK.Framework.Actions
{
    public class UntilAction : AbstactAction
    {
        private readonly Func<bool> m_Predicate;

        public UntilAction(Func<bool> predicate, System.Action action)
        {
            m_Predicate = predicate;
            m_OnCompleted = action;
        }

        protected override void OnInvoke()
        {
            m_IsCompleted = m_Predicate.Invoke();
        }
    }
}