/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

namespace SK.Framework.Actions
{
    public class SequenceActionChain : ActionChainBase
    {
        protected override void OnInvoke()
        {
            if (m_StopPredicate != null && m_StopPredicate.Invoke())
                m_IsCompleted = true;
            else if (!isPaused)
            {
                if (m_InvokeList.Count > 0)
                {
                    if (m_InvokeList[0].Invoke())
                        m_InvokeList.RemoveAt(0);
                    m_IsCompleted = m_InvokeList.Count == 0;
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

        protected override void OnCompleted()
        {
            base.OnCompleted();
            Release();
        }

        protected override void Release()
        {
            base.Release();
            SKFramework.Module<ObjectPool.ObjectPool>().Get<SequenceActionChain>().Recycle(this);
        }
    }
}