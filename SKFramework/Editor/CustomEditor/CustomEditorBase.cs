using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using Object = UnityEngine.Object;

namespace SK.Framework
{
    public abstract class CustomEditorBase<T> : Editor where T : Object
    {
        protected T _target;

        protected virtual void OnEnable()
        {
            _target = target as T;
        }

        protected bool Foldout(float spaceWidth, string label, bool value, bool toggleOnLabelClick)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(spaceWidth);
            bool boolValue = EditorGUILayout.Foldout(value, label, toggleOnLabelClick);
            GUILayout.EndHorizontal();
            return boolValue;
        }
        protected bool Foldout(float spaceWidth, string label, AnimBool animBool, bool toggleOnLabelClick)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(spaceWidth);
            animBool.target = EditorGUILayout.Foldout(animBool.target, label, toggleOnLabelClick);
            GUILayout.EndHorizontal();
            return animBool.target;
        }

        protected void FadeGroup(AnimBool animBool, Action onGUI)
        {
            if (EditorGUILayout.BeginFadeGroup(animBool.faded))
            {
                onGUI();
            }
            EditorGUILayout.EndFadeGroup();
        }
    }
}