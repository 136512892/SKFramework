using System;
using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework.Events
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/Event")]
    public class EventComponent : MonoBehaviour
    {
        private readonly Dictionary<int, List<Delegate>> fireDic = new Dictionary<int, List<Delegate>>();
        private readonly Dictionary<int, EventArgsPack> packDic = new Dictionary<int, EventArgsPack>();

        private void SubscribeInternal(int eventId, Delegate callback)
        {
            if (!fireDic.ContainsKey(eventId))
            {
                fireDic.Add(eventId, new List<Delegate>());
            }
            fireDic[eventId].Add(callback);
        }

        private bool UnsubscribeInternal(int eventId, Delegate callback)
        {
            if (fireDic.TryGetValue(eventId, out List<Delegate> list))
            {
                list.Remove(callback);
                if (list.Count == 0)
                {
                    fireDic.Remove(eventId);
                }
                return true;
            }
            return false;
        }

        public void Publish(int eventId)
        {
            if (fireDic.TryGetValue(eventId, out List<Delegate> list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] is Action)
                    {
                        Action action = list[i] as Action;
                        action.Invoke();
                    }
                }
            }
        }

        public void Publish<T>(int eventId, T arg)
        {
            if (fireDic.TryGetValue(eventId, out List<Delegate> list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] is Action<T>)
                    {
                        Action<T> action = list[i] as Action<T>;
                        action.Invoke(arg);
                    }
                }
            }
        }

        public void Publish<T1, T2>(int eventId, T1 arg1, T2 arg2)
        {
            if (fireDic.TryGetValue(eventId, out List<Delegate> list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] is Action<T1, T2>)
                    {
                        Action<T1, T2> action = list[i] as Action<T1, T2>;
                        action.Invoke(arg1, arg2);
                    }
                }
            }
        }

        public void Publish<T1, T2, T3>(int eventId, T1 arg1, T2 arg2, T3 arg3)
        {
            if (fireDic.TryGetValue(eventId, out List<Delegate> list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] is Action<T1, T2, T3>)
                    {
                        Action<T1, T2, T3> action = list[i] as Action<T1, T2, T3>;
                        action.Invoke(arg1, arg2, arg3);
                    }
                }
            }
        }

        public void Publish<T1, T2, T3, T4>(int eventId, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (fireDic.TryGetValue(eventId, out List<Delegate> list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] is Action<T1, T2, T3, T4>)
                    {
                        Action<T1, T2, T3, T4> action = list[i] as Action<T1, T2, T3, T4>;
                        action.Invoke(arg1, arg2, arg3, arg4);
                    }
                }
            }
        }

        public void Publish<T1, T2, T3, T4, T5>(int eventId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (fireDic.TryGetValue(eventId, out List<Delegate> list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] is Action<T1, T2, T3, T4, T5>)
                    {
                        Action<T1, T2, T3, T4, T5> action = list[i] as Action<T1, T2, T3, T4, T5>;
                        action.Invoke(arg1, arg2, arg3, arg4, arg5);
                    }
                }
            }
        }

        public void Publish<T1, T2, T3, T4, T5, T6>(int eventId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            if (fireDic.TryGetValue(eventId, out List<Delegate> list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] is Action<T1, T2, T3, T4, T5, T6>)
                    {
                        Action<T1, T2, T3, T4, T5, T6> action = list[i] as Action<T1, T2, T3, T4, T5, T6>;
                        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
                    }
                }
            }
        }

        public void Subscribe(int subject, Action callback)
        {
            SubscribeInternal(subject, callback);
        }

        public void Subscribe<T>(int subject, Action<T> callback)
        {
            SubscribeInternal(subject, callback);
        }

        public void Subscribe<T1, T2>(int subject, Action<T1, T2> callback)
        {
            SubscribeInternal(subject, callback);
        }

        public void Subscribe<T1, T2, T3>(int subject, Action<T1, T2, T3> callback)
        {
            SubscribeInternal(subject, callback);
        }

        public void Subscribe<T1, T2, T3, T4>(int subject, Action<T1, T2, T3, T4> callback)
        {
            SubscribeInternal(subject, callback);
        }
 
        public void Subscribe<T1, T2, T3, T4, T5>(int subject, Action<T1, T2, T3, T4, T5> callback)
        {
            SubscribeInternal(subject, callback);
        }

        public void Subscribe<T1, T2, T3, T4, T5, T6>(int subject, Action<T1, T2, T3, T4, T5, T6> callback)
        {
            SubscribeInternal(subject, callback);
        }

        public bool Unsubscribe(int subject, Action callback)
        {
            return UnsubscribeInternal(subject, callback);
        }

        public bool Unsubscribe<T>(int subject, Action<T> callback)
        {
            return UnsubscribeInternal(subject, callback);
        }

        public bool Unsubscribe<T1, T2>(int subject, Action<T1, T2> callback)
        {
            return UnsubscribeInternal(subject, callback);
        }

        public bool Unsubscribe<T1, T2, T3>(int subject, Action<T1, T2, T3> callback)
        {
            return UnsubscribeInternal(subject, callback);
        }

        public bool Unsubscribe<T1, T2, T3, T4>(int subject, Action<T1, T2, T3, T4> callback)
        {
            return UnsubscribeInternal(subject, callback);
        }

        public bool Unsubscribe<T1, T2, T3, T4, T5>(int subject, Action<T1, T2, T3, T4, T5> callback)
        {
            return UnsubscribeInternal(subject, callback);
        }

        public bool Unsubscribe<T1, T2, T3, T4, T5, T6>(int subject, Action<T1, T2, T3, T4, T5, T6> callback)
        {
            return UnsubscribeInternal(subject, callback);
        }

        public bool Pack<T>(int packId, T pack) where T : EventArgsPack
        {
            if (!packDic.ContainsKey(packId))
            {
                packDic.Add(packId, pack);
                return true;
            }
            return false;
        }

        public bool Unpack<T>(int packId, Action<T> callback) where T : EventArgsPack
        {
            if (packDic.TryGetValue(packId, out EventArgsPack pack))
            {
                callback.Invoke(pack as T);
                return true;
            }
            return false;
        }
    }
}