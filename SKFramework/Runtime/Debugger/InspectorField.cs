/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Reflection;

using UnityEngine;

namespace SK.Framework.Debugger
{
    public abstract class InspectorField
    {
        public delegate object Getter();
        public delegate void Setter(object value);

        private readonly object m_Target;
        protected readonly MemberInfo m_MemberInfo;

        protected readonly Getter m_Getter;
        protected readonly Setter m_Setter;

        public object Value
        {
            get
            {
                return m_Getter();
            }
            set
            {
                try
                {
                    m_Setter?.Invoke(value);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public InspectorField(object target, MemberInfo memberInfo)
        {
            if (memberInfo is FieldInfo fieldInfo)
            {
                m_Target = target;
                m_MemberInfo = memberInfo;
                m_Getter = () => fieldInfo.GetValue(m_Target);
                m_Setter = v => fieldInfo.SetValue(m_Target, v);
            }
            else if (memberInfo is PropertyInfo propertyInfo)
            {
                m_Target = target;
                m_MemberInfo = memberInfo;
                m_Getter = () => propertyInfo.GetValue(m_Target, null);
                if (propertyInfo.CanWrite)
                    m_Setter = v => propertyInfo.SetValue(m_Target, v, null);
            }
            else throw new ArgumentException();
        }

        public void Draw(float indent = 20f)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(indent);
            GUILayout.Label(m_MemberInfo.Name, GUILayout.Width(150f));
            OnDrawField();
            GUILayout.EndHorizontal();
            OnDrawSubField();
        }

        protected abstract void OnDrawField();
        protected virtual void OnDrawSubField() { }
    }
}