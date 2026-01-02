/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;

namespace SK.Framework.Debugger
{
    public class ObjectField : InspectorField
    {
        private readonly List<InspectorField> m_FieldList = new List<InspectorField>();

        public float indent;

        public ObjectField(object target, MemberInfo memberInfo, Dictionary<Type, Type> inspectDic, float indent = 20f) : base(target, memberInfo)
        {
            this.indent = indent + 20f;
            MemberInfo[] mis = target.GetType().GetMembers(BindingFlags.Instance
                | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                    .Where(m => m.GetCustomAttribute<ObsoleteAttribute>() == null
                        && ((m is FieldInfo fi && (fi.IsPublic || fi.GetCustomAttribute<SerializeField>() != null))
                            || (m is PropertyInfo pi && pi.GetGetMethod(true).IsPublic)))
                    .ToArray();
            for (int i = 0; i < mis.Length; i++)
            {
                MemberInfo mi = mis[i];
                Type subType = null;
                object subTarget = null;
                if (mi is FieldInfo fi)
                {
                    subType = fi.FieldType;
                    subTarget = fi.GetValue(target);
                }
                else if (mi is PropertyInfo pi)
                {
                    subType = pi.PropertyType;
                    subTarget = pi.GetValue(target);
                }
                if (inspectDic.TryGetValue(subType, out Type inspectorFieldType))
                {
                    if (Activator.CreateInstance(inspectorFieldType,
                        target, mi) is InspectorField inspectorField)
                        m_FieldList.Add(inspectorField);
                }
                else if (!subType.IsPrimitive && subType.GetCustomAttribute<SerializableAttribute>() != null)
                {
                    m_FieldList.Add(new ObjectField(subTarget, mi, inspectDic, this.indent));
                }
            }
        }

        protected override void OnDrawField()
        {

        }
        protected override void OnDrawSubField()
        {
            for (int i = 0; i < m_FieldList.Count; i++)
                m_FieldList[i].Draw(indent);
        }
    }
}