/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using SK.Framework.ObjectPool;

namespace SK.Framework.Actions
{
    public class ConcurrentActionChain : ActionChainBase
    {
        protected override void OnInvoke()
        {
            if (m_StopPredicate != null && m_StopPredicate.Invoke())
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

        protected override void OnCompleted()
        {
            base.OnCompleted();
            Release();
        }

        protected override void Release()
        {
            base.Release();
            SKFramework.Module<ObjectPool.ObjectPool>().Get<ConcurrentActionChain>().Recycle(this);
        }
    }
}