/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework.Actions
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/SKFramework.Actions")]
    public class Actions : ModuleBase
    {
        private readonly List<IAction> m_List = new List<IAction>();

        protected internal override void OnUpdate()
        {
            base.OnUpdate();
            for (int i = 0; i < m_List.Count; i++)
            {
                var action = m_List[i];
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
            return SKFramework.Module<ObjectPool.ObjectPool>().Get<SequenceActionChain>().Allocate();
        }

        public ConcurrentActionChain Concurrent()
        {
            return SKFramework.Module<ObjectPool.ObjectPool>().Get<ConcurrentActionChain>().Allocate();
        }
    }
}