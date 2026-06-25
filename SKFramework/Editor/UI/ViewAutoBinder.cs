/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

using SK.Framework.UI;

namespace SK.Framework
{
    public class ViewAutoBinder
    {
        [MenuItem("CONTEXT/UIView/Auto Bind")]
        public static void AutoBind(MenuCommand command)
        {
            if (command.context is not UIView view)
                return;

            var type = view.GetType();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(f => f.IsDefined(typeof(SerializeField), true));

            Undo.RecordObject(view, "Auto Bind UIView");

            var childs = view.GetComponentsInChildren<RectTransform>(true);
            var typeBracketRegex = new Regex(@"^.*\[(?<type>[^\[\]]+?)\](?<varname>[^\[\]]*)$");
            
            foreach (var field in fields)
            {
                var fieldName = field.Name;
                if (!fieldName.StartsWith("m_"))
                    continue;
                var split = fieldName[2..];
                foreach (var child in childs)
                {
                    var match = typeBracketRegex.Match(child.name);
                    if (!match.Success)
                        continue;
                    var vartype = match.Groups["type"].Value;
                    var varname = match.Groups["varname"].Value.TrimStart('_');
                    var target = $"{vartype}{varname}";
                    if (target != split)
                        continue;
                    var component = child.GetComponent(field.FieldType);
                    if (component == null)
                        continue;
                    field.SetValue(view, component);
                    TryBindUnityEvent(view, vartype, varname, component);
                    break;
                }
            }
            EditorUtility.SetDirty(view);
        }

        private static void TryBindUnityEvent(UIView view, string type, string varname, Component component)
        {
            if (component is Button)
            {
                TryBindUnityEvent(component, "m_OnClick", view, $"On{type}{varname}Clicked", Type.EmptyTypes);
            }
            else if (component is Toggle)
            {
                TryBindUnityEvent(component, "onValueChanged", view, $"On{type}{varname}ValueChanged", new[] { typeof(bool) });
            }
            else if (component is Slider)
            {
                TryBindUnityEvent(component, "m_OnValueChanged", view, $"On{type}{varname}ValueChanged", new[] { typeof(float) });
            }
            else if (component is InputField)
            {
                TryBindUnityEvent(component, "m_OnEndEdit", view, $"On{type}{varname}EndEdit", new[] { typeof(string) });
                TryBindUnityEvent(component, "m_OnValueChanged", view, $"On{type}{varname}ValueChanged", new[] { typeof(string) });
            }
            else if (component is TMPro.TMP_InputField)
            {
                TryBindUnityEvent(component, "m_OnEndEdit", view, $"On{type}{varname}EndEdit", new[] { typeof(string) });
                TryBindUnityEvent(component, "m_OnValueChanged", view, $"On{type}{varname}ValueChanged", new[] { typeof(string) });
            }
            else if (component is Dropdown)
            {
                TryBindUnityEvent(component, "m_OnValueChanged", view, $"On{type}{varname}ValueChanged", new[] { typeof(int) });
            }
            else if (component is TMPro.TMP_Dropdown)
            {
                TryBindUnityEvent(component, "m_OnValueChanged", view, $"On{type}{varname}ValueChanged", new[] { typeof(int) });
            }
        }

        private static void TryBindUnityEvent(Component component, string eventPropName,
            UIView view, string methodName, Type[] paramTypes)
        {
            var method = view.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public, null, paramTypes, null);
            if (method == null)
                return;
            Undo.RecordObject(component, "Auto Bind Events");
            var so = new SerializedObject(component);
            var eventProp = so.FindProperty(eventPropName);
            if (eventProp == null)
                return;
            var callsProp = eventProp.FindPropertyRelative("m_PersistentCalls.m_Calls");
            if (callsProp == null) 
                return;
            for (int i = 0; i < callsProp.arraySize; i++)
            {
                var call = callsProp.GetArrayElementAtIndex(i);
                var targetProp = call.FindPropertyRelative("m_Target");
                var methodNameProp = call.FindPropertyRelative("m_MethodName");
                if (targetProp.objectReferenceValue == null || string.IsNullOrEmpty(methodNameProp.stringValue))
                {
                    callsProp.DeleteArrayElementAtIndex(i);
                    i--;
                    continue;
                }
                if (targetProp.objectReferenceValue == view && methodNameProp.stringValue == methodName)
                    return;
            }

            callsProp.arraySize++;
            var newCall = callsProp.GetArrayElementAtIndex(callsProp.arraySize - 1);
            newCall.FindPropertyRelative("m_Target").objectReferenceValue = view;
            newCall.FindPropertyRelative("m_MethodName").stringValue = methodName;
            newCall.FindPropertyRelative("m_Mode").enumValueIndex = (int)UnityEngine.Events.PersistentListenerMode.EventDefined;
            newCall.FindPropertyRelative("m_CallState").enumValueIndex = (int)UnityEngine.Events.UnityEventCallState.RuntimeOnly;

            so.ApplyModifiedProperties();
        }
    }
}