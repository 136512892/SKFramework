/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using UnityEngine;

namespace SK.Framework.Actions
{
    public class FrameAction : AbstactAction
    {
        private readonly int m_Duration;

        private int m_BeginFrame;

        private bool m_IsBegan;

        public FrameAction(int duration, System.Action action)
        {
            m_Duration = duration;
            m_OnCompleted = action;
        }

        protected override void OnInvoke()
        {
            if (!m_IsBegan)
            {
                m_IsBegan = true;
                m_BeginFrame = Time.frameCount;
            }
            m_IsCompleted = Time.frameCount - m_BeginFrame >= m_Duration;
        }

        protected override void OnReset()
        {
            base.OnReset();
            m_IsBegan = false;
        }
    }
}