using System;
using System.Reflection;

using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace SK.Framework.UI
{
    [CustomEditor(typeof(UIView), true)]
    public class UIViewEditor : Editor
    {
        private UIViewAnimation openAnim;
        private UIViewAnimation closeAnim;
        private SerializedProperty onOpenEvent;
        private SerializedProperty onCloseEvent;

        private readonly GUIContent onVisibleContent = new GUIContent("On Open", "视图开启");
        private readonly GUIContent onInvisibleContent = new GUIContent("On Close", "视图关闭");

        private AnimBool visibleAnimBool;
        private AnimBool invisibleAnimBool;

        private AnimBool vMoveAnimBool;
        private AnimBool vRotateAnimBool;
        private AnimBool vScaleAnimBool;
        private AnimBool vFadeAnimBool;

        private AnimBool iMoveAnimBool;
        private AnimBool iRotateAnimBool;
        private AnimBool iScaleAnimBool;
        private AnimBool iFadeAnimBool;

        private const string visibleKey = "UIView Visible Foldout";
        private const string invisibleKey = "UIView Invisible Foldout";

        private void OnEnable()
        {
            visibleAnimBool = new AnimBool(EditorPrefs.HasKey(visibleKey) ? EditorPrefs.GetBool(visibleKey) : false, Repaint);
            invisibleAnimBool = new AnimBool(EditorPrefs.HasKey(invisibleKey) ? EditorPrefs.GetBool(invisibleKey) : false, Repaint);

            onOpenEvent = serializedObject.FindProperty("onOpenEvent");
            onCloseEvent = serializedObject.FindProperty("onCloseEvent");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (openAnim == null || closeAnim == null)
            {
                Type type = typeof(UIView);
                openAnim = type.GetField("openAnim", BindingFlags.Instance | BindingFlags.NonPublic)
                    .GetValue(target) as UIViewAnimation;
                closeAnim = type.GetField("closeAnim", BindingFlags.Instance | BindingFlags.NonPublic)
                    .GetValue(target) as UIViewAnimation;

                vMoveAnimBool = new AnimBool(openAnim.move.toggle, Repaint);
                vRotateAnimBool = new AnimBool(openAnim.rotate.toggle, Repaint);
                vScaleAnimBool = new AnimBool(openAnim.scale.toggle, Repaint);
                vFadeAnimBool = new AnimBool(openAnim.fade.toggle, Repaint);

                iMoveAnimBool = new AnimBool(closeAnim.move.toggle, Repaint);
                iRotateAnimBool = new AnimBool(closeAnim.rotate.toggle, Repaint);
                iScaleAnimBool = new AnimBool(closeAnim.scale.toggle, Repaint);
                iFadeAnimBool = new AnimBool(closeAnim.fade.toggle, Repaint);

                openAnim.move.moveMode = MoveAnimation.MoveMode.MoveIn;
                closeAnim.move.moveMode = MoveAnimation.MoveMode.MoveOut;
            }

            DrawEvent(onVisibleContent, openAnim, visibleAnimBool, visibleKey,
                vMoveAnimBool, vRotateAnimBool, vScaleAnimBool, vFadeAnimBool, true);

            DrawEvent(onInvisibleContent, closeAnim, invisibleAnimBool, invisibleKey,
                iMoveAnimBool, iRotateAnimBool, iScaleAnimBool, iFadeAnimBool, false);
        }

        private void DrawEvent(GUIContent content, UIViewAnimation e, AnimBool animBool, string prefsKey,
            AnimBool mAnimBool, AnimBool rAnimBool, AnimBool sAnimBool, AnimBool fAnimBool, bool visiable)
        {
            bool newValue = EditorGUILayout.Foldout(animBool.target, content, true);
            if (newValue != animBool.target)
            {
                animBool.target = newValue;
                EditorPrefs.SetBool(prefsKey, animBool.target);
            }
            if (EditorGUILayout.BeginFadeGroup(animBool.faded))
            {
                DrawAnimation(e, mAnimBool, rAnimBool, sAnimBool, fAnimBool, visiable);
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(visiable ? onOpenEvent : onCloseEvent);
                if (EditorGUI.EndChangeCheck())
                {
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(target);
                }
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void DrawAnimation(UIViewAnimation animations,
            AnimBool mAnimBool, AnimBool rAnimBool, AnimBool sAnimBool, AnimBool fAnimBool, bool visiable)
        {
            GUILayout.Space(5f);
            //Move、Rotate、Scale、Fade
            GUILayout.BeginHorizontal(GUILayout.Height(25f));
            {
                GUILayout.Space(5f);
                AnimationsDrawer.DrawMoveToggle(animations.move, mAnimBool, target, "ButtonLeft");
                AnimationsDrawer.DrawRotateToggle(animations.rotate, rAnimBool, target, "ButtonMid");
                AnimationsDrawer.DrawScaleToggle(animations.scale, sAnimBool, target, "ButtonMid");
                AnimationsDrawer.DrawFadeToggle(animations.fade, fAnimBool, target, "ButtonRight");
            }
            GUILayout.EndHorizontal();
            //MoveAnimation
            AnimationsDrawer.DrawMove(animations.move, mAnimBool, target, visiable);
            GUILayout.Space(3f);
            //RotateAnimation
            AnimationsDrawer.DrawRotate(animations.rotate, rAnimBool, target);
            GUILayout.Space(3f);
            //ScaleAnimation
            AnimationsDrawer.DrawScale(animations.scale, sAnimBool, target);
            GUILayout.Space(3f);
            //FadeAnimation
            AnimationsDrawer.DrawFade(animations.fade, fAnimBool, target);
            GUILayout.Space(5f);
        }
    }
}