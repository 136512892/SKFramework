/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;
using SK.Framework.Logger;
using ILogger = SK.Framework.Logger.ILogger;

namespace SK.Framework.Custom
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/SKFramework.Custom")]
    public class Custom : ModuleBase
    {
        private readonly Dictionary<Type, ICustomComponent> m_Dic = new Dictionary<Type, ICustomComponent>();
        private ILogger m_Logger;

        protected internal override void OnInitialization()
        {
            base.OnInitialization();
            m_Logger = SKFramework.Module<Log>().GetLogger<ModuleLogger>();
            ICustomComponent[] components = GetComponentsInChildren<ICustomComponent>(true);
            for (int i= 0; i < components.Length; i++)
            {
                ICustomComponent component = components[i];
                m_Dic.Add(component.GetType(), component);
            }
            foreach (var component in m_Dic.Values)
            {
                component.OnInitialization();
            }

            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(m => m.GetTypes())
                .Where(m => typeof(ICustomComponent).IsAssignableFrom(m) && !m.IsAbstract 
                    && m.GetCustomAttribute<AutoRegisterAttribute>() != null);
            foreach (var type in types)
            {
                if (type.IsSubclassOf(typeof(MonoBehaviour)))
                {
                    var obj = new GameObject(type.FullName);
                    obj.transform.parent = transform;
                    var component = obj.AddComponent(type) as ICustomComponent;
                    Add(component);
                }
                else
                {
                    var ctors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
                    if (Array.FindIndex(ctors, m => m.GetParameters().Length == 0) != -1)
                    {
                        var component = Activator.CreateInstance(type) as ICustomComponent;
                        Add(component);
                    }
                }
            }
        }

        protected internal override void OnTermination()
        {
            base.OnTermination();
            foreach (var component in m_Dic.Values)
            {
                component.OnTermination();
            }
            m_Dic.Clear();
        }

        public void Add<T>(params object[] args) where T : ICustomComponent
        {
            Type type = typeof(T);
            if (!m_Dic.ContainsKey(type))
            {
                if (type.IsSubclassOf(typeof(MonoBehaviour)))
                {
                    var obj = new GameObject(typeof(T).FullName);
                    obj.transform.parent = transform;
                    T instance = (T)(obj.AddComponent(type) as object);
                    Add(instance);
                }
                else
                {
                    int index = Array.FindIndex(type.GetConstructors(BindingFlags.Instance 
                        | BindingFlags.Public), m => m.GetParameters().Length == args.Length);
                    if (index != -1)
                    {
                        T instance = (T)Activator.CreateInstance(type, args);
                        Add(instance);
                    }
                    else
                    {
                        m_Logger.Error("A constructor with {0} arguments does not exist.", args.Length);
                    }
                }
            }
            else
            {
                m_Logger.Warning("[Custom] A custom component of type {0} already exists.", type.FullName);
            }
        }

        public void Add(ICustomComponent component)
        {
            Type type = component.GetType();
            if (!m_Dic.ContainsKey(type))
            {
                component.OnInitialization();
                m_Dic.Add(type, component);
                m_Logger.Info("[Custom] Add component: {0}", type.FullName);
            }
            else
            {
                m_Logger.Warning("[Custom] A custom component of type {0} already exists.", type.FullName);
            }
        }

        public bool Remove<T>() where T : ICustomComponent
        {
            Type type = typeof(T);
            if (m_Dic.TryGetValue(type, out ICustomComponent customComponent))
            {
                customComponent.OnTermination();
                m_Dic.Remove(type);
                m_Logger.Info("[Custom] Remove component: {0}", type.FullName);
                return true;
            }
            m_Logger.Warning("[Custom] A custom component of type {0} does not exists.", type.FullName);
            return false;
        }

        public bool Remove(ICustomComponent component)
        {
            Type type = component.GetType();
            if (m_Dic.ContainsKey(type))
            {
                component.OnTermination();
                m_Dic.Remove(type);
                m_Logger.Info("[Custom] Remove component: {0}", type.FullName);
                return true;
            }
            m_Logger.Warning("[Custom] A custom component of type {0} does not exists.", type.FullName);
            return false;
        }

        public bool Has<T>() where T : ICustomComponent
        {
            return m_Dic != null && m_Dic.ContainsKey(typeof(T));
        }

        public T Get<T>() where T : ICustomComponent
        {
            Type type = typeof(T);
            return m_Dic.ContainsKey(type) ? (T)m_Dic[type] : default;
        }

        public bool TryGet<T>(out T customComponent) where T : ICustomComponent
        {
            customComponent = default;
            if (m_Dic.TryGetValue(typeof(T), out ICustomComponent target))
            {
                customComponent = (T)target;
                return true;
            }
            return false;
        }
    }
}