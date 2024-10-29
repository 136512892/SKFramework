/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;
using System.Reflection;

namespace SK.Framework.Debugger
{
    [InspectType(typeof(Color))]
    public class ColorField : InspectorField
    {
        public ColorField(object target, MemberInfo memberInfo)
            : base(target, memberInfo) { }

        protected override void OnDrawField()
        {
            Color oldValue = (Color)Value;
            GUILayout.Label("R", GUILayout.Width(10f));
            string rStr = GUILayout.TextField(oldValue.r.ToString());
            GUILayout.Label("G", GUILayout.Width(10f));
            string gStr = GUILayout.TextField(oldValue.g.ToString());
            GUILayout.Label("B", GUILayout.Width(10f));
            string bStr = GUILayout.TextField(oldValue.b.ToString());
            GUILayout.Label("A", GUILayout.Width(10f));
            string aStr = GUILayout.TextField(oldValue.a.ToString());

            if (float.TryParse(rStr, out float r) && r != oldValue.r)
            {
                oldValue.r = r;
                Value = oldValue;
            }
            else if (float.TryParse(gStr, out float g) && g != oldValue.g)
            {
                oldValue.g = g;
                Value = oldValue;
            }
            else if (float.TryParse(bStr, out float b) && b != oldValue.b)
            {
                oldValue.b = b;
                Value = oldValue;
            }
            else if (float.TryParse(aStr, out float a) && a != oldValue.a)
            {
                oldValue.a = a;
                Value = oldValue;
            }
        }
    }
}