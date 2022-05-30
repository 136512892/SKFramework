using System;
using UnityEditor;
using UnityEngine;
using System.Reflection;
using Object = UnityEngine.Object;

namespace SK.Framework
{
    /// <summary>
    /// 抽象编辑器类
    /// </summary>
    public abstract class AbstractEditor<E> : Editor where E : Object
    {
        protected E Target;
        private Texture csdnTex;
        private CSDNUrlAttribute csdnUrl;

        protected virtual bool IsEnableBaseInspectorGUI
        {
            get
            {
                return false;
            }
        }

        private void OnEnable()
        {
            Target = target as E;
            csdnUrl = GetType().GetCustomAttribute<CSDNUrlAttribute>();
            csdnTex = AssetDatabase.LoadAssetAtPath<Texture>("Assets/SKFramework/Editor/Main/Texture/CSDNLogo.png");

            OnBaseEnable();

            if (EditorApplication.isPlaying)
            {
                OnRuntimeEnable();
            }
        }

        public override void OnInspectorGUI()
        {
            if (csdnUrl != null && csdnTex != null)
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                GUI.enabled = !string.IsNullOrEmpty(csdnUrl.Url);
                if (GUILayout.Button(csdnTex, "IconButton", GUILayout.Width(16f), GUILayout.Height(16f)))
                {
                    Application.OpenURL(csdnUrl.Url);
                }
                GUI.enabled = true;
                GUILayout.EndHorizontal();
            }

            if (IsEnableBaseInspectorGUI)
            {
                base.OnInspectorGUI();
            }

            OnBaseInspectorGUI();

            if (EditorApplication.isPlaying)
            {
                Color color = GUI.color;
                GUI.color = Color.cyan;
                GUILayout.BeginVertical("Box");
                OnRuntimeInspectorGUI();
                GUILayout.EndVertical();
                GUI.color = color;
            }

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void OnBaseEnable() { }
        protected virtual void OnRuntimeEnable() { }

        protected virtual void OnBaseInspectorGUI() { }
        protected virtual void OnRuntimeInspectorGUI() { }

        protected void OnGUIEvent()
        {
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }

        #region GUILayout
        protected void GLButton(Action action, Texture image, params GUILayoutOption[] options)
        {
            if (GUILayout.Button(image, options))
            {
                Undo.RecordObject(target, "Button Click");
                action.Invoke();
                OnGUIEvent();
            }
        }
        protected void GLButton(Action action, string name, params GUILayoutOption[] options)
        {
            if (GUILayout.Button(name, options))
            {
                Undo.RecordObject(target, "Button Click");
                action.Invoke();
                OnGUIEvent();
            }
        }
        protected void GLButton(Action action, GUIContent content, params GUILayoutOption[] options)
        {
            if (GUILayout.Button(content, options))
            {
                Undo.RecordObject(target, "Button Click");
                action.Invoke();
                OnGUIEvent();
            }
        }
        protected void GLButton(Action action, Texture image, GUIStyle style, params GUILayoutOption[] options)
        {
            if (GUILayout.Button(image, style, options))
            {
                Undo.RecordObject(target, "Button Click");
                action.Invoke();
                OnGUIEvent();
            }
        }
        protected void GLButton(Action action, string name, GUIStyle style, params GUILayoutOption[] options)
        {
            if (GUILayout.Button(name, style, options))
            {
                Undo.RecordObject(target, "Button Click");
                action.Invoke();
                OnGUIEvent();
            }
        }
        protected void GLButton(Action action, GUIContent content, GUIStyle style, params GUILayoutOption[] options)
        {
            if (GUILayout.Button(content, style, options))
            {
                Undo.RecordObject(target, "Button Click");
                action.Invoke();
                OnGUIEvent();
            }
        }

        protected void GLToggle(Texture image, ref bool value, params GUILayoutOption[] options)
        {
            bool newValue = GUILayout.Toggle(value, image, options);
            if (value != newValue)
            {
                Undo.RecordObject(target, "Tool Value Changed");
                value = newValue;
                OnGUIEvent();
            }
        }
        protected void GLToggle(string text, ref bool value, params GUILayoutOption[] options)
        {
            bool newValue = GUILayout.Toggle(value, text, options);
            if (value != newValue)
            {
                Undo.RecordObject(target, "Tool Value Changed");
                value = newValue;
                OnGUIEvent();
            }
        }
        protected void GLToggle(GUIContent content, ref bool value, params GUILayoutOption[] options)
        {
            bool newValue = GUILayout.Toggle(value, content, options);
            if (value != newValue)
            {
                Undo.RecordObject(target, "Tool Value Changed");
                value = newValue;
                OnGUIEvent();
            }
        }
        protected void GLToggle(Texture image, ref bool value, GUIStyle style, params GUILayoutOption[] options)
        {
            bool newValue = GUILayout.Toggle(value, image, style, options);
            if (value != newValue)
            {
                Undo.RecordObject(target, "Tool Value Changed");
                value = newValue;
                OnGUIEvent();
            }
        }
        protected void GLToggle(string text, ref bool value, GUIStyle style, params GUILayoutOption[] options)
        {
            bool newValue = GUILayout.Toggle(value, text, style, options);
            if (value != newValue)
            {
                Undo.RecordObject(target, "Tool Value Changed");
                value = newValue;
                OnGUIEvent();
            }
        }
        protected void GLToggle(GUIContent content, ref bool value, GUIStyle style, params GUILayoutOption[] options)
        {
            bool newValue = GUILayout.Toggle(value, content, style, options);
            if (value != newValue)
            {
                Undo.RecordObject(target, "Tool Value Changed");
                value = newValue;
                OnGUIEvent();
            }
        }
        #endregion

        #region EditorGUILayout
        protected void EGLTextField(ref string text, params GUILayoutOption[] options)
        {
            string newText = EditorGUILayout.TextField(text, options);
            if (text != newText)
            {
                Undo.RecordObject(target, "Text Field Value Changed");
                text = newText;
                OnGUIEvent();
            }
        }
        protected void EGLTextField(ref string text, GUIStyle style, params GUILayoutOption[] options)
        {
            string newText = EditorGUILayout.TextField(text, style, options);
            if (text != newText)
            {
                Undo.RecordObject(target, "Text Field Value Changed");
                text = newText;
                OnGUIEvent();
            }
        }
        protected void EGLTextField(string label, ref string text, params GUILayoutOption[] options)
        {
            string newText = EditorGUILayout.TextField(label, text, options);
            if (text != newText)
            {
                Undo.RecordObject(target, "Text Field Value Changed");
                text = newText;
                OnGUIEvent();
            }
        }
        protected void EGLTextField(string label, ref string text, GUIStyle style, params GUILayoutOption[] options)
        {
            string newText = EditorGUILayout.TextField(label, text, style, options);
            if (text != newText)
            {
                Undo.RecordObject(target, "Text Field Value Changed");
                text = newText;
                OnGUIEvent();
            }
        }
        protected void EGLTextField(GUIContent content, ref string text, params GUILayoutOption[] options)
        {
            string newText = EditorGUILayout.TextField(content, text, options);
            if (text != newText)
            {
                Undo.RecordObject(target, "Text Field Value Changed");
                text = newText;
                OnGUIEvent();
            }
        }
        protected void EGLTextField(GUIContent content, ref string text, GUIStyle style, params GUILayoutOption[] options)
        {
            string newText = EditorGUILayout.TextField(content, text, style, options);
            if (text != newText)
            {
                Undo.RecordObject(target, "Text Field Value Changed");
                text = newText;
                OnGUIEvent();
            }
        }

        protected void EGLObjectField<T>(ref T t, bool allowSceneObjects, params GUILayoutOption[] options) where T : Object
        {
            T newValue = EditorGUILayout.ObjectField(t, typeof(T), allowSceneObjects, options) as T;
            if (t != newValue)
            {
                Undo.RecordObject(target, "Object Field Value Changed");
                t = newValue;
                OnGUIEvent();
            }
        }
        protected void EGLObjectField<T>(string label, ref T t, bool allowSceneObjects, params GUILayoutOption[] options) where T : Object
        {
            T newValue = EditorGUILayout.ObjectField(label, t, typeof(T), allowSceneObjects, options) as T;
            if (t != newValue)
            {
                Undo.RecordObject(target, "Object Field Value Changed");
                t = newValue;
                OnGUIEvent();
            }
        }
        protected void EGLObjectField<T>(GUIContent content, ref T t, bool allowSceneObjects, params GUILayoutOption[] options) where T : Object
        {
            T newValue = EditorGUILayout.ObjectField(content, t, typeof(T), allowSceneObjects, options) as T;
            if (t != newValue)
            {
                Undo.RecordObject(target, "Object Field Value Changed");
                t = newValue;
                OnGUIEvent();
            }
        }
        #endregion
    }
}