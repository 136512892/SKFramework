/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;
using System.Reflection;

namespace SK.Framework.Debugger
{
    [InspectType(typeof(Vector2))]
    public class Vector2Field : InspectorField
    {
        public Vector2Field(object target, MemberInfo memberInfo)
            : base(target, memberInfo) { }

        protected override void OnDrawField()
        {
            Vector2 oldValue = (Vector2)Value;
            GUILayout.Label("X", GUILayout.Width(10f));
            string xStr = GUILayout.TextField(oldValue.x.ToString());
            GUILayout.Label("Y", GUILayout.Width(10f));
            string yStr = GUILayout.TextField(oldValue.y.ToString());

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
        }
    }
}