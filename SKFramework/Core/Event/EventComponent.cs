using System;
using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework.Events
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/Event")]
    public class EventComponent : MonoBehaviour
    {
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
    }
}