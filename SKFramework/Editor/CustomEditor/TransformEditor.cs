using System.Text;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace SK.Framework
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Transform))]
    public class TransformEditor : CustomEditorBase<Transform>
    {
        private Editor instance;

        private const string originFoldoutKey = "Transform_Original";
        private const string extensionFoldoutKey = "Transform_Extension";

        private AnimBool originAnimBool;
        private AnimBool extensionAnimBool;

        protected override void OnEnable()
        {
            base.OnEnable();

            var editorType = Assembly.GetAssembly(typeof(Editor)).GetTypes().FirstOrDefault(m => m.Name == "TransformInspector");
            instance = CreateEditor(targets, editorType);

            originAnimBool = new AnimBool(EditorPrefs.HasKey(originFoldoutKey) && EditorPrefs.GetBool(originFoldoutKey), Repaint);
            extensionAnimBool = new AnimBool(EditorPrefs.HasKey(extensionFoldoutKey) && EditorPrefs.GetBool(extensionFoldoutKey), Repaint);
        }

        private void OnDisable()
        {
            EditorPrefs.SetBool(originFoldoutKey, originAnimBool.target);
            EditorPrefs.SetBool(extensionFoldoutKey, extensionAnimBool.target);

            if (instance)
            {
                DestroyImmediate(instance);
            }
        }

        public override void OnInspectorGUI()
        {
            GUILayout.BeginVertical(originAnimBool.target ? "Badge" : "SelectionRect");
            Foldout(12f, "Original", originAnimBool, true);
            FadeGroup(originAnimBool, OnOriginalGUI);
            GUILayout.EndVertical();

            GUILayout.Space(1f);

            GUILayout.BeginVertical(extensionAnimBool.target ? "Badge" : "SelectionRect");
            Foldout(12f, "Extension", extensionAnimBool, true);
            FadeGroup(extensionAnimBool, OnExtensionGUI);
            GUILayout.EndVertical();
        }

        private void OnOriginalGUI()
        {
            instance.OnInspectorGUI();
        }
        private void OnExtensionGUI()
        {
            if (GUILayout.Button("Copy Full Path"))
            {
                List<Transform> tfs = new List<Transform>();
                Transform tf = _target.transform;
                tfs.Add(tf);
                while (tf.parent)
                {
                    tf = tf.parent;
                    tfs.Add(tf);
                }
                StringBuilder sb = new StringBuilder();
                sb.Append(tfs[tfs.Count - 1].name);
                for (int i = tfs.Count - 2; i >= 0; i--)
                {
                    sb.Append("/" + tfs[i].name);
                }
                GUIUtility.systemCopyBuffer = sb.ToString();
            }
        }
    }
}