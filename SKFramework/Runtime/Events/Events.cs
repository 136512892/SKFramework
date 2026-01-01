/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Collections.Generic;

using UnityEngine;

namespace SK.Framework.Events
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/SKFramework.Events")]
    public class Events : ModuleBase
    {
        private readonly Queue<EventArgs> m_Queue = new Queue<EventArgs>();
        private readonly Dictionary<int, List<Action<EventArgs>>> m_Dic = new Dictionary<int, List<Action<EventArgs>>>();
   
        protected internal override void OnUpdate()
        {
            base.OnUpdate();
            while (m_Queue.Count > 0)
            {
                EventArgs e = m_Queue.Dequeue();
                PublishImmediate(e);
            }
        }

        /* The difference between the Publish method and the PublishImmediate method
         * is that the former will queue Event first, waiting for the next frame to
         * execute, while the latter will be executed immediately,  so be careful
         * to call it in the main thread */

        public void Publish<T>(T e) where T : EventArgs
        {
            if (e == null)
                return;
            m_Queue.Enqueue(e);
        }

        public void PublishImmediate<T>(T e) where T : EventArgs
        {
            if (m_Dic.TryGetValue(e.ID, out List<Action<EventArgs>> list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Invoke(e);
                }
                if (SKFramework.Module<ObjectPool.ObjectPool>().TryGet(e.GetType(), out var pool))
                    pool.Recycle(e);
            }
        }

        public void Subscribe(int eventID, Action<EventArgs> callback)
        {
            if (!m_Dic.TryGetValue(eventID, out List<Action<EventArgs>> list))
            {
                m_Dic.Add(eventID, new List<Action<EventArgs>>());
                m_Dic[eventID].Add(callback);
            }
            else
            {
                if (!list.Contains(callback))
                    list.Add(callback);
            }
        }

        public void Unsubscribe(int eventID, Action<EventArgs> callback)
        {
            if (m_Dic.TryGetValue(eventID, out List<Action<EventArgs>> list))
            {
                if (list.Contains(callback))
                {
                    list.Remove(callback);
                    if (list.Count == 0)
                        m_Dic.Remove(eventID);
                }
            }
        }

        public bool Has(int eventID, Action<EventArgs> callback)
        {
            if (m_Dic.TryGetValue(eventID, out List<Action<EventArgs>> list))
            {
                return list.Contains(callback);
            }
            return false;
        }
    }
}