/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;
using System.Reflection;

namespace SK.Framework.Debugger
{
    [InspectType(typeof(string))]
    public class StringField : InspectorField
    {
        public StringField(object target, MemberInfo memberInfo) : base(target, memberInfo) { }

        protected override void OnDrawField()
        {
            string oldValue = Value as string;
            string newValue = GUILayout.TextField(oldValue);
            if (newValue != oldValue)
                Value = newValue;
        }
    }
}