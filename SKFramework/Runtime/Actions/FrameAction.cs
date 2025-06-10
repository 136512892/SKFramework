/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using UnityEngine;

namespace SK.Framework.Actions
{
    public class FrameAction : ActionBase
    {
        private int m_DelayFrameCount;
        private int m_BeginFrame;
        private bool m_IsBegan;
        
        protected override void OnInvoke()
        {
            if (!m_IsBegan)
            {
                m_IsBegan = true;
                m_BeginFrame = Time.frameCount;
            }
            m_IsCompleted = Time.frameCount - m_BeginFrame >= m_DelayFrameCount;
        }

        protected internal override void Reset()
        {
            base.Reset();
            m_IsBegan = false;
        }

        protected override void Release()
        {
            base.Release();
            SKFramework.Module<ObjectPool.ObjectPool>().Get<FrameAction>().Recycle(this);
        }

        protected override void OnRecycled()
        {
            base.OnRecycled();
            m_IsBegan = false;
        }

        public static FrameAction Allocate(int delayFrameCount, Action action = null)
        {
            var instace = SKFramework.Module<ObjectPool.ObjectPool>().Get<FrameAction>().Allocate();
            instace.m_DelayFrameCount = delayFrameCount;
            instace.m_OnCompleted = action;
            return instace;
        }
    }
}