/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using UnityEngine;

namespace SK.Framework.Actions
{
    public class TimerAction : ActionBase
    {
        private float m_Duration;
        private bool m_IsReverse;
        private float m_BeginTime;
        private bool m_IsBegan;
        private Action<float> m_Action;
        private bool m_IsIgnoreTimeScale;
        
        protected override void OnInvoke()
        {
            if (!m_IsBegan)
            {
                m_IsBegan = true;
                m_BeginTime = m_IsIgnoreTimeScale ? Time.realtimeSinceStartup : Time.time;
            }

            float elapsedTime = (m_IsIgnoreTimeScale 
                ? Time.realtimeSinceStartup : Time.time) - m_BeginTime;
            m_Action.Invoke(Mathf.Clamp(m_IsReverse
                ? m_Duration - elapsedTime
                : elapsedTime, 0, m_Duration));
            m_IsCompleted = elapsedTime >= m_Duration;
        }

        protected internal override void Reset()
        {
            base.Reset();
            m_IsBegan = false;
        }

        protected override void Release()
        {
            base.Release();
            SKFramework.Module<ObjectPool.ObjectPool>().Get<TimerAction>().Recycle(this);
        }

        protected override void OnRecycled()
        {
            base.OnRecycled();
            m_Action = null;
            m_IsBegan = false;
        }

        public static TimerAction Allocate(float duration, Action<float> action,
            bool isReverse = false, bool ignoreTimeScale = false)
        {
            var instance = SKFramework.Module<ObjectPool.ObjectPool>().Get<TimerAction>().Allocate();
            instance.m_Duration = duration;
            instance.m_Action = action;
            instance.m_IsReverse = isReverse;
            instance.m_IsIgnoreTimeScale = ignoreTimeScale;
            return instance;
        }
    }
}