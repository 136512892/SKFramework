using System.Linq;
using System.Reflection;

using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace SK.Framework
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(RectTransform))]
    public class RectTransformEditor : CustomEditorBase<RectTransform>
    {
        private Editor instance;
        private MethodInfo onSceneGUI;
        private static readonly object[] emptyArray = new object[0];

        private const string originFoldoutKey = "RectTransform_Original";
        private const string extensionFoldoutKey = "RectTransform_Extension";

        private AnimBool originAnimBool;
        private AnimBool extensionAnimBool;

        protected override void OnEnable()
        {
            base.OnEnable();

            var editorType = Assembly.GetAssembly(typeof(Editor)).GetTypes().FirstOrDefault(m => m.Name == "RectTransformEditor");
            instance = CreateEditor(targets, editorType);
            onSceneGUI = editorType.GetMethod("OnSceneGUI", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

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
            GUILayout.BeginHorizontal();
            Vector3 newPos = EditorGUILayout.Vector3Field("Position", _target.localPosition);
            if (newPos != _target.localPosition)
            {
                Undo.RecordObject(_target, "Local Position");
                _target.localPosition = newPos;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            Vector2 size = EditorGUILayout.Vector2Field("Size", _target.rect.size);
            if (size != _target.rect.size)
            {
                Undo.RecordObject(_target, "Size");
                _target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
                _target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5f);
            if (GUILayout.Button("Auto Anchors"))
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    RectTransform tempTarget = targets[i] as RectTransform;
                    Undo.RecordObject(tempTarget, "Auto Anchors");
                    RectTransform prt = tempTarget.parent as RectTransform;
                    Vector2 anchorMin = new Vector2(
                        tempTarget.anchorMin.x + tempTarget.offsetMin.x / prt.rect.width,
                        tempTarget.anchorMin.y + tempTarget.offsetMin.y / prt.rect.height);
                    Vector2 anchorMax = new Vector2(
                        tempTarget.anchorMax.x + tempTarget.offsetMax.x / prt.rect.width,
                        tempTarget.anchorMax.y + tempTarget.offsetMax.y / prt.rect.height);
                    tempTarget.anchorMin = anchorMin;
                    tempTarget.anchorMax = anchorMax;
                    tempTarget.offsetMin = tempTarget.offsetMax = Vector2.zero;
                }
            }
        }

        private void OnSceneGUI()
        {
            if (instance)
            {
                onSceneGUI.Invoke(instance, emptyArray);
            }
        }
    }
}