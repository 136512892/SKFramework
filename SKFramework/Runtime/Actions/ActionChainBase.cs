/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Collections.Generic;

namespace SK.Framework.Actions
{
    public abstract class ActionChainBase : ActionBase, IActionChain
    {
        protected List<IAction> m_CacheList = new List<IAction>();
        protected List<IAction> m_InvokeList = new List<IAction>();
        protected Func<bool> m_StopPredicate;
        protected int m_Loops = 1;
        private int m_LoopsCache = 1;
        
        public bool isPaused { get; protected set; }

        IActionChain IActionChain.Append(IAction action) => Append(action);

        protected internal override void Reset()
        {
            base.Reset();
            isPaused = false;
            for (int i = 0; i < m_CacheList.Count; i++)
            {
                var action = m_CacheList[i];
                action.Reset();
                m_CacheList.Add(action);
            }
        }
        
        protected override void OnRecycled()
        {
            base.OnRecycled();
            m_StopPredicate = null;
            for (int i = 0; i < m_CacheList.Count; i++)
            {
                var action = m_CacheList[i];
                action.Release();
            }
            m_CacheList.Clear();
            m_InvokeList.Clear();
            m_IsCompleted = false;
            m_Loops = m_LoopsCache;
        }
        
        protected internal IActionChain Append(IAction action)
        {
            m_CacheList.Add(action);
            m_InvokeList.Add(action);
            return this;
        }
        
        public IActionChain StopWhen(Func<bool> predicate)
        {
            m_StopPredicate = predicate;
            return this;
        }

        public IActionChain OnStop(Action onStop)
        {
            m_OnCompleted = onStop;
            return this;
        }
        
        public IActionChain Begin()
        {
            SKFramework.Module<Actions>().Invoke(this);
            return this;
        }

        public void Stop()
        {
            m_IsCompleted = true;
        }

        public void Pause()
        {
            isPaused = true;
        }

        public void Resume()
        {
            isPaused = false;
        }

        public IActionChain SetLoops(int loops)
        {
            m_Loops = loops;
            m_LoopsCache = loops;
            return this;
        }
    }
    
    public static class ActionChainExtension
    {
        public static IActionChain Event(this IActionChain chain, Action action)
        {
            return chain.Append(SimpleAction.Allocate(action));
        }

        public static IActionChain Events(this IActionChain chain, params Action[] actions)
        {
            for (int i = 0; i < actions.Length; i++)
                chain.Append(SimpleAction.Allocate(actions[i]));
            return chain;
        }

        public static IActionChain Delay(this IActionChain chain, float duration, bool ignoreTimeScale = false, Action action = null)
        {
            return chain.Append(DelayAction.Allocate(duration, ignoreTimeScale, action));
        }

        public static IActionChain Frame(this IActionChain chain, int delayFrameCount, Action action = null)
        {
            return chain.Append(FrameAction.Allocate(delayFrameCount, action));
        }

        public static IActionChain Timer(this IActionChain chain, float duration, Action<float> action, bool isReverse, bool ignoreTimeScale = false)
        {
            return chain.Append(TimerAction.Allocate(duration, action, isReverse, ignoreTimeScale));
        }

        public static IActionChain Until(this IActionChain chain, Func<bool> predicate, Action action = null)
        {
            return chain.Append(UntilAction.Allocate(predicate, action));
        }

        public static IActionChain While(this IActionChain chain, Func<bool> predicate, Action action = null)
        {
            return chain.Append(WhileAction.Allocate(predicate, action));
        }
    }
}