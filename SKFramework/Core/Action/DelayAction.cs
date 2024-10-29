/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using UnityEngine;

namespace SK.Framework.Actions
{
    public class DelayAction : AbstactAction
    {
        private readonly float m_Duration;

        private float m_BeginTime;

        private bool m_IsBegan;

        public DelayAction(float duration, System.Action action)
        {
            m_Duration = duration;
            m_OnCompleted = action;
        }

        protected override void OnInvoke()
        {
            if (!m_IsBegan)
            {
                m_IsBegan = true;
                m_BeginTime = Time.time;
            }
            m_IsCompleted = Time.time - m_BeginTime >= m_Duration;
        }

        protected override void OnReset()
        {
            base.OnReset();
            m_IsBegan = false;
        }
    }
}