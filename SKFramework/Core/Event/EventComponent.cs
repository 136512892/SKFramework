using System;
using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework.Events
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/Event")]
    public class EventComponent : MonoBehaviour, IEventComponent
    {
        #region >> 立即执行 - 事件可能是主线程之外的线程抛出的
        private readonly Dictionary<int, List<Delegate>> dic = new Dictionary<int, List<Delegate>>();

        private void SubscribeInternal(int eventId, Delegate callback)
        {
            if (!dic.ContainsKey(eventId))
            {
                dic.Add(eventId, new List<Delegate>());
            }
            dic[eventId].Add(callback);
        }

        private bool UnsubscribeInternal(int eventId, Delegate callback)
        {
            if (dic.TryGetValue(eventId, out List<Delegate> list))
            {
                list.Remove(callback);
                if (list.Count == 0)
                {
                    dic.Remove(eventId);
                }
                return true;
            }
            return false;
        }

        public void Publish(int eventId)
        {
            if (dic.TryGetValue(eventId, out List<Delegate> list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] is Action action)
                    {
                        action.Invoke();
                    }
                }
            }
        }

        public void Publish<T>(int eventId, T arg)
        {
            if (dic.TryGetValue(eventId, out List<Delegate> list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] is Action<T> action)
                    {
                        action.Invoke(arg);
                    }
                }
            }
        }

        public void Publish<T1, T2>(int eventId, T1 arg1, T2 arg2)
        {
            if (dic.TryGetValue(eventId, out List<Delegate> list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] is Action<T1, T2> action)
                    {
                        action.Invoke(arg1, arg2);
                    }
                }
            }
        }

        public void Publish<T1, T2, T3>(int eventId, T1 arg1, T2 arg2, T3 arg3)
        {
            if (dic.TryGetValue(eventId, out List<Delegate> list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] is Action<T1, T2, T3> action)
                    {
                        action.Invoke(arg1, arg2, arg3);
                    }
                }
            }
        }

        public void Publish<T1, T2, T3, T4>(int eventId, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (dic.TryGetValue(eventId, out List<Delegate> list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] is Action<T1, T2, T3, T4> action)
                    {
                        action.Invoke(arg1, arg2, arg3, arg4);
                    }
                }
            }
        }

        public void Publish<T1, T2, T3, T4, T5>(int eventId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (dic.TryGetValue(eventId, out List<Delegate> list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] is Action<T1, T2, T3, T4, T5> action)
                    {
                        action.Invoke(arg1, arg2, arg3, arg4, arg5);
                    }
                }
            }
        }

        public void Publish<T1, T2, T3, T4, T5, T6>(int eventId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            if (dic.TryGetValue(eventId, out List<Delegate> list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] is Action<T1, T2, T3, T4, T5, T6> action)
                    {
                        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
                    }
                }
            }
        }

        public void Publish<T1, T2, T3, T4, T5, T6, T7>(int eventId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            if (dic.TryGetValue(eventId, out List<Delegate> list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] is Action<T1, T2, T3, T4, T5, T6, T7> action)
                    {
                        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                    }
                }
            }
        }

        public void Publish<T1, T2, T3, T4, T5, T6, T7, T8>(int eventId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            if (dic.TryGetValue(eventId, out List<Delegate> list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] is Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
                    {
                        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
                    }
                }
            }
        }

        public void Publish<T1, T2, T3, T4, T5, T6, T7, T8, T9>(int eventId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            if (dic.TryGetValue(eventId, out List<Delegate> list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] is Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
                    {
                        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
                    }
                }
            }
        }

        public void Publish<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int eventId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            if (dic.TryGetValue(eventId, out List<Delegate> list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] is Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
                    {
                        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
                    }
                }
            }
        }

        public void Subscribe(int eventId, Action callback)
        {
            SubscribeInternal(eventId, callback);
        }

        public void Subscribe<T>(int eventId, Action<T> callback)
        {
            SubscribeInternal(eventId, callback);
        }

        public void Subscribe<T1, T2>(int eventId, Action<T1, T2> callback)
        {
            SubscribeInternal(eventId, callback);
        }

        public void Subscribe<T1, T2, T3>(int eventId, Action<T1, T2, T3> callback)
        {
            SubscribeInternal(eventId, callback);
        }

        public void Subscribe<T1, T2, T3, T4>(int eventId, Action<T1, T2, T3, T4> callback)
        {
            SubscribeInternal(eventId, callback);
        }
 
        public void Subscribe<T1, T2, T3, T4, T5>(int eventId, Action<T1, T2, T3, T4, T5> callback)
        {
            SubscribeInternal(eventId, callback);
        }

        public void Subscribe<T1, T2, T3, T4, T5, T6>(int eventId, Action<T1, T2, T3, T4, T5, T6> callback)
        {
            SubscribeInternal(eventId, callback);
        }

        public void Subscribe<T1, T2, T3, T4, T5, T6, T7>(int eventId, Action<T1, T2, T3, T4, T5, T6, T7> callback)
        {
            SubscribeInternal(eventId, callback);
        }

        public void Subscribe<T1, T2, T3, T4, T5, T6, T7, T8>(int eventId, Action<T1, T2, T3, T4, T5, T6, T7, T8> callback)
        {
            SubscribeInternal(eventId, callback);
        }

        public void Subscribe<T1, T2, T3, T4, T5, T6, T7, T8, T9>(int eventId, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> callback)
        {
            SubscribeInternal(eventId, callback);
        }

        public void Subscribe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int eventId, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callback)
        {
            SubscribeInternal(eventId, callback);
        }

        public bool Unsubscribe(int eventId, Action callback)
        {
            return UnsubscribeInternal(eventId, callback);
        }

        public bool Unsubscribe<T>(int eventId, Action<T> callback)
        {
            return UnsubscribeInternal(eventId, callback);
        }

        public bool Unsubscribe<T1, T2>(int eventId, Action<T1, T2> callback)
        {
            return UnsubscribeInternal(eventId, callback);
        }

        public bool Unsubscribe<T1, T2, T3>(int eventId, Action<T1, T2, T3> callback)
        {
            return UnsubscribeInternal(eventId, callback);
        }

        public bool Unsubscribe<T1, T2, T3, T4>(int eventId, Action<T1, T2, T3, T4> callback)
        {
            return UnsubscribeInternal(eventId, callback);
        }

        public bool Unsubscribe<T1, T2, T3, T4, T5>(int eventId, Action<T1, T2, T3, T4, T5> callback)
        {
            return UnsubscribeInternal(eventId, callback);
        }

        public bool Unsubscribe<T1, T2, T3, T4, T5, T6>(int eventId, Action<T1, T2, T3, T4, T5, T6> callback)
        {
            return UnsubscribeInternal(eventId, callback);
        }

        public bool Unsubscribe<T1, T2, T3, T4, T5, T6, T7>(int eventId, Action<T1, T2, T3, T4, T5, T6, T7> callback)
        {
            return UnsubscribeInternal(eventId, callback);
        }

        public bool Unsubscribe<T1, T2, T3, T4, T5, T6, T7, T8>(int eventId, Action<T1, T2, T3, T4, T5, T6, T7, T8> callback)
        {
            return UnsubscribeInternal(eventId, callback);
        }

        public bool Unsubscribe<T1, T2, T3, T4, T5, T6, T7, T8, T9>(int eventId, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> callback)
        {
            return UnsubscribeInternal(eventId, callback);
        }

        public bool Unsubscribe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int eventId, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callback)
        {
            return UnsubscribeInternal(eventId, callback);
        }
        #endregion

        #region >> 下帧执行 - 确保事件在主线程中抛出
        private readonly Queue<EventArgs> queue = new Queue<EventArgs>();
        private readonly Dictionary<int, List<Action<EventArgs>>> dic2 = new Dictionary<int, List<Action<EventArgs>>>();

        private void Update()
        {
            while (queue.Count > 0)
            {
                var e = queue.Dequeue();
                if (dic2.TryGetValue(e.ID, out List<Action<EventArgs>> list))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].Invoke(e);
                    }
                    e.OnInvoke();
                }
            }
        }

        public void Publish<T>(T e) where T : EventArgs
        {
            queue.Enqueue(e);
        }
        
        public void Subscribe(int eventId, Action<EventArgs> callback)
        {
            if (!dic2.ContainsKey(eventId))
            {
                dic2.Add(eventId, new List<Action<EventArgs>>());
            }
            dic2[eventId].Add(callback);
        }

        public bool Unsubscribe(int eventId, Action<EventArgs> callback)
        {
            if (dic2.TryGetValue(eventId, out List<Action<EventArgs>> list))
            {
                list.Remove(callback);
                if (list.Count == 0)
                {
                    dic2.Remove(eventId);
                }
                return true;
            }
            return false;
        }
        #endregion
    }
}