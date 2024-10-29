/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System.Collections.Generic;

namespace SK.Framework.Actions
{
    public class Actions : ModuleBase
    {
        private readonly List<IAction> m_List = new List<IAction>(8);

        private void Update()
        {
            for (int i = 0; i < m_List.Count; i++)
            {
                IAction action = m_List[i];
                if (action.Invoke())
                {
                    m_List.RemoveAt(i);
                    i--;
                }
            }
        }

        internal void Invoke(IAction action)
        {
            if (!m_List.Contains(action))
                m_List.Add(action);
        }

        public SequenceActionChain Sequence()
        {
            return new SequenceActionChain();
        }

        public ConcurrentActionChain Concurrent()
        {
            return new ConcurrentActionChain();
        }

        public TimelineActionChain Timeline()
        {
            return new TimelineActionChain();
        }
    }
}