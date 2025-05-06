/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;
using System.Reflection;

namespace SK.Framework.Debugger
{
    [InspectType(typeof(bool))]
    public class BoolField : InspectorField
    {
        public BoolField(object target, MemberInfo memberInfo) : base(target, memberInfo) { }

        protected override void OnDrawField()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(20f);
            GUILayout.Label(m_MemberInfo.Name, GUILayout.Width(150f));
            bool oldValue = (bool)Value;
            bool newValue = GUILayout.Toggle(oldValue, string.Empty);
            if (newValue != oldValue)
                Value = newValue;
            GUILayout.EndHorizontal();
        }
    }
}