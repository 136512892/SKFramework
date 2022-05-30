using System;
using UnityEditor;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace SK.Framework
{
    [CustomEditor(typeof(UIView), true)]
    [CSDNUrl("https://coderz.blog.csdn.net/article/details/124740012?spm=1001.2014.3001.5502")]
    public class UIViewInspector : AbstractEditor<UIView>
    {
        private ViewAnimationEvent onVisible;
        private ViewAnimationEvent onInvisible;

        private SerializedProperty onVisibleBegan;
        private SerializedProperty onVisibleEnd;

        private SerializedProperty onVisibleBeginSound;
        private SerializedProperty onVisibleEndSound;

        private bool visibleFoldout;
        private bool invisibleFoldout;

        private const float labelWidth = 60f;

        private Dictionary<UIAnimation, bool> foldoutDic = new Dictionary<UIAnimation, bool>();

        private enum Menu
        {
            Animation,
            Event,
            Sound
        }

        private Menu onVisibleMenu = Menu.Animation;
        private Menu onInvisibleMenu = Menu.Animation;

        private static class GUICONTENTS
        {
            public static GUIContent Animation = new GUIContent("Animation");
            public static GUIContent Event = new GUIContent("Event");
            public static GUIContent Sound = new GUIContent("Sound");

            public static GUIContent MoveTool = EditorGUIUtility.IconContent("MoveTool");
            public static GUIContent RotateTool = EditorGUIUtility.IconContent("RotateTool");
            public static GUIContent ScaleTool = EditorGUIUtility.IconContent("ScaleTool");
            public static GUIContent ViewTool = EditorGUIUtility.IconContent("ViewToolOrbit");
            public static GUIContent Trash = EditorGUIUtility.IconContent("TreeEditor.Trash");

            public static GUIContent Duration = new GUIContent("Duration");
            public static GUIContent Delay = new GUIContent("Delay");
            public static GUIContent From = new GUIContent("From");
            public static GUIContent To = new GUIContent("To");
            public static GUIContent Ease = new GUIContent("Ease");
            public static GUIContent RotateMode = new GUIContent("Mode", "Rotate Mode");
            public static GUIContent Direction = new GUIContent("Direction");
            public static GUIContent CustomPosition = new GUIContent("Custom Position");
            public static GUIContent CurrentRotation = new GUIContent("Current Rotation");
            public static GUIContent FixedRotation = new GUIContent("Fixed Rotation");
            public static GUIContent CurrentScale = new GUIContent("Current Scale");
            public static GUIContent FixedScale = new GUIContent("Fixed Scale");
            public static GUIContent CurrentAlpha = new GUIContent("Current Alpha");
            public static GUIContent FixedAlpha = new GUIContent("Fixed Alpha");
        }

        protected override bool IsEnableBaseInspectorGUI
        {
            get
            {
                return true;
            }
        }

        protected override void OnBaseInspectorGUI()
        {
            if (onVisible == null)
            {
                Type type = typeof(UIView);
                onVisible = type.GetField("onVisible", BindingFlags.Instance | BindingFlags.NonPublic)
                    .GetValue(target) as ViewAnimationEvent;
                onInvisible = type.GetField("onInvisible", BindingFlags.Instance | BindingFlags.NonPublic)
                    .GetValue(target) as ViewAnimationEvent;

                onVisibleBegan = serializedObject.FindProperty("onVisible").FindPropertyRelative("onBegan");
                onVisibleEnd = serializedObject.FindProperty("onVisible").FindPropertyRelative("onEnd");

                onVisibleBeginSound = serializedObject.FindProperty("onVisible").FindPropertyRelative("beginSound");
                onVisibleEndSound = serializedObject.FindProperty("onVisible").FindPropertyRelative("endSound");
            }

            Color cacheColor = GUI.color;
            Color alphaColor = cacheColor.Alpha(.5f);

            visibleFoldout = EditorGUILayout.Foldout(visibleFoldout, "On Visible", true);
            if (visibleFoldout)
            {
                GUILayout.BeginHorizontal();
                GUI.color = onVisibleMenu == Menu.Animation ? cacheColor : alphaColor;
                if (GUILayout.Button(GUICONTENTS.Animation, "ButtonLeft"))
                {
                    onVisibleMenu = Menu.Animation;
                }
                GUI.color = onVisibleMenu == Menu.Event ? cacheColor : alphaColor;
                if (GUILayout.Button(GUICONTENTS.Event, "ButtonMid"))
                {
                    onVisibleMenu = Menu.Event;
                }
                GUI.color = onVisibleMenu == Menu.Sound ? cacheColor : alphaColor;
                if (GUILayout.Button(GUICONTENTS.Sound, "ButtonRight"))
                {
                    onVisibleMenu = Menu.Sound;
                }
                GUI.color = cacheColor;
                GUILayout.EndHorizontal();
                switch (onVisibleMenu)
                {
                    case Menu.Animation:
                        ViewAnimation viewAnimation = onVisible.animation;
                        var newType = (UIAnimationType)EditorGUILayout.EnumPopup(viewAnimation.type);
                        if (newType != viewAnimation.type)
                        {
                            Undo.RecordObject(target, "Visible Animation Type");
                            viewAnimation.type = newType;
                        }

                        switch (viewAnimation.type)
                        {
                            case UIAnimationType.Tween:
                                if (viewAnimation.actors.Count == 0)
                                {
                                    UIAnimationActor actor = new UIAnimationActor()
                                    {
                                        executer = Target,
                                        actor = Target.RectTransform,
                                        animation = new UIAnimation()
                                        {
                                            moveAnimation = new UIMoveAnimation() { moveMode = UIMoveAnimation.MoveMode.MoveIn }
                                        }
                                    };
                                    viewAnimation.actors.Add(actor);
                                    EditorUtility.SetDirty(target);
                                }
                                var animation = viewAnimation.actors[0].animation;
                                AnimationDrawer("Main", animation, true);
                                if (viewAnimation.actors.Count > 1)
                                {
                                    for (int i = 1; i < viewAnimation.actors.Count; i++)
                                    {
                                        var actor = viewAnimation.actors[i];
                                        AnimationDrawer(actor.actor.name, actor.animation, false);
                                    }
                                }

                                GUILayout.BeginHorizontal();
                                GUILayout.Label(GUIContent.none, GUILayout.ExpandWidth(true));
                                Rect lastRect = GUILayoutUtility.GetLastRect();
                                var dropRect = new Rect(lastRect.x + 2f, lastRect.y - 2f, lastRect.width, 20f);
                                bool containsMouse = dropRect.Contains(Event.current.mousePosition);
                                if (containsMouse)
                                {
                                    switch (Event.current.type)
                                    {
                                        case EventType.DragUpdated:
                                            bool containsRectTransform = DragAndDrop.objectReferences.OfType<GameObject>().Select(m => m.GetComponent<RectTransform>()).Any();
                                            DragAndDrop.visualMode = containsRectTransform ? DragAndDropVisualMode.Copy : DragAndDropVisualMode.Rejected;
                                            Event.current.Use();
                                            Repaint();
                                            break;
                                        case EventType.DragPerform:
                                            IEnumerable<RectTransform> rts = DragAndDrop.objectReferences.OfType<GameObject>().Select(m => m.GetComponent<RectTransform>());
                                            foreach (RectTransform t in rts)
                                            {
                                                UIAnimationActor actor = new UIAnimationActor()
                                                {
                                                    executer = Target,
                                                    actor = t,
                                                    animation = new UIAnimation()
                                                    {
                                                        moveAnimation = new UIMoveAnimation() { moveMode = UIMoveAnimation.MoveMode.MoveIn }
                                                    }
                                                };
                                                viewAnimation.actors.Add(actor);
                                                EditorUtility.SetDirty(target);
                                            }
                                            break;
                                    }
                                }
                                GUILayout.EndHorizontal();
                                Color color = GUI.color;
                                GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, containsMouse ? .9f : .5f);
                                GUI.Box(dropRect, "Drop Animation Element Here", new GUIStyle(GUI.skin.box) { fontSize = 10 });
                                GUI.color = color;
                                break;
                            case UIAnimationType.Animator:
                                GUILayout.BeginHorizontal();
                                GUILayout.Label("State Name", GUILayout.Width(100f));
                                EGLTextField(ref viewAnimation.stateName);
                                GUILayout.EndHorizontal();
                                break;
                            default:
                                break;
                        }
                        break;
                    case Menu.Event:
                        EditorGUILayout.PropertyField(onVisibleBegan);
                        EditorGUILayout.PropertyField(onVisibleEnd);
                        break;
                    case Menu.Sound:
                        EditorGUILayout.PropertyField(onVisibleBeginSound);
                        EditorGUILayout.PropertyField(onVisibleEndSound);
                        break;
                }
            }

            invisibleFoldout = EditorGUILayout.Foldout(invisibleFoldout, "On Invisible", true);
            if (invisibleFoldout)
            {
                GUILayout.BeginHorizontal();
                GUI.color = onInvisibleMenu == Menu.Animation ? cacheColor : alphaColor;
                if (GUILayout.Button(GUICONTENTS.Animation, "ButtonLeft"))
                {
                    onInvisibleMenu = Menu.Animation;
                }
                GUI.color = onInvisibleMenu == Menu.Event ? cacheColor : alphaColor;
                if (GUILayout.Button(GUICONTENTS.Event, "ButtonMid"))
                {
                    onInvisibleMenu = Menu.Event;
                }
                GUI.color = onInvisibleMenu == Menu.Sound ? cacheColor : alphaColor;
                if (GUILayout.Button(GUICONTENTS.Sound, "ButtonRight"))
                {
                    onInvisibleMenu = Menu.Sound;
                }
                GUI.color = cacheColor;
                GUILayout.EndHorizontal();
                switch (onInvisibleMenu)
                {
                    case Menu.Animation:
                        ViewAnimation viewAnimation = onInvisible.animation;
                        var newType = (UIAnimationType)EditorGUILayout.EnumPopup(viewAnimation.type);
                        if (newType != viewAnimation.type)
                        {
                            Undo.RecordObject(target, "Invisible Animation Type");
                            viewAnimation.type = newType;
                        }

                        switch (viewAnimation.type)
                        {
                            case UIAnimationType.Tween:
                                if (viewAnimation.actors.Count == 0)
                                {
                                    UIAnimationActor actor = new UIAnimationActor()
                                    {
                                        executer = Target,
                                        actor = Target.RectTransform,
                                        animation = new UIAnimation()
                                        {
                                            moveAnimation = new UIMoveAnimation() { moveMode = UIMoveAnimation.MoveMode.MoveOut }
                                        }
                                    };
                                    viewAnimation.actors.Add(actor);
                                    EditorUtility.SetDirty(target);
                                }
                                var animation = viewAnimation.actors[0].animation;
                                AnimationDrawer("Main", animation, true);
                                if (viewAnimation.actors.Count > 1)
                                {
                                    for (int i = 1; i < viewAnimation.actors.Count; i++)
                                    {
                                        var actor = viewAnimation.actors[i];
                                        AnimationDrawer(actor.actor.name, actor.animation, false);
                                    }
                                }

                                GUILayout.BeginHorizontal();
                                GUILayout.Label(GUIContent.none, GUILayout.ExpandWidth(true));
                                Rect lastRect = GUILayoutUtility.GetLastRect();
                                var dropRect = new Rect(lastRect.x + 2f, lastRect.y - 2f, lastRect.width, 20f);
                                bool containsMouse = dropRect.Contains(Event.current.mousePosition);
                                if (containsMouse)
                                {
                                    switch (Event.current.type)
                                    {
                                        case EventType.DragUpdated:
                                            bool containsRectTransform = DragAndDrop.objectReferences.OfType<GameObject>().Select(m => m.GetComponent<RectTransform>()).Any();
                                            DragAndDrop.visualMode = containsRectTransform ? DragAndDropVisualMode.Copy : DragAndDropVisualMode.Rejected;
                                            Event.current.Use();
                                            Repaint();
                                            break;
                                        case EventType.DragPerform:
                                            IEnumerable<RectTransform> rts = DragAndDrop.objectReferences.OfType<GameObject>().Select(m => m.GetComponent<RectTransform>());
                                            foreach (RectTransform t in rts)
                                            {
                                                UIAnimationActor actor = new UIAnimationActor()
                                                {
                                                    executer = Target,
                                                    actor = t,
                                                    animation = new UIAnimation()
                                                    {
                                                        moveAnimation = new UIMoveAnimation() { moveMode = UIMoveAnimation.MoveMode.MoveOut }
                                                    }
                                                };
                                                viewAnimation.actors.Add(actor);
                                                EditorUtility.SetDirty(target);
                                            }
                                            break;
                                    }
                                }
                                GUILayout.EndHorizontal();
                                Color color = GUI.color;
                                GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, containsMouse ? .9f : .5f);
                                GUI.Box(dropRect, "Drop Animation Element Here", new GUIStyle(GUI.skin.box) { fontSize = 10 });
                                GUI.color = color;
                                break;
                            case UIAnimationType.Animator:
                                GUILayout.BeginHorizontal();
                                GUILayout.Label("State Name", GUILayout.Width(100f));
                                EGLTextField(ref viewAnimation.stateName);
                                GUILayout.EndHorizontal();
                                break;
                            default:
                                break;
                        }
                        break;
                    case Menu.Event:
                        EditorGUILayout.PropertyField(onVisibleBegan);
                        EditorGUILayout.PropertyField(onVisibleEnd);
                        break;
                    case Menu.Sound:
                        EditorGUILayout.PropertyField(onVisibleBeginSound);
                        EditorGUILayout.PropertyField(onVisibleEndSound);
                        break;
                }
            }
        }
        private void AnimationDrawer(string actor, UIAnimation animation, bool main)
        {
            if (!foldoutDic.ContainsKey(animation))
            {
                foldoutDic.Add(animation, false);
            }
            Color cacheColor = GUI.color;
            //Move、Rotate、Scale、Fade
            GUILayout.BeginHorizontal(GUILayout.Height(25f));
            {
                GUILayout.Space(10f);
                foldoutDic[animation] = EditorGUILayout.Foldout(foldoutDic[animation], actor, true);
                GUILayout.FlexibleSpace();
                GUI.color = animation.moveToggle ? cacheColor : Color.gray;
                if (GUILayout.Button(GUICONTENTS.MoveTool, "ButtonLeft", GUILayout.Width(25f)))
                {
                    Undo.RecordObject(target, "Move Toggle");
                    animation.moveToggle = !animation.moveToggle;
                    EditorUtility.SetDirty(target);
                }
                GUI.color = animation.rotateToggle ? cacheColor : Color.gray;
                if (GUILayout.Button(GUICONTENTS.RotateTool, "ButtonMid", GUILayout.Width(25f)))
                {
                    Undo.RecordObject(target, "Rotate Toggle");
                    animation.rotateToggle = !animation.rotateToggle;
                    EditorUtility.SetDirty(target);
                }
                GUI.color = animation.scaleToggle ? cacheColor : Color.gray;
                if (GUILayout.Button(GUICONTENTS.ScaleTool, "ButtonMid", GUILayout.Width(25f)))
                {
                    Undo.RecordObject(target, "Scale Toggle");
                    animation.scaleToggle = !animation.scaleToggle;
                    EditorUtility.SetDirty(target);
                }
                GUI.color = animation.fadeToggle ? cacheColor : Color.gray;
                if (GUILayout.Button(GUICONTENTS.ViewTool, "ButtonRight", GUILayout.Width(25f)))
                {
                    Undo.RecordObject(target, "Fade Toggle");
                    animation.fadeToggle = !animation.fadeToggle;
                    EditorUtility.SetDirty(target);
                }
                GUI.color = cacheColor;
                GUI.enabled = !main;
                if (GUILayout.Button(GUICONTENTS.Trash, "ButtonRight", GUILayout.Width(25f)))
                {
                    Undo.RecordObject(target, "Delete");
                    var temp = onVisible.animation.actors.Find(m => m.animation == animation);
                    onVisible.animation.actors.Remove(temp);
                    foldoutDic.Remove(animation);
                    EditorUtility.SetDirty(target);
                    return;
                }
                GUI.enabled = true;
            }
            GUILayout.EndHorizontal();

            if (foldoutDic[animation])
            {
                //MoveAnimation
                if (animation.moveToggle)
                {
                    var moveAnimation = animation.moveAnimation;
                    GUILayout.BeginHorizontal("Badge");
                    {
                        GUILayout.BeginVertical();
                        {
                            GUILayout.Space(40f);
                            GUILayout.Label(GUICONTENTS.MoveTool);
                        }
                        GUILayout.EndVertical();

                        GUILayout.BeginVertical();
                        {
                            //Duration、Delay
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(GUICONTENTS.Duration, GUILayout.Width(labelWidth));
                                var newDuration = EditorGUILayout.FloatField(moveAnimation.duration);
                                if (newDuration != moveAnimation.duration)
                                {
                                    Undo.RecordObject(target, "Move Duration");
                                    moveAnimation.duration = newDuration;
                                    EditorUtility.SetDirty(target);
                                }

                                GUILayout.Label(GUICONTENTS.Delay, GUILayout.Width(labelWidth - 20f));
                                var newDelay = EditorGUILayout.FloatField(moveAnimation.delay);
                                if (newDelay != moveAnimation.delay)
                                {
                                    Undo.RecordObject(target, "Move Delay");
                                    moveAnimation.delay = newDelay;
                                    EditorUtility.SetDirty(target);
                                }
                            }
                            GUILayout.EndHorizontal();

                            //Is Custom
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(GUICONTENTS.From, GUILayout.Width(labelWidth - 2f));
                                if (GUILayout.Button(moveAnimation.isCustom ? GUICONTENTS.CustomPosition : GUICONTENTS.Direction, "DropDownButton"))
                                {
                                    GenericMenu gm = new GenericMenu();
                                    gm.AddItem(GUICONTENTS.Direction, !moveAnimation.isCustom, () => { moveAnimation.isCustom = false; EditorUtility.SetDirty(target); });
                                    gm.AddItem(GUICONTENTS.CustomPosition, moveAnimation.isCustom, () => { moveAnimation.isCustom = true; EditorUtility.SetDirty(target); });
                                    gm.ShowAsContext();
                                }
                            }
                            GUILayout.EndHorizontal();

                            //From
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(GUIContent.none, GUILayout.Width(labelWidth));
                                if (moveAnimation.isCustom)
                                {
                                    Vector3 newStartValue = EditorGUILayout.Vector3Field(GUIContent.none, moveAnimation.startValue);
                                    if (newStartValue != moveAnimation.startValue)
                                    {
                                        Undo.RecordObject(target, "Move From");
                                        moveAnimation.startValue = newStartValue;
                                        EditorUtility.SetDirty(target);
                                    }
                                }
                                else
                                {
                                    var newMoveDirection = (UIMoveAnimationDirection)EditorGUILayout.EnumPopup(moveAnimation.direction);
                                    if (newMoveDirection != moveAnimation.direction)
                                    {
                                        Undo.RecordObject(target, "Move Direction");
                                        moveAnimation.direction = newMoveDirection;
                                        EditorUtility.SetDirty(target);
                                    }
                                }
                            }
                            GUILayout.EndHorizontal();

                            //To
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(GUICONTENTS.To, GUILayout.Width(labelWidth));
                                Vector3 newEndValue = EditorGUILayout.Vector3Field(GUIContent.none, moveAnimation.endValue);
                                if (newEndValue != moveAnimation.endValue)
                                {
                                    Undo.RecordObject(target, "Move To");
                                    moveAnimation.endValue = newEndValue;
                                    EditorUtility.SetDirty(target);
                                }
                            }
                            GUILayout.EndHorizontal();

                            //Ease
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(GUICONTENTS.Ease, GUILayout.Width(labelWidth));
                                var newEase = (Ease)EditorGUILayout.EnumPopup(moveAnimation.ease);
                                if (newEase != moveAnimation.ease)
                                {
                                    Undo.RecordObject(target, "Move Ease");
                                    moveAnimation.ease = newEase;
                                    EditorUtility.SetDirty(target);
                                }
                            }
                            GUILayout.EndHorizontal();
                        }
                        GUILayout.EndVertical();
                    }
                    GUILayout.EndHorizontal();
                }

                //RotateAnimation
                if (animation.rotateToggle)
                {
                    var rotateAnimation = animation.rotateAnimation;
                    GUILayout.BeginHorizontal("Badge");
                    {
                        GUILayout.BeginVertical();
                        {
                            GUILayout.Space(rotateAnimation.isCustom ? 50f : 40f);
                            GUILayout.Label(GUICONTENTS.RotateTool);
                        }
                        GUILayout.EndVertical();

                        GUILayout.BeginVertical();
                        {
                            //Duration、Delay
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(GUICONTENTS.Duration, GUILayout.Width(labelWidth));
                                var newDuration = EditorGUILayout.FloatField(rotateAnimation.duration);
                                if (newDuration != rotateAnimation.duration)
                                {
                                    Undo.RecordObject(target, "Rotate Duration");
                                    rotateAnimation.duration = newDuration;
                                    EditorUtility.SetDirty(target);
                                }

                                GUILayout.Label(GUICONTENTS.Delay, GUILayout.Width(labelWidth - 20f));
                                var newDelay = EditorGUILayout.FloatField(rotateAnimation.delay);
                                if (newDelay != rotateAnimation.delay)
                                {
                                    Undo.RecordObject(target, "Rotate Delay");
                                    rotateAnimation.delay = newDelay;
                                    EditorUtility.SetDirty(target);
                                }
                            }
                            GUILayout.EndHorizontal();

                            //Is Custom
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(GUICONTENTS.From, GUILayout.Width(labelWidth - 2f));
                                if (GUILayout.Button(rotateAnimation.isCustom ? GUICONTENTS.FixedRotation : GUICONTENTS.CurrentRotation, "DropDownButton"))
                                {
                                    GenericMenu gm = new GenericMenu();
                                    gm.AddItem(GUICONTENTS.CurrentRotation, !rotateAnimation.isCustom, () => { rotateAnimation.isCustom = false; EditorUtility.SetDirty(target); });
                                    gm.AddItem(GUICONTENTS.FixedRotation, rotateAnimation.isCustom, () => { rotateAnimation.isCustom = true; EditorUtility.SetDirty(target); });
                                    gm.ShowAsContext();
                                }
                            }
                            GUILayout.EndHorizontal();

                            if (rotateAnimation.isCustom)
                            {
                                //From
                                GUILayout.BeginHorizontal();
                                {
                                    GUILayout.Label(GUIContent.none, GUILayout.Width(labelWidth));
                                    Vector3 newStartValue = EditorGUILayout.Vector3Field(GUIContent.none, rotateAnimation.startValue);
                                    if (newStartValue != rotateAnimation.startValue)
                                    {
                                        Undo.RecordObject(target, "Rotate From");
                                        rotateAnimation.startValue = newStartValue;
                                        EditorUtility.SetDirty(target);
                                    }
                                }
                                GUILayout.EndHorizontal();
                            }

                            //To
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(GUICONTENTS.To, GUILayout.Width(labelWidth));
                                Vector3 newEndValue = EditorGUILayout.Vector3Field(GUIContent.none, rotateAnimation.endValue);
                                if (newEndValue != rotateAnimation.endValue)
                                {
                                    Undo.RecordObject(target, "Rotate To");
                                    rotateAnimation.endValue = newEndValue;
                                    EditorUtility.SetDirty(target);
                                }
                            }
                            GUILayout.EndHorizontal();

                            //Rotate Mode
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(GUICONTENTS.RotateMode, GUILayout.Width(labelWidth));
                                var newRotateMode = (RotateMode)EditorGUILayout.EnumPopup(rotateAnimation.rotateMode);
                                if (newRotateMode != rotateAnimation.rotateMode)
                                {
                                    Undo.RecordObject(target, "Rotate Mode");
                                    rotateAnimation.rotateMode = newRotateMode;
                                    EditorUtility.SetDirty(target);
                                }
                            }
                            GUILayout.EndHorizontal();

                            //Ease
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(GUICONTENTS.Ease, GUILayout.Width(labelWidth));
                                var newEase = (Ease)EditorGUILayout.EnumPopup(rotateAnimation.ease);
                                if (newEase != rotateAnimation.ease)
                                {
                                    Undo.RecordObject(target, "Rotate Ease");
                                    rotateAnimation.ease = newEase;
                                    EditorUtility.SetDirty(target);
                                }
                            }
                            GUILayout.EndHorizontal();
                        }
                        GUILayout.EndVertical();
                    }
                    GUILayout.EndHorizontal();
                }

                //ScaleAnimation
                if (animation.scaleToggle)
                {
                    var scaleAnimation = animation.scaleAnimation;
                    GUILayout.BeginHorizontal("Badge");
                    {
                        GUILayout.BeginVertical();
                        {
                            GUILayout.Space(scaleAnimation.isCustom ? 40f : 30f);
                            GUILayout.Label(GUICONTENTS.ScaleTool);
                        }
                        GUILayout.EndVertical();

                        GUILayout.BeginVertical();
                        {
                            //Duration、Delay
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(GUICONTENTS.Duration, GUILayout.Width(labelWidth));
                                var newDuration = EditorGUILayout.FloatField(scaleAnimation.duration);
                                if (newDuration != scaleAnimation.duration)
                                {
                                    Undo.RecordObject(target, "Scale Duration");
                                    scaleAnimation.duration = newDuration;
                                    EditorUtility.SetDirty(target);
                                }

                                GUILayout.Label(GUICONTENTS.Delay, GUILayout.Width(labelWidth - 20f));
                                var newDelay = EditorGUILayout.FloatField(scaleAnimation.delay);
                                if (newDelay != scaleAnimation.delay)
                                {
                                    Undo.RecordObject(target, "Scale Delay");
                                    scaleAnimation.delay = newDelay;
                                    EditorUtility.SetDirty(target);
                                }
                            }
                            GUILayout.EndHorizontal();

                            //Is Custom
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(GUICONTENTS.From, GUILayout.Width(labelWidth - 2f));
                                if (GUILayout.Button(scaleAnimation.isCustom ? GUICONTENTS.FixedScale : GUICONTENTS.CurrentScale, "DropDownButton"))
                                {
                                    GenericMenu gm = new GenericMenu();
                                    gm.AddItem(GUICONTENTS.CurrentScale, !scaleAnimation.isCustom, () => { scaleAnimation.isCustom = false; EditorUtility.SetDirty(target); });
                                    gm.AddItem(GUICONTENTS.FixedScale, scaleAnimation.isCustom, () => { scaleAnimation.isCustom = true; EditorUtility.SetDirty(target); });
                                    gm.ShowAsContext();
                                }
                            }
                            GUILayout.EndHorizontal();

                            if (scaleAnimation.isCustom)
                            {
                                //From
                                GUILayout.BeginHorizontal();
                                {
                                    GUILayout.Label(GUIContent.none, GUILayout.Width(labelWidth));
                                    Vector3 newStartValue = EditorGUILayout.Vector3Field(GUIContent.none, scaleAnimation.startValue);
                                    if (newStartValue != scaleAnimation.startValue)
                                    {
                                        Undo.RecordObject(target, "Scale From");
                                        scaleAnimation.startValue = newStartValue;
                                        EditorUtility.SetDirty(target);
                                    }
                                }
                                GUILayout.EndHorizontal();
                            }

                            //To
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(GUICONTENTS.To, GUILayout.Width(labelWidth));
                                Vector3 newEndValue = EditorGUILayout.Vector3Field(GUIContent.none, scaleAnimation.endValue);
                                if (newEndValue != scaleAnimation.endValue)
                                {
                                    Undo.RecordObject(target, "Scale To");
                                    scaleAnimation.endValue = newEndValue;
                                    EditorUtility.SetDirty(target);
                                }
                            }
                            GUILayout.EndHorizontal();

                            //Ease
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(GUICONTENTS.Ease, GUILayout.Width(labelWidth));
                                var newEase = (Ease)EditorGUILayout.EnumPopup(scaleAnimation.ease);
                                if (newEase != scaleAnimation.ease)
                                {
                                    Undo.RecordObject(target, "Scale Ease");
                                    scaleAnimation.ease = newEase;
                                    EditorUtility.SetDirty(target);
                                }
                            }
                            GUILayout.EndHorizontal();
                        }
                        GUILayout.EndVertical();
                    }
                    GUILayout.EndHorizontal();
                }

                //FadeAnimation
                if (animation.fadeToggle)
                {
                    var fadeAnimation = animation.fadeAnimation;
                    GUILayout.BeginHorizontal("Badge");
                    {
                        GUILayout.BeginVertical();
                        {
                            GUILayout.Space(fadeAnimation.isCustom ? 40f : 30f);
                            GUILayout.Label(GUICONTENTS.ViewTool);
                        }
                        GUILayout.EndVertical();

                        GUILayout.BeginVertical();
                        {
                            //Duration、Delay
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(GUICONTENTS.Duration, GUILayout.Width(labelWidth));
                                var newDuration = EditorGUILayout.FloatField(fadeAnimation.duration);
                                if (newDuration != fadeAnimation.duration)
                                {
                                    Undo.RecordObject(target, "Fade Duration");
                                    fadeAnimation.duration = newDuration;
                                    EditorUtility.SetDirty(target);
                                }

                                GUILayout.Label(GUICONTENTS.Delay, GUILayout.Width(labelWidth - 20f));
                                var newDelay = EditorGUILayout.FloatField(fadeAnimation.delay);
                                if (newDelay != fadeAnimation.delay)
                                {
                                    Undo.RecordObject(target, "Fade Delay");
                                    fadeAnimation.delay = newDelay;
                                    EditorUtility.SetDirty(target);
                                }
                            }
                            GUILayout.EndHorizontal();

                            //Is Custom
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(GUICONTENTS.From, GUILayout.Width(labelWidth - 2f));
                                if (GUILayout.Button(fadeAnimation.isCustom ? GUICONTENTS.FixedAlpha : GUICONTENTS.CurrentAlpha, "DropDownButton"))
                                {
                                    GenericMenu gm = new GenericMenu();
                                    gm.AddItem(GUICONTENTS.CurrentAlpha, !fadeAnimation.isCustom, () => { fadeAnimation.isCustom = false; EditorUtility.SetDirty(target); });
                                    gm.AddItem(GUICONTENTS.FixedAlpha, fadeAnimation.isCustom, () => { fadeAnimation.isCustom = true; EditorUtility.SetDirty(target); });
                                    gm.ShowAsContext();
                                }
                            }
                            GUILayout.EndHorizontal();

                            if (fadeAnimation.isCustom)
                            {
                                //From
                                GUILayout.BeginHorizontal();
                                {
                                    GUILayout.Label(GUIContent.none, GUILayout.Width(labelWidth));
                                    float newStartValue = EditorGUILayout.FloatField(GUIContent.none, fadeAnimation.startValue);
                                    if (newStartValue != fadeAnimation.startValue)
                                    {
                                        Undo.RecordObject(target, "Fade From");
                                        fadeAnimation.startValue = newStartValue;
                                        EditorUtility.SetDirty(target);
                                    }
                                }
                                GUILayout.EndHorizontal();
                            }

                            //To
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(GUICONTENTS.To, GUILayout.Width(labelWidth));
                                float newEndValue = EditorGUILayout.FloatField(GUIContent.none, fadeAnimation.endValue);
                                if (newEndValue != fadeAnimation.endValue)
                                {
                                    Undo.RecordObject(target, "Fade To");
                                    fadeAnimation.endValue = newEndValue;
                                    EditorUtility.SetDirty(target);
                                }
                            }
                            GUILayout.EndHorizontal();

                            //Ease
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(GUICONTENTS.Ease, GUILayout.Width(labelWidth));
                                var newEase = (Ease)EditorGUILayout.EnumPopup(fadeAnimation.ease);
                                if (newEase != fadeAnimation.ease)
                                {
                                    Undo.RecordObject(target, "Fade Ease");
                                    fadeAnimation.ease = newEase;
                                    EditorUtility.SetDirty(target);
                                }
                            }
                            GUILayout.EndHorizontal();
                        }
                        GUILayout.EndVertical();
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.Space(5f);
            }
        }
    }
}