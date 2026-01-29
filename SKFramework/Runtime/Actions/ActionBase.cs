/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using SK.Framework.ObjectPool;

namespace SK.Framework.Actions
{
    public abstract class ActionBase : IAction
    {
        protected bool m_IsCompleted;

        protected Action m_OnCompleted;
        
        bool IPoolable.isRecycled { get; set; }

        DateTime IPoolable.entryTime { get; set; }
        
        bool IAction.Invoke() => Invoke();
        void IAction.Reset() => Reset();
        void IAction.Release() => Release();
        void IPoolable.OnRecycled() => OnRecycled();

        private bool Invoke()
        {
            if (!m_IsCompleted)
                OnInvoke();
            if (m_IsCompleted)
            {
                m_OnCompleted?.Invoke();
                OnCompleted();
            }
            return m_IsCompleted;
        }
        
        protected abstract void OnInvoke();

        protected virtual void OnCompleted() { }

        protected internal virtual void Reset()
        {
            m_IsCompleted = false;
        }
        
        protected virtual void Release() { }
        
        protected virtual void OnRecycled()
        {
            m_IsCompleted = false;
            m_OnCompleted = null;
        }
    }
}