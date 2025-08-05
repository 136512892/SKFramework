/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using UnityEngine;

namespace SK.Framework.Actions
{
    public class DelayAction : ActionBase
    {
        private float m_Duration; //In seconds.
        private float m_BeginTime;
        private bool m_IsBegan;
        private bool m_IsIgnoreTimeScale;
        
        protected override void OnInvoke()
        {
            if (!m_IsBegan)
            {
                m_IsBegan = true;
                m_BeginTime = m_IsIgnoreTimeScale ? Time.realtimeSinceStartup : Time.time;
            }
            m_IsCompleted = (m_IsIgnoreTimeScale ? Time.realtimeSinceStartup : Time.time) - m_BeginTime >= m_Duration;
        }

        protected internal override void Reset()
        {
            base.Reset();
            m_IsBegan = false;
        }

        protected override void Release()
        {
            base.Release();
            SKFramework.Module<ObjectPool.ObjectPool>().Get<DelayAction>().Recycle(this);
        }

        public static DelayAction Allocate(float duration, bool ignoreTimeScale = false, Action action = null)
        {
            var instance = SKFramework.Module<ObjectPool.ObjectPool>().Get<DelayAction>().Allocate();
            instance.m_Duration = duration;
            instance.m_IsIgnoreTimeScale = ignoreTimeScale;
            instance.m_OnCompleted = action;
            return instance;
        }
    }
}