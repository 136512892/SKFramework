#if UNITY_EDITOR
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
        private ViewAnimationEvent onVisible;
        private ViewAnimationEvent onInvisible;

        private enum Menu
        {
            Animation,
            Event,
            Sound
        }
        private Menu onVisibleMenu = Menu.Animation;
        private Menu onInvisibleMenu = Menu.Animation;

        private readonly GUIContent onVisibleContent = new GUIContent("On Visible", "视图加载、显示");
        private readonly GUIContent onInvisibleContent = new GUIContent("On Invisible", "视图卸载、隐藏");

        private SerializedProperty onVisibleBeganEvent;
        private SerializedProperty onVisibleEndEvent;
        private SerializedProperty onInvisibleBeganEvent;
        private SerializedProperty onInvisibleEndEvent;

        private SerializedProperty onVisibleBeganSound;
        private SerializedProperty onVisibleEndSound;
        private SerializedProperty onInvisibleBeganSound;
        private SerializedProperty onInvisibleEndSound;

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
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (onVisible == null)
            {
                Type type = typeof(UIView);
                onVisible = type.GetField("onVisible", BindingFlags.Instance | BindingFlags.NonPublic)
                    .GetValue(target) as ViewAnimationEvent;
                onInvisible = type.GetField("onInvisible", BindingFlags.Instance | BindingFlags.NonPublic)
                    .GetValue(target) as ViewAnimationEvent;

                var vso = serializedObject.FindProperty("onVisible");
                onVisibleBeganEvent = vso.FindPropertyRelative("onBeganEvent");
                onVisibleEndEvent = vso.FindPropertyRelative("onEndEvent");
                onVisibleBeganSound = vso.FindPropertyRelative("onBeganSound");
                onVisibleEndSound = vso.FindPropertyRelative("onEndSound");

                var iso = serializedObject.FindProperty("onInvisible");
                onInvisibleBeganEvent = iso.FindPropertyRelative("onBeganEvent");
                onInvisibleEndEvent = iso.FindPropertyRelative("onEndEvent");
                onInvisibleBeganSound = iso.FindPropertyRelative("onBeganSound");
                onInvisibleEndSound = iso.FindPropertyRelative("onEndSound");

                vMoveAnimBool = new AnimBool(onVisible.animation.animations.move.toggle, Repaint);
                vRotateAnimBool = new AnimBool(onVisible.animation.animations.rotate.toggle, Repaint);
                vScaleAnimBool = new AnimBool(onVisible.animation.animations.scale.toggle, Repaint);
                vFadeAnimBool = new AnimBool(onVisible.animation.animations.fade.toggle, Repaint);

                iMoveAnimBool = new AnimBool(onInvisible.animation.animations.move.toggle, Repaint);
                iRotateAnimBool = new AnimBool(onInvisible.animation.animations.rotate.toggle, Repaint);
                iScaleAnimBool = new AnimBool(onInvisible.animation.animations.scale.toggle, Repaint);
                iFadeAnimBool = new AnimBool(onInvisible.animation.animations.fade.toggle, Repaint);

                onVisible.animation.animations.move.moveMode = MoveAnimation.MoveMode.MoveIn;
                onInvisible.animation.animations.move.moveMode = MoveAnimation.MoveMode.MoveOut;
            }

            DrawEvent(onVisibleContent, onVisible, ref onVisibleMenu, visibleAnimBool, visibleKey,
                onVisibleBeganEvent, onVisibleEndEvent, onVisibleBeganSound, onVisibleEndSound,
                vMoveAnimBool, vRotateAnimBool, vScaleAnimBool, vFadeAnimBool, true);

            DrawEvent(onInvisibleContent, onInvisible, ref onInvisibleMenu, invisibleAnimBool, invisibleKey,
                onInvisibleBeganEvent, onInvisibleEndEvent, onInvisibleBeganSound, onInvisibleEndSound,
                iMoveAnimBool, iRotateAnimBool, iScaleAnimBool, iFadeAnimBool, false);
        }

        private void DrawEvent(GUIContent content, ViewAnimationEvent e, ref Menu menu, AnimBool animBool, string prefsKey,
            SerializedProperty onBeganEvent, SerializedProperty onEndEvent,
            SerializedProperty onBeganSound, SerializedProperty onEndSound,
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
                Color cacheColor = GUI.color;
                Color alphaColor = new Color(cacheColor.r, cacheColor.g, cacheColor.b, .4f);
                GUILayout.BeginHorizontal();
                GUI.color = menu == Menu.Animation ? cacheColor : alphaColor;
                if (GUILayout.Button("Animation", "ButtonLeft")) menu = Menu.Animation;
                GUI.color = menu == Menu.Event ? cacheColor : alphaColor;
                if (GUILayout.Button("Unity Event", "ButtonMid")) menu = Menu.Event;
                GUI.color = menu == Menu.Sound ? cacheColor : alphaColor;
                if (GUILayout.Button("Sound", "ButtonRight")) menu = Menu.Sound;
                GUI.color = cacheColor;
                GUILayout.EndHorizontal();

                switch (menu)
                {
                    case Menu.Animation:
                        ViewAnimation viewAnimation = e.animation;
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Type", GUILayout.Width(40f));
                        var newType = (AnimationType)EditorGUILayout.EnumPopup(viewAnimation.type);
                        if (newType != viewAnimation.type)
                        {
                            Undo.RecordObject(target, "Animation Type");
                            viewAnimation.type = newType;
                        }
                        GUILayout.EndHorizontal();

                        switch (viewAnimation.type)
                        {
                            case AnimationType.Tween:
                                DrawAnimation(viewAnimation.animations, mAnimBool, rAnimBool, sAnimBool, fAnimBool, visiable);
                                break;
                            case AnimationType.Animator:
                                GUILayout.BeginHorizontal();
                                GUILayout.Label("State Name", GUILayout.Width(80f));
                                string newText = EditorGUILayout.TextField(viewAnimation.stateName);
                                if (viewAnimation.stateName != newText)
                                {
                                    Undo.RecordObject(target, "Animator State Name");
                                    viewAnimation.stateName = newText;
                                    if (GUI.changed)
                                    {
                                        EditorUtility.SetDirty(target);
                                    }
                                }
                                GUILayout.EndHorizontal();
                                break;
                            default:
                                break;
                        }
                        break;
                    case Menu.Event:
                        EditorGUILayout.PropertyField(onBeganEvent);
                        EditorGUILayout.PropertyField(onEndEvent);
                        HasChanged();
                        break;
                    case Menu.Sound:
                        EditorGUILayout.PropertyField(onBeganSound);
                        EditorGUILayout.PropertyField(onEndSound);
                        HasChanged();
                        break;
                }
            }
            EditorGUILayout.EndFadeGroup();
        }

        private void DrawAnimation(TweenAnimations animations, 
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
        private void HasChanged()
        {
            if (GUI.changed)
            {
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }
        }
    }
}
#endif