/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Collections.Generic;

namespace SK.Framework.Actions
{
    public abstract class AbstractActionChain : AbstactAction, IActionChain
    {
        public bool isPaused { get; protected set; }

        protected readonly List<IAction> m_CacheList;

        protected readonly List<IAction> m_InvokeList;

        protected Func<bool> m_StopWhen;

        protected int m_Loops = 1;

        public AbstractActionChain()
        {
            m_CacheList = new List<IAction>();
            m_InvokeList = new List<IAction>();
        }

        public IActionChain Append(IAction action)
        {
            m_CacheList.Add(action);
            m_InvokeList.Add(action);
            return this;
        }

        public IActionChain StopWhen(Func<bool> predicate)
        {
            m_StopWhen = predicate;
            return this;
        }

        public IActionChain OnStop(System.Action action)
        {
            m_OnCompleted = action;
            return this;
        }

        public IActionChain Begin()
        {
            SKFramework.Module<Actions>().Invoke(this);
            return this;
        }

        public void Pause()
        {
            isPaused = true;
        }

        public void Resume()
        {
            isPaused = false;
        }

        public void Stop()
        {
            m_IsCompleted = true;
        }

        public IActionChain SetLoops(int loops)
        {
            m_Loops = loops;
            return this;
        }
    }
}