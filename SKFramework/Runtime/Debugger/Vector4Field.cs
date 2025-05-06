/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;
using System.Reflection;

namespace SK.Framework.Debugger
{
    [InspectType(typeof(Vector4))]
    public class Vector4Field : InspectorField
    {
        public Vector4Field(object target, MemberInfo memberInfo) : base(target, memberInfo) { }

        protected override void OnDrawField()
        {
            Vector4 oldValue = (Vector4)Value;
            GUILayout.Label("X", GUILayout.Width(10f));
            string xStr = GUILayout.TextField(oldValue.x.ToString());
            GUILayout.Label("Y", GUILayout.Width(10f));
            string yStr = GUILayout.TextField(oldValue.y.ToString());
            GUILayout.Label("Z", GUILayout.Width(10f));
            string zStr = GUILayout.TextField(oldValue.z.ToString());
            GUILayout.Label("W", GUILayout.Width(10f));
            string wStr = GUILayout.TextField(oldValue.w.ToString());

            if (float.TryParse(xStr, out float x) && x != oldValue.x)
            {
                oldValue.x = x;
                Value = oldValue;
            }
            else if (float.TryParse(yStr, out float y) && y != oldValue.y)
            {
                oldValue.y = y;
                Value = oldValue;
            }
            else if (float.TryParse(zStr, out float z) && z != oldValue.z)
            {
                oldValue.z = z;
                Value = oldValue;
            }
            else if (float.TryParse(wStr, out float w) && w != oldValue.w)
            {
                oldValue.w = w;
                Value = oldValue;
            }
        }
    }
}