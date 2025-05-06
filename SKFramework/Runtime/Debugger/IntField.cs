/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;
using System.Reflection;

namespace SK.Framework.Debugger
{
    [InspectType(typeof(int))]
    public class IntField : InspectorField
    {
        public IntField(object target, MemberInfo memberInfo) : base(target, memberInfo) { }

        protected override void OnDrawField()
        {
            int oldValue = (int)Value;
            if (int.TryParse(GUILayout.TextField(oldValue.ToString()),
                out int newValue) && newValue != oldValue)
                Value = newValue;
        }
    }
}