/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;

namespace SK.Framework.Custom
{
    public class Custom : ModuleBase
    {
        private readonly Dictionary<Type, ICustomComponent> m_CustomComponentDic = new Dictionary<Type, ICustomComponent>();

        public override void OnInitialization()
        {
            base.OnInitialization();
            ICustomComponent[] customComponents = GetComponentsInChildren<ICustomComponent>(true);
            for (int i = 0; i < customComponents.Length; i++)
            {
                ICustomComponent customComponent = customComponents[i];
                customComponent.OnInitialization();
                m_CustomComponentDic.Add(customComponent.GetType(), customComponent);
            }
        }

        public override void OnTermination()
        {
            base.OnTermination();
            foreach (ICustomComponent customComponent in m_CustomComponentDic.Values)
                customComponent.OnTermination();
            m_CustomComponentDic.Clear();
        }

        public void Add<T>() where T : ICustomComponent
        {
            Type type = typeof(T);
            if (!m_CustomComponentDic.ContainsKey(type))
            {
                if (type.IsSubclassOf(typeof(MonoBehaviour)))
                {
                    T instance = (T)(gameObject.AddComponent(type) as object);
                    Add(instance);
                }
                else
                {
                    int index = Array.FindIndex(
                        type.GetConstructors(BindingFlags.Instance | BindingFlags.Public), 
                        m => m.GetParameters().Length == 0);
                    if (index != -1)
                    {
                        T instance = (T)Activator.CreateInstance(type);
                        Add(instance);
                    }
                    else throw new Exception(string.Format("{0}类型不存在公有无参构造函数", type));
                }
            }
            else throw new Exception(string.Format("{0}类型的自定义组件已存在", type));
        }

        public void Add(ICustomComponent component)
        {
            if (!m_CustomComponentDic.ContainsKey(component.GetType()))
            {
                component.OnInitialization();
                m_CustomComponentDic.Add(component.GetType(), component);
            }            
        }

        public bool Remove<T>() where T : ICustomComponent
        {
            Type type = typeof(T);
            if (m_CustomComponentDic.TryGetValue(type, out ICustomComponent customComponent))
            {
                customComponent.OnTermination();
                m_CustomComponentDic.Remove(type);
                return true;
            }
            return false;
        }

        public bool Remove(ICustomComponent component)
        {
            Type type = component.GetType();
            if (m_CustomComponentDic.ContainsKey(type))
            {
                component.OnTermination();
                m_CustomComponentDic.Remove(type);
                return true;
            }
            return false;
        }

        public bool Has<T>() where T : ICustomComponent
        {
            return m_CustomComponentDic != null && m_CustomComponentDic.ContainsKey(typeof(T));
        }

        public T Get<T>() where T : ICustomComponent
        {
            Type type = typeof(T);
            return m_CustomComponentDic.ContainsKey(type) ? (T)m_CustomComponentDic[type] : default;
        }

        public bool TryGet<T>(out T customComponent) where T : ICustomComponent
        {
            customComponent = default;
            if (m_CustomComponentDic.TryGetValue(typeof(T), out ICustomComponent target))
            {
                customComponent = (T)target;
                return true;
            }
            return false;
        }
    }
}