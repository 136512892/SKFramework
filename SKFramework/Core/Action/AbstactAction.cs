/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;

namespace SK.Framework.Actions
{
    public abstract class AbstactAction : IAction
    {
        protected bool m_IsCompleted;

        protected System.Action m_OnCompleted;

        public bool Invoke()
        {
            if (!m_IsCompleted)
                OnInvoke();
            if (m_IsCompleted)
                m_OnCompleted?.Invoke();
            return m_IsCompleted;
        }

        protected abstract void OnInvoke();

        public void Reset()
        {
            m_IsCompleted = false;
            OnReset();
        }

        protected virtual void OnReset() { }
    }
}