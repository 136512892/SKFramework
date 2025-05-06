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

namespace SK.Framework.Debugger
{
    [WindowTitle("Inspector")]
    public class InspectorWindow : DebuggerWindow
    {
        private Component[] m_Components;
        private readonly Dictionary<Component, bool> m_FoldoutDic = new Dictionary<Component, bool>();
        private Vector2 m_Scroll;
        private readonly Dictionary<Component, List<InspectorField>> m_FieldDic = new Dictionary<Component, List<InspectorField>>();
        private readonly Dictionary<Type, Type> m_InspectDic = new Dictionary<Type, Type>();

        public override void OnInitialized()
        {
            base.OnInitialized();
            m_InspectDic.Clear();
            Type[] types = GetType().Assembly.GetTypes()
                .Where(m => m.IsSubclassOf(typeof(InspectorField)))
                .ToArray();
            for (int i = 0; i < types.Length; i++)
            {
                Type type = types[i];
                var attribute = type.GetCustomAttribute<InspectType>();
                if (attribute != null)
                    m_InspectDic.Add(attribute.type, type);
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Draw4Components(Camera.main.GetComponents<Component>());
        }

        public override void OnGUI()
        {
            if (m_Components == null) return;
            m_Scroll = GUILayout.BeginScrollView(m_Scroll);
            for (int i = 0; i < m_Components.Length; i++)
            {
                Component component = m_Components[i];
                Type type = component.GetType();
                m_FoldoutDic[component] = GUILayout.Toggle(m_FoldoutDic[component], type.Name);
                if (m_FoldoutDic[component])
                {
                    List<InspectorField> fields = m_FieldDic[component];
                    for (int j = 0; j < fields.Count; j++)
                    {
                        fields[j].Draw();
                    }
                }
            }
            GUILayout.EndScrollView();
        }

        private void Draw4Components(Component[] components)
        {
            m_Components = components;
            m_FoldoutDic.Clear();
            m_FieldDic.Clear();
            for (int i = 0; i < m_Components.Length; i++)
            {
                Component component = m_Components[i];
                m_FoldoutDic.Add(component, false);
                m_FieldDic.Add(component, new List<InspectorField>());
                Draw4Component(component);
            }
        }
        private void Draw4Component(Component component)
        {
            MemberInfo[] mis = component.GetType().GetMembers(BindingFlags.Instance
                | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                .Where(m => m.GetCustomAttribute<ObfuscationAttribute>() == null
                   && ((m is FieldInfo fi && ((fi.IsPublic && fi.GetCustomAttribute<HideInInspector>() == null)
                        || fi.GetCustomAttribute<SerializeField>() != null))
                        || (m is PropertyInfo pi && pi.GetGetMethod(true).IsPublic)))
                .ToArray();
            for (int i = 0; i < mis.Length; i++)
            {
                MemberInfo memberInfo = mis[i];
                Type type = memberInfo is FieldInfo fi
                    ? fi.FieldType
                    : memberInfo is PropertyInfo pi
                    ? pi.PropertyType
                    : null;
                if (m_InspectDic.TryGetValue(type, out Type inspectorFieldType))
                {
                    if (Activator.CreateInstance(inspectorFieldType,
                        component, memberInfo) is InspectorField inspectorField)
                        m_FieldDic[component].Add(inspectorField);
                }
                else if (!type.IsPrimitive && type.GetCustomAttribute<SerializableAttribute>() != null)
                {
                    var target = memberInfo is FieldInfo fieldInfo
                        ? fieldInfo.GetValue(component)
                        : memberInfo is PropertyInfo propertyInfo
                        ? propertyInfo.GetValue(component)
                        : null;
                    m_FieldDic[component].Add(new ObjectField(target, memberInfo, m_InspectDic));
                }
            }
        }
    }
}