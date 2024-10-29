/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
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
                    Debug.Log(e.Message);
                }
            }
        }

        public InspectorField(object target, MemberInfo memberInfo)
        {
            if (memberInfo is FieldInfo fi)
            {
                m_Target = target;
                m_MemberInfo = memberInfo;
                m_Getter = () => fi.GetValue(m_Target);
                m_Setter = value => fi.SetValue(m_Target, value);
            }
            else if (memberInfo is PropertyInfo pi)
            {
                m_Target = target;
                m_MemberInfo = memberInfo;
                m_Getter = () => pi.GetValue(m_Target, null);
                if (pi.CanWrite)
                    m_Setter = value => pi.SetValue(m_Target, value, null);
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