/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using UnityEngine;

namespace SK.Framework.Actions
{
    public class TimelineAction : AbstactAction
    {
        private readonly float m_BeginTime;

        private readonly float m_Duration;

        private readonly Action<float> m_Action;

        public float currentTime;

        public TimelineAction(float beginTime, float duration, Action<float> action)
        {
            m_BeginTime = beginTime;
            m_Duration = duration;
            m_Action = action;
        }

        protected override void OnInvoke()
        {
            float t = (currentTime - m_BeginTime) / m_Duration;
            t = Mathf.Clamp01(t);
            m_Action.Invoke(t);
        }
    }
}