/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

namespace SK.Framework.Actions
{
    public class ConcurrentActionChain : AbstractActionChain
    {
        public ConcurrentActionChain() : base() { }

        protected override void OnInvoke()
        {
            if (m_StopWhen != null && m_StopWhen.Invoke())
                m_IsCompleted = true;
            else if (!isPaused)
            {
                for (int i = 0; i < m_InvokeList.Count; i++)
                {
                    if (m_InvokeList[i].Invoke())
                    {
                        m_InvokeList.RemoveAt(i);
                        i--;
                    }
                }
                m_IsCompleted = m_InvokeList.Count == 0;
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
        }
    }
}