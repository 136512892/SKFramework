/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;

namespace SK.Framework.Actions
{
    public class TimelineActionChain : AbstractActionChain
    {
        public TimelineActionChain() : base() { }

        public float currentTime { get; set; }

        public float speed { get; set; } = 1f;

        protected override void OnInvoke()
        {
            if (m_StopWhen != null && m_StopWhen.Invoke())
                m_IsCompleted = true;
            else if (!isPaused)
            {
                currentTime += Time.deltaTime * speed;
                for (int i = 0; i < m_InvokeList.Count; i++)
                {
                    IAction action = m_InvokeList[i];
                    if (action is TimelineAction ta)
                    {
                        ta.currentTime = currentTime;
                        ta.Invoke();
                    }
                }
            }

            if (m_IsCompleted)
            {
                m_Loops--;
                if (m_Loops != 0)
                    Reset();
                else m_IsCompleted = true;
            }
        }

        protected override void OnReset()
        {
            base.OnReset();
            isPaused = false;
            for (int i = 0; i < m_CacheList.Count; i++)
            {
                IAction action = m_CacheList[i];
                action.Reset();
                m_InvokeList.Add(action);
            }
            currentTime = 0f;
            speed = 1f;
        }
    }
}