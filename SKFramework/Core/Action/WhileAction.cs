/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;

namespace SK.Framework.Actions
{
    public class WhileAction : AbstactAction
    {
        private readonly Func<bool> m_Predicate;

        private readonly System.Action m_Action;

        public WhileAction(Func<bool> predicate, System.Action action)
        {
            m_Predicate = predicate;
            m_Action = action;
        }

        protected override void OnInvoke()
        {
            m_Action?.Invoke();
            m_IsCompleted = !m_Predicate.Invoke();
        }
    }
}