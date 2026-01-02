/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;
using System.Reflection;

namespace SK.Framework.Debugger
{
    [InspectType(typeof(float))]
    public class FloatField : InspectorField
    {
        public FloatField(object target, MemberInfo memberInfo) : base(target, memberInfo) { }

        protected override void OnDrawField()
        {
            float oldValue = (float)Value;
            if (float.TryParse(GUILayout.TextField(oldValue.ToString()),
                out float newValue) && newValue != oldValue)
                Value = newValue;
        }
    }
}