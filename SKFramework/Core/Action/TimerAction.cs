/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using UnityEngine;

namespace SK.Framework.Actions
{
    public class TimerAction : AbstactAction
    {
        private readonly float m_Duration;

        private readonly bool m_IsReverse;

        private float m_BeginTime;

        private bool m_IsBegan;

        private readonly Action<float> m_Action;

        public TimerAction(float duration, bool isReverse, Action<float> action)
        {
            m_Duration = duration;
            m_IsBegan = isReverse;
            m_Action = action;
        }

        protected override void OnInvoke()
        {
            if (!m_IsBegan)
            {
                m_IsBegan = true;
                m_BeginTime = Time.time;
            }
            float elapsedTime = Time.time - m_BeginTime;
            m_Action.Invoke(Mathf.Clamp(m_IsReverse
                ? m_Duration - elapsedTime
                : elapsedTime, 0f, m_Duration));
            m_IsCompleted = elapsedTime >= m_Duration;
        }

        protected override void OnReset()
        {
            base.OnReset();
            m_IsBegan = false;
        }
    }
}