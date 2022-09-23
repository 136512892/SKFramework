using System;
using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework.Message
{
    /// <summary>
    /// 消息中心
    /// </summary>
    [AddComponentMenu("")]
    public class Messenger : MonoBehaviour
    {
        private static Messenger instance;
        private Dictionary<int, List<Delegate>> intSubjects;
        private Dictionary<string, List<Delegate>> stringSubjects;
        private Dictionary<int, List<IMessage>> intMessages;
        private Dictionary<string, List<IMessage>> stringMessages;

        internal static Messenger Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("[SKFramework.Messenger]").AddComponent<Messenger>();
                    instance.intSubjects = new Dictionary<int, List<Delegate>>();
                    instance.stringSubjects = new Dictionary<string, List<Delegate>>();
                    instance.intMessages = new Dictionary<int, List<IMessage>>();
                    instance.stringMessages = new Dictionary<string, List<IMessage>>();
                    DontDestroyOnLoad(instance);
                }
                return instance;
            }
        }

        private void Subscribe(int subject, Delegate callback)
        {
            if (!intSubjects.ContainsKey(subject))
            {
                intSubjects.Add(subject, new List<Delegate>());
            }
            intSubjects[subject].Add(callback);
        }
        private void Subscribe(string subject, Delegate callback)
        {
            if (!stringSubjects.ContainsKey(subject))
            {
                stringSubjects.Add(subject, new List<Delegate>());
            }
            stringSubjects[subject].Add(callback);
        }
        private bool Unsubscribe(int subject, Delegate callback)
        {
            if (intSubjects.TryGetValue(subject, out List<Delegate> target))
            {
                target.Remove(callback);
                if (target.Count == 0)
                {
                    intSubjects.Remove(subject);
                }
                return true;
            }
            return false;
        }
        private bool Unsubscribe(string subject, Delegate callback)
        {
            if (stringSubjects.TryGetValue(subject, out List<Delegate> target))
            {
                target.Remove(callback);
                if (target.Count == 0)
                {
                    stringSubjects.Remove(subject);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="subject">消息主题</param>
        /// <param name="callback">订阅事件</param>
        public static void Subscribe(int subject, Action callback)
        {
            Instance.Subscribe(subject, callback);
        }
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="subject">消息主题</param>
        /// <param name="callback">订阅事件</param>
        public static void Subscribe<T>(int subject, Action<T> callback)
        {
            Instance.Subscribe(subject, callback);
        }
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="subject">消息主题</param>
        /// <param name="callback">订阅事件</param>
        public static void Subscribe<T1, T2>(int subject, Action<T1, T2> callback)
        {
            Instance.Subscribe(subject, callback);
        }
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="subject">消息主题</param>
        /// <param name="callback">订阅事件</param>
        public static void Subscribe<T1, T2, T3>(int subject, Action<T1, T2, T3> callback)
        {
            Instance.Subscribe(subject, callback);
        }
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="subject">消息主题</param>
        /// <param name="callback">订阅事件</param>
        public static void Subscribe<T1, T2, T3, T4>(int subject, Action<T1, T2, T3, T4> callback)
        {
            Instance.Subscribe(subject, callback);
        }
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="subject">消息主题</param>
        /// <param name="callback">订阅事件</param>
        public static void Subscribe<T1, T2, T3, T4, T5>(int subject, Action<T1, T2, T3, T4, T5> callback)
        {
            Instance.Subscribe(subject, callback);
        }

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="subject">消息主题</param>
        /// <param name="callback">订阅事件</param>
        public static void Subscribe(string subject, Action callback)
        {
            Instance.Subscribe(subject, callback);
        }
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="subject">消息主题</param>
        /// <param name="callback">订阅事件</param>
        public static void Subscribe<T>(string subject, Action<T> callback)
        {
            Instance.Subscribe(subject, callback);
        }
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="subject">消息主题</param>
        /// <param name="callback">订阅事件</param>
        public static void Subscribe<T1, T2>(string subject, Action<T1, T2> callback)
        {
            Instance.Subscribe(subject, callback);
        }
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="subject">消息主题</param>
        /// <param name="callback">订阅事件</param>
        public static void Subscribe<T1, T2, T3>(string subject, Action<T1, T2, T3> callback)
        {
            Instance.Subscribe(subject, callback);
        }
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="subject">消息主题</param>
        /// <param name="callback">订阅事件</param>
        public static void Subscribe<T1, T2, T3, T4>(string subject, Action<T1, T2, T3, T4> callback)
        {
            Instance.Subscribe(subject, callback);
        }
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="subject">消息主题</param>
        /// <param name="callback">订阅事件</param>
        public static void Subscribe<T1, T2, T3, T4, T5>(string subject, Action<T1, T2, T3, T4, T5> callback)
        {
            Instance.Subscribe(subject, callback);
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="subject">消息主题</param>
        /// <param name="callback">订阅事件</param>
        /// <returns>成功取消订阅返回true 否则返回false</returns>
        public static bool Unsubscribe(int subject, Action callback)
        {
            return Instance.Unsubscribe(subject, callback);
        }
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="subject">消息主题</param>
        /// <param name="callback">订阅事件</param>
        /// <returns>成功取消订阅返回true 否则返回false</returns>
        public static bool Unsubscribe<T>(int subject, Action<T> callback)
        {
            return Instance.Unsubscribe(subject, callback);
        }
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="subject">消息主题</param>
        /// <param name="callback">订阅事件</param>
        /// <returns>成功取消订阅返回true 否则返回false</returns>
        public static bool Unsubscribe<T1, T2>(int subject, Action<T1, T2> callback)
        {
            return Instance.Unsubscribe(subject, callback);
        }
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="subject">消息主题</param>
        /// <param name="callback">订阅事件</param>
        /// <returns>成功取消订阅返回true 否则返回false</returns>
        public static bool Unsubscribe<T1, T2, T3>(int subject, Action<T1, T2, T3> callback)
        {
            return Instance.Unsubscribe(subject, callback);
        }
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="subject">消息主题</param>
        /// <param name="callback">订阅事件</param>
        /// <returns>成功取消订阅返回true 否则返回false</returns>
        public static bool Unsubscribe<T1, T2, T3, T4>(int subject, Action<T1, T2, T3, T4> callback)
        {
            return Instance.Unsubscribe(subject, callback);
        }
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="subject">消息主题</param>
        /// <param name="callback">订阅事件</param>
        /// <returns>成功取消订阅返回true 否则返回false</returns>
        public static bool Unsubscribe<T1, T2, T3, T4, T5>(int subject, Action<T1, T2, T3, T4, T5> callback)
        {
            return Instance.Unsubscribe(subject, callback);
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="subject">消息主题</param>
        /// <param name="callback">订阅事件</param>
        /// <returns>成功取消订阅返回true 否则返回false</returns>
        public static bool Unsubscribe(string subject, Action callback)
        {
            return Instance.Unsubscribe(subject, callback);
        }
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="subject">消息主题</param>
        /// <param name="callback">订阅事件</param>
        /// <returns>成功取消订阅返回true 否则返回false</returns>
        public static bool Unsubscribe<T>(string subject, Action<T> callback)
        {
            return Instance.Unsubscribe(subject, callback);
        }
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="subject">消息主题</param>
        /// <param name="callback">订阅事件</param>
        /// <returns>成功取消订阅返回true 否则返回false</returns>
        public static bool Unsubscribe<T1, T2>(string subject, Action<T1, T2> callback)
        {
            return Instance.Unsubscribe(subject, callback);
        }
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="subject">消息主题</param>
        /// <param name="callback">订阅事件</param>
        /// <returns>成功取消订阅返回true 否则返回false</returns>
        public static bool Unsubscribe<T1, T2, T3>(string subject, Action<T1, T2, T3> callback)
        {
            return Instance.Unsubscribe(subject, callback);
        }
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="subject">消息主题</param>
        /// <param name="callback">订阅事件</param>
        /// <returns>成功取消订阅返回true 否则返回false</returns>
        public static bool Unsubscribe<T1, T2, T3, T4>(string subject, Action<T1, T2, T3, T4> callback)
        {
            return Instance.Unsubscribe(subject, callback);
        }
        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="subject">消息主题</param>
        /// <param name="callback">订阅事件</param>
        /// <returns>成功取消订阅返回true 否则返回false</returns>
        public static bool Unsubscribe<T1, T2, T3, T4, T5>(string subject, Action<T1, T2, T3, T4, T5> callback)
        {
            return Instance.Unsubscribe(subject, callback);
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="subject">消息主题</param>
        public static void Publish(int subject)
        {
            var dic = Instance.intSubjects;
            if (dic.TryGetValue(subject, out List<Delegate> target))
            {
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] is Action)
                    {
                        Action callback = target[i] as Action;
                        callback.Invoke();
                    }
                }
            }
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="subject">消息主题</param>
        public static void Publish<T>(int subject, T t)
        {
            var dic = Instance.intSubjects;
            if (dic.TryGetValue(subject, out List<Delegate> target))
            {
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] is Action<T>)
                    {
                        Action<T> callback = target[i] as Action<T>;
                        callback.Invoke(t);
                    }
                }
            }
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="subject">消息主题</param>
        public static void Publish<T1, T2>(int subject, T1 t1, T2 t2)
        {
            var dic = Instance.intSubjects;
            if (dic.TryGetValue(subject, out List<Delegate> target))
            {
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] is Action<T1, T2>)
                    {
                        Action<T1, T2> callback = target[i] as Action<T1, T2>;
                        callback.Invoke(t1, t2);
                    }
                }
            }
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="subject">消息主题</param>
        public static void Publish<T1, T2, T3>(int subject, T1 t1, T2 t2, T3 t3)
        {
            var dic = Instance.intSubjects;
            if (dic.TryGetValue(subject, out List<Delegate> target))
            {
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] is Action<T1, T2, T3>)
                    {
                        Action<T1, T2, T3> callback = target[i] as Action<T1, T2, T3>;
                        callback.Invoke(t1, t2, t3);
                    }
                }
            }
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="subject">消息主题</param>
        public static void Publish<T1, T2, T3, T4>(int subject, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            var dic = Instance.intSubjects;
            if (dic.TryGetValue(subject, out List<Delegate> target))
            {
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] is Action<T1, T2, T3, T4>)
                    {
                        Action<T1, T2, T3, T4> callback = target[i] as Action<T1, T2, T3, T4>;
                        callback.Invoke(t1, t2, t3, t4);
                    }
                }
            }
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="subject">消息主题</param>
        public static void Publish<T1, T2, T3, T4, T5>(int subject, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            var dic = Instance.intSubjects;
            if (dic.TryGetValue(subject, out List<Delegate> target))
            {
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] is Action<T1, T2, T3, T4, T5>)
                    {
                        Action<T1, T2, T3, T4, T5> callback = target[i] as Action<T1, T2, T3, T4, T5>;
                        callback.Invoke(t1, t2, t3, t4, t5);
                    }
                }
            }
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="subject">消息主题</param>
        public static void Publish(string subject)
        {
            var dic = Instance.stringSubjects;
            if (dic.TryGetValue(subject, out List<Delegate> target))
            {
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] is Action)
                    {
                        Action callback = target[i] as Action;
                        callback.Invoke();
                    }
                }
            }
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="subject">消息主题</param>
        public static void Publish<T>(string subject, T t)
        {
            var dic = Instance.stringSubjects;
            if (dic.TryGetValue(subject, out List<Delegate> target))
            {
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] is Action<T>)
                    {
                        Action<T> callback = target[i] as Action<T>;
                        callback.Invoke(t);
                    }
                }
            }
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="subject">消息主题</param>
        public static void Publish<T1, T2>(string subject, T1 t1, T2 t2)
        {
            var dic = Instance.stringSubjects;
            if (dic.TryGetValue(subject, out List<Delegate> target))
            {
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] is Action<T1, T2>)
                    {
                        Action<T1, T2> callback = target[i] as Action<T1, T2>;
                        callback.Invoke(t1, t2);
                    }
                }
            }
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="subject">消息主题</param>
        public static void Publish<T1, T2, T3>(string subject, T1 t1, T2 t2, T3 t3)
        {
            var dic = Instance.stringSubjects;
            if (dic.TryGetValue(subject, out List<Delegate> target))
            {
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] is Action<T1, T2, T3>)
                    {
                        Action<T1, T2, T3> callback = target[i] as Action<T1, T2, T3>;
                        callback.Invoke(t1, t2, t3);
                    }
                }
            }
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="subject">消息主题</param>
        public static void Publish<T1, T2, T3, T4>(string subject, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            var dic = Instance.stringSubjects;
            if (dic.TryGetValue(subject, out List<Delegate> target))
            {
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] is Action<T1, T2, T3, T4>)
                    {
                        Action<T1, T2, T3, T4> callback = target[i] as Action<T1, T2, T3, T4>;
                        callback.Invoke(t1, t2, t3, t4);
                    }
                }
            }
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="subject">消息主题</param>
        public static void Publish<T1, T2, T3, T4, T5>(string subject, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            var dic = Instance.stringSubjects;
            if (dic.TryGetValue(subject, out List<Delegate> target))
            {
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] is Action<T1, T2, T3, T4, T5>)
                    {
                        Action<T1, T2, T3, T4, T5> callback = target[i] as Action<T1, T2, T3, T4, T5>;
                        callback.Invoke(t1, t2, t3, t4, t5);
                    }
                }
            }
        }

        /// <summary>
        /// 打包消息
        /// </summary>
        /// <param name="identifier">标识码</param>
        public static void Pack<T>(int identifier, T t)
        {
            var dic = Instance.intMessages;
            if (!dic.ContainsKey(identifier))
            {
                dic.Add(identifier, new List<IMessage>());
            }
            Message<T> message = Activator.CreateInstance<Message<T>>();
            message.content = t;
            dic[identifier].Add(message);
        }
        /// <summary>
        /// 打包消息
        /// </summary>
        /// <param name="identifier">标识码</param>
        public static void Pack<T1, T2>(int identifier, T1 t1, T2 t2)
        {
            var dic = Instance.intMessages;
            if (!dic.ContainsKey(identifier))
            {
                dic.Add(identifier, new List<IMessage>());
            }
            Message<T1, T2> message = Activator.CreateInstance<Message<T1, T2>>();
            message.content1 = t1;
            message.content2 = t2;
            dic[identifier].Add(message);
        }
        /// <summary>
        /// 打包消息
        /// </summary>
        /// <param name="identifier">标识码</param>
        public static void Pack<T1, T2, T3>(int identifier, T1 t1, T2 t2, T3 t3)
        {
            var dic = Instance.intMessages;
            if (!dic.ContainsKey(identifier))
            {
                dic.Add(identifier, new List<IMessage>());
            }
            Message<T1, T2, T3> message = Activator.CreateInstance<Message<T1, T2, T3>>();
            message.content1 = t1;
            message.content2 = t2;
            message.content3 = t3;
            dic[identifier].Add(message);
        }
        /// <summary>
        /// 打包消息
        /// </summary>
        /// <param name="identifier">标识码</param>
        public static void Pack<T1, T2, T3, T4>(int identifier, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            var dic = Instance.intMessages;
            if (!dic.ContainsKey(identifier))
            {
                dic.Add(identifier, new List<IMessage>());
            }
            Message<T1, T2, T3, T4> message = Activator.CreateInstance<Message<T1, T2, T3, T4>>();
            message.content1 = t1;
            message.content2 = t2;
            message.content3 = t3;
            message.content4 = t4;
            dic[identifier].Add(message);
        }
        /// <summary>
        /// 打包消息
        /// </summary>
        /// <param name="identifier">标识码</param>
        public static void Pack<T1, T2, T3, T4, T5>(int identifier, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            var dic = Instance.intMessages;
            if (!dic.ContainsKey(identifier))
            {
                dic.Add(identifier, new List<IMessage>());
            }
            Message<T1, T2, T3, T4, T5> message = Activator.CreateInstance<Message<T1, T2, T3, T4, T5>>();
            message.content1 = t1;
            message.content2 = t2;
            message.content3 = t3;
            message.content4 = t4;
            message.content5 = t5;
            dic[identifier].Add(message);
        }

        /// <summary>
        /// 打包消息
        /// </summary>
        /// <param name="identifier">标识码</param>
        public static void Pack<T>(string identifier, T t)
        {
            var dic = Instance.stringMessages;
            if (!dic.ContainsKey(identifier))
            {
                dic.Add(identifier, new List<IMessage>());
            }
            Message<T> message = Activator.CreateInstance<Message<T>>();
            message.content = t;
            dic[identifier].Add(message);
        }
        /// <summary>
        /// 打包消息
        /// </summary>
        /// <param name="identifier">标识码</param>
        public static void Pack<T1, T2>(string identifier, T1 t1, T2 t2)
        {
            var dic = Instance.stringMessages;
            if (!dic.ContainsKey(identifier))
            {
                dic.Add(identifier, new List<IMessage>());
            }
            Message<T1, T2> message = Activator.CreateInstance<Message<T1, T2>>();
            message.content1 = t1;
            message.content2 = t2;
            dic[identifier].Add(message);
        }
        /// <summary>
        /// 打包消息
        /// </summary>
        /// <param name="identifier">标识码</param>
        public static void Pack<T1, T2, T3>(string identifier, T1 t1, T2 t2, T3 t3)
        {
            var dic = Instance.stringMessages;
            if (!dic.ContainsKey(identifier))
            {
                dic.Add(identifier, new List<IMessage>());
            }
            Message<T1, T2, T3> message = Activator.CreateInstance<Message<T1, T2, T3>>();
            message.content1 = t1;
            message.content2 = t2;
            message.content3 = t3;
            dic[identifier].Add(message);
        }
        /// <summary>
        /// 打包消息
        /// </summary>
        /// <param name="identifier">标识码</param>
        public static void Pack<T1, T2, T3, T4>(string identifier, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            var dic = Instance.stringMessages;
            if (!dic.ContainsKey(identifier))
            {
                dic.Add(identifier, new List<IMessage>());
            }
            Message<T1, T2, T3, T4> message = Activator.CreateInstance<Message<T1, T2, T3, T4>>();
            message.content1 = t1;
            message.content2 = t2;
            message.content3 = t3;
            message.content4 = t4;
            dic[identifier].Add(message);
        }
        /// <summary>
        /// 打包消息
        /// </summary>
        /// <param name="identifier">标识码</param>
        public static void Pack<T1, T2, T3, T4, T5>(string identifier, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            var dic = Instance.stringMessages;
            if (!dic.ContainsKey(identifier))
            {
                dic.Add(identifier, new List<IMessage>());
            }
            Message<T1, T2, T3, T4, T5> message = Activator.CreateInstance<Message<T1, T2, T3, T4, T5>>();
            message.content1 = t1;
            message.content2 = t2;
            message.content3 = t3;
            message.content4 = t4;
            message.content5 = t5;
            dic[identifier].Add(message);
        }

        /// <summary>
        /// 拆包消息
        /// </summary>
        /// <param name="identifier">标识码</param>
        /// <param name="callback">回调函数</param>
        public static void Unpack<T>(int identifier, Action<T> callback)
        {
            var dic = Instance.intMessages;
            if (dic.TryGetValue(identifier, out List<IMessage> target))
            {
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] is Message<T>)
                    {
                        Message<T> message = target[i] as Message<T>;
                        callback.Invoke(message.content);
                        target.RemoveAt(i);
                        i--;
                    }
                }
                if (target.Count == 0)
                {
                    dic.Remove(identifier);
                }
            }
        }
        /// <summary>
        /// 拆包消息
        /// </summary>
        /// <param name="identifier">标识码</param>
        /// <param name="callback">回调函数</param>
        public static void Unpack<T1, T2>(int identifier, Action<T1, T2> callback)
        {
            var dic = Instance.intMessages;
            if (dic.TryGetValue(identifier, out List<IMessage> target))
            {
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] is Message<T1, T2>)
                    {
                        Message<T1, T2> message = target[i] as Message<T1, T2>;
                        callback.Invoke(message.content1, message.content2);
                        target.RemoveAt(i);
                        i--;
                    }
                }
                if (target.Count == 0)
                {
                    dic.Remove(identifier);
                }
            }
        }
        /// <summary>
        /// 拆包消息
        /// </summary>
        /// <param name="identifier">标识码</param>
        /// <param name="callback">回调函数</param>
        public static void Unpack<T1, T2, T3>(int identifier, Action<T1, T2, T3> callback)
        {
            var dic = Instance.intMessages;
            if (dic.TryGetValue(identifier, out List<IMessage> target))
            {
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] is Message<T1, T2, T3>)
                    {
                        Message<T1, T2, T3> message = target[i] as Message<T1, T2, T3>;
                        callback.Invoke(message.content1, message.content2, message.content3);
                        target.RemoveAt(i);
                        i--;
                    }
                }
                if (target.Count == 0)
                {
                    dic.Remove(identifier);
                }
            }
        }
        /// <summary>
        /// 拆包消息
        /// </summary>
        /// <param name="identifier">标识码</param>
        /// <param name="callback">回调函数</param>
        public static void Unpack<T1, T2, T3, T4>(int identifier, Action<T1, T2, T3, T4> callback)
        {
            var dic = Instance.intMessages;
            if (dic.TryGetValue(identifier, out List<IMessage> target))
            {
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] is Message<T1, T2, T3, T4>)
                    {
                        Message<T1, T2, T3, T4> message = target[i] as Message<T1, T2, T3, T4>;
                        callback.Invoke(message.content1, message.content2, message.content3, message.content4);
                        target.RemoveAt(i);
                        i--;
                    }
                }
                if (target.Count == 0)
                {
                    dic.Remove(identifier);
                }
            }
        }
        /// <summary>
        /// 拆包消息
        /// </summary>
        /// <param name="identifier">标识码</param>
        /// <param name="callback">回调函数</param>
        public static void Unpack<T1, T2, T3, T4, T5>(int identifier, Action<T1, T2, T3, T4, T5> callback)
        {
            var dic = Instance.intMessages;
            if (dic.TryGetValue(identifier, out List<IMessage> target))
            {
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] is Message<T1, T2, T3, T4, T5>)
                    {
                        Message<T1, T2, T3, T4, T5> message = target[i] as Message<T1, T2, T3, T4, T5>;
                        callback.Invoke(message.content1, message.content2, message.content3, message.content4, message.content5);
                        target.RemoveAt(i);
                        i--;
                    }
                }
                if (target.Count == 0)
                {
                    dic.Remove(identifier);
                }
            }
        }

        /// <summary>
        /// 拆包消息
        /// </summary>
        /// <param name="identifier">标识码</param>
        /// <param name="callback">回调函数</param>
        public static void Unpack<T>(string identifier, Action<T> callback)
        {
            var dic = Instance.stringMessages;
            if (dic.TryGetValue(identifier, out List<IMessage> target))
            {
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] is Message<T>)
                    {
                        Message<T> message = target[i] as Message<T>;
                        callback.Invoke(message.content);
                        target.RemoveAt(i);
                        i--;
                    }
                }
                if (target.Count == 0)
                {
                    dic.Remove(identifier);
                }
            }
        }
        /// <summary>
        /// 拆包消息
        /// </summary>
        /// <param name="identifier">标识码</param>
        /// <param name="callback">回调函数</param>
        public static void Unpack<T1, T2>(string identifier, Action<T1, T2> callback)
        {
            var dic = Instance.stringMessages;
            if (dic.TryGetValue(identifier, out List<IMessage> target))
            {
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] is Message<T1, T2>)
                    {
                        Message<T1, T2> message = target[i] as Message<T1, T2>;
                        callback.Invoke(message.content1, message.content2);
                        target.RemoveAt(i);
                        i--;
                    }
                }
                if (target.Count == 0)
                {
                    dic.Remove(identifier);
                }
            }
        }
        /// <summary>
        /// 拆包消息
        /// </summary>
        /// <param name="identifier">标识码</param>
        /// <param name="callback">回调函数</param>
        public static void Unpack<T1, T2, T3>(string identifier, Action<T1, T2, T3> callback)
        {
            var dic = Instance.stringMessages;
            if (dic.TryGetValue(identifier, out List<IMessage> target))
            {
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] is Message<T1, T2, T3>)
                    {
                        Message<T1, T2, T3> message = target[i] as Message<T1, T2, T3>;
                        callback.Invoke(message.content1, message.content2, message.content3);
                        target.RemoveAt(i);
                        i--;
                    }
                }
                if (target.Count == 0)
                {
                    dic.Remove(identifier);
                }
            }
        }
        /// <summary>
        /// 拆包消息
        /// </summary>
        /// <param name="identifier">标识码</param>
        /// <param name="callback">回调函数</param>
        public static void Unpack<T1, T2, T3, T4>(string identifier, Action<T1, T2, T3, T4> callback)
        {
            var dic = Instance.stringMessages;
            if (dic.TryGetValue(identifier, out List<IMessage> target))
            {
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] is Message<T1, T2, T3, T4>)
                    {
                        Message<T1, T2, T3, T4> message = target[i] as Message<T1, T2, T3, T4>;
                        callback.Invoke(message.content1, message.content2, message.content3, message.content4);
                        target.RemoveAt(i);
                        i--;
                    }
                }
                if (target.Count == 0)
                {
                    dic.Remove(identifier);
                }
            }
        }
        /// <summary>
        /// 拆包消息
        /// </summary>
        /// <param name="identifier">标识码</param>
        /// <param name="callback">回调函数</param>
        public static void Unpack<T1, T2, T3, T4, T5>(string identifier, Action<T1, T2, T3, T4, T5> callback)
        {
            var dic = Instance.stringMessages;
            if (dic.TryGetValue(identifier, out List<IMessage> target))
            {
                for (int i = 0; i < target.Count; i++)
                {
                    if (target[i] is Message<T1, T2, T3, T4, T5>)
                    {
                        Message<T1, T2, T3, T4, T5> message = target[i] as Message<T1, T2, T3, T4, T5>;
                        callback.Invoke(message.content1, message.content2, message.content3, message.content4, message.content5);
                        target.RemoveAt(i);
                        i--;
                    }
                }
                if (target.Count == 0)
                {
                    dic.Remove(identifier);
                }
            }
        }
    }
}