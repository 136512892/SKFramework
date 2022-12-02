#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using DG.Tweening;
using UnityEditor.AnimatedValues;

namespace SK.Framework.UI
{
    public class AnimationsDrawer
    {
        private readonly static float labelWidth = 60f;
        private readonly static float alpha = .4f;
        private readonly static GUIContent moveTool = EditorGUIUtility.IconContent("MoveTool");
        private readonly static GUIContent rotateTool = EditorGUIUtility.IconContent("RotateTool");
        private readonly static GUIContent scaleTool = EditorGUIUtility.IconContent("ScaleTool");
        private readonly static GUIContent viewTool = EditorGUIUtility.IconContent("ViewToolOrbit");
        private readonly static GUIContent duration = new GUIContent("Duration", "动画时长");
        private readonly static GUIContent delay = new GUIContent("Delay", "延时时长");
        private readonly static GUIContent from = new GUIContent("From", "初始值");
        private readonly static GUIContent to = new GUIContent("To", "目标值");
        private readonly static GUIContent ease = new GUIContent("Ease");
        private readonly static GUIContent rotateMode = new GUIContent("Mode", "旋转模式");
        private readonly static GUIContent direction = new GUIContent("Direction", "方向");
        private readonly static GUIContent customPosition = new GUIContent("Custom Position", "自定义坐标");
        private readonly static GUIContent currentRotation = new GUIContent("Current Rotation", "当前旋转值");
        private readonly static GUIContent fixedRotation = new GUIContent("Fixed Rotation", "固定旋转值");
        private readonly static GUIContent currentScale = new GUIContent("Current Scale", "当前缩放值");
        private readonly static GUIContent fixedScale = new GUIContent("Fixed Scale", "固定缩放值");
        private readonly static GUIContent currentAlpha = new GUIContent("Current Alpha", "当前透明值");
        private readonly static GUIContent fixedAlpha = new GUIContent("Fixed Alpha", "固定透明值");

        public static void DrawMoveToggle(MoveAnimation moveAnimation, AnimBool animBool, Object target, GUIStyle style)
        {
            Color cacheColor = GUI.color;
            Color alphaColor = new Color(cacheColor.r, cacheColor.g, cacheColor.b, alpha);
            GUI.color = moveAnimation.toggle ? cacheColor : alphaColor;
            if (GUILayout.Button(moveTool, style, GUILayout.Width(25f)))
            {
                Undo.RecordObject(target, "Move Toggle");
                moveAnimation.toggle = !moveAnimation.toggle;
                animBool.target = moveAnimation.toggle;
                EditorUtility.SetDirty(target);
            }
            GUI.color = cacheColor;
        }
        public static void DrawRotateToggle(RotateAnimation rotateAnimation, AnimBool animBool, Object target, GUIStyle style)
        {
            Color cacheColor = GUI.color;
            Color alphaColor = new Color(cacheColor.r, cacheColor.g, cacheColor.b, alpha);
            GUI.color = rotateAnimation.toggle ? cacheColor : alphaColor;
            if (GUILayout.Button(rotateTool, style, GUILayout.Width(25f)))
            {
                Undo.RecordObject(target, "Rotate Toggle");
                rotateAnimation.toggle = !rotateAnimation.toggle;
                animBool.target = rotateAnimation.toggle;
                EditorUtility.SetDirty(target);
            }
            GUI.color = cacheColor;
        }
        public static void DrawScaleToggle(ScaleAnimation scaleAnimation, AnimBool animBool, Object target, GUIStyle style)
        {
            Color cacheColor = GUI.color;
            Color alphaColor = new Color(cacheColor.r, cacheColor.g, cacheColor.b, alpha);
            GUI.color = scaleAnimation.toggle ? cacheColor : alphaColor;
            if (GUILayout.Button(scaleTool, style, GUILayout.Width(25f)))
            {
                Undo.RecordObject(target, "Scale Toggle");
                scaleAnimation.toggle = !scaleAnimation.toggle;
                animBool.target = scaleAnimation.toggle;
                EditorUtility.SetDirty(target);
            }
            GUI.color = cacheColor;
        }
        public static void DrawFadeToggle(FadeAnimation fadeAnimation, AnimBool animBool, Object target, GUIStyle style)
        {
            Color cacheColor = GUI.color;
            Color alphaColor = new Color(cacheColor.r, cacheColor.g, cacheColor.b, alpha);
            GUI.color = fadeAnimation.toggle ? cacheColor : alphaColor;
            if (GUILayout.Button(viewTool, style, GUILayout.Width(25f)))
            {
                Undo.RecordObject(target, "Fade Toggle");
                fadeAnimation.toggle = !fadeAnimation.toggle;
                animBool.target = fadeAnimation.toggle;
                EditorUtility.SetDirty(target);
            }
            GUI.color = cacheColor;
        }

        public static void DrawMove(MoveAnimation moveAnimation, AnimBool animBool, Object target, bool visiable)
        {
            if (EditorGUILayout.BeginFadeGroup(animBool.faded))
            {
                GUILayout.BeginHorizontal("Badge");
                {
                    GUILayout.BeginVertical();
                    {
                        GUILayout.Space(40f);
                        GUILayout.Label(moveTool);
                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical();
                    {
                        //Duration、Delay
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(duration, GUILayout.Width(labelWidth));
                            var newDuration = EditorGUILayout.FloatField(moveAnimation.duration);
                            if (newDuration != moveAnimation.duration)
                            {
                                Undo.RecordObject(target, "Move Duration");
                                moveAnimation.duration = newDuration;
                                EditorUtility.SetDirty(target);
                            }

                            GUILayout.Label(delay, GUILayout.Width(labelWidth - 20f));
                            var newDelay = EditorGUILayout.FloatField(moveAnimation.delay);
                            if (newDelay != moveAnimation.delay)
                            {
                                Undo.RecordObject(target, "Move Delay");
                                moveAnimation.delay = newDelay;
                                EditorUtility.SetDirty(target);
                            }
                        }
                        GUILayout.EndHorizontal();

                        if (visiable)
                        {
                            //Is Custom
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(from, GUILayout.Width(labelWidth - 2f));
                                if (GUILayout.Button(moveAnimation.isCustom ? customPosition : direction, "DropDownButton"))
                                {
                                    GenericMenu gm = new GenericMenu();
                                    gm.AddItem(direction, !moveAnimation.isCustom, () => { moveAnimation.isCustom = false; EditorUtility.SetDirty(target); });
                                    gm.AddItem(customPosition, moveAnimation.isCustom, () => { moveAnimation.isCustom = true; EditorUtility.SetDirty(target); });
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
                                    var newMoveDirection = (Direction)EditorGUILayout.EnumPopup(moveAnimation.direction);
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
                                GUILayout.Label(to, GUILayout.Width(labelWidth));
                                Vector3 newEndValue = EditorGUILayout.Vector3Field(GUIContent.none, moveAnimation.endValue);
                                if (newEndValue != moveAnimation.endValue)
                                {
                                    Undo.RecordObject(target, "Move To");
                                    moveAnimation.endValue = newEndValue;
                                    EditorUtility.SetDirty(target);
                                }
                            }
                            GUILayout.EndHorizontal();
                        }
                        else
                        {
                            //From
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(from, GUILayout.Width(labelWidth - 2f));
                                Vector3 newStartValue = EditorGUILayout.Vector3Field(GUIContent.none, moveAnimation.startValue);
                                if (newStartValue != moveAnimation.startValue)
                                {
                                    Undo.RecordObject(target, "Move From");
                                    moveAnimation.startValue = newStartValue;
                                    EditorUtility.SetDirty(target);
                                }
                            }
                            GUILayout.EndHorizontal();

                            //Is Custom
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(to, GUILayout.Width(labelWidth - 2f));
                                if (GUILayout.Button(moveAnimation.isCustom ? customPosition : direction, "DropDownButton"))
                                {
                                    GenericMenu gm = new GenericMenu();
                                    gm.AddItem(direction, !moveAnimation.isCustom, () => { moveAnimation.isCustom = false; EditorUtility.SetDirty(target); });
                                    gm.AddItem(customPosition, moveAnimation.isCustom, () => { moveAnimation.isCustom = true; EditorUtility.SetDirty(target); });
                                    gm.ShowAsContext();
                                }
                            }
                            GUILayout.EndHorizontal();

                            //To
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(GUIContent.none, GUILayout.Width(labelWidth));

                                if (moveAnimation.isCustom)
                                {
                                    Vector3 newEndValue = EditorGUILayout.Vector3Field(GUIContent.none, moveAnimation.endValue);
                                    if (newEndValue != moveAnimation.endValue)
                                    {
                                        Undo.RecordObject(target, "Move To");
                                        moveAnimation.endValue = newEndValue;
                                        EditorUtility.SetDirty(target);
                                    }
                                }
                                else
                                {
                                    var newMoveDirection = (Direction)EditorGUILayout.EnumPopup(moveAnimation.direction);
                                    if (newMoveDirection != moveAnimation.direction)
                                    {
                                        Undo.RecordObject(target, "Move Direction");
                                        moveAnimation.direction = newMoveDirection;
                                        EditorUtility.SetDirty(target);
                                    }
                                }
                            }
                            GUILayout.EndHorizontal();
                        }

                        //Ease
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(ease, GUILayout.Width(labelWidth));
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
            EditorGUILayout.EndFadeGroup();
        }
        public static void DrawRotate(RotateAnimation rotateAnimation, AnimBool animBool, Object target)
        {
            if (EditorGUILayout.BeginFadeGroup(animBool.faded))
            {
                GUILayout.BeginHorizontal("Badge");
                {
                    GUILayout.BeginVertical();
                    {
                        GUILayout.Space(rotateAnimation.isCustom ? 50f : 40f);
                        GUILayout.Label(rotateTool);
                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical();
                    {
                        //Duration、Delay
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(duration, GUILayout.Width(labelWidth));
                            var newDuration = EditorGUILayout.FloatField(rotateAnimation.duration);
                            if (newDuration != rotateAnimation.duration)
                            {
                                Undo.RecordObject(target, "Rotate Duration");
                                rotateAnimation.duration = newDuration;
                                EditorUtility.SetDirty(target);
                            }

                            GUILayout.Label(delay, GUILayout.Width(labelWidth - 20f));
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
                            GUILayout.Label(from, GUILayout.Width(labelWidth - 2f));
                            if (GUILayout.Button(rotateAnimation.isCustom ? fixedRotation : currentRotation, "DropDownButton"))
                            {
                                GenericMenu gm = new GenericMenu();
                                gm.AddItem(currentRotation, !rotateAnimation.isCustom, () => { rotateAnimation.isCustom = false; EditorUtility.SetDirty(target); });
                                gm.AddItem(fixedRotation, rotateAnimation.isCustom, () => { rotateAnimation.isCustom = true; EditorUtility.SetDirty(target); });
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
                            GUILayout.Label(to, GUILayout.Width(labelWidth));
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
                            GUILayout.Label(rotateMode, GUILayout.Width(labelWidth));
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
                            GUILayout.Label(ease, GUILayout.Width(labelWidth));
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
            EditorGUILayout.EndFadeGroup();
        }
        public static void DrawScale(ScaleAnimation scaleAnimation, AnimBool animBool, Object target)
        {
            if (EditorGUILayout.BeginFadeGroup(animBool.faded))
            {
                GUILayout.BeginHorizontal("Badge");
                {
                    GUILayout.BeginVertical();
                    {
                        GUILayout.Space(scaleAnimation.isCustom ? 40f : 30f);
                        GUILayout.Label(scaleTool);
                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical();
                    {
                        //Duration、Delay
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(duration, GUILayout.Width(labelWidth));
                            var newDuration = EditorGUILayout.FloatField(scaleAnimation.duration);
                            if (newDuration != scaleAnimation.duration)
                            {
                                Undo.RecordObject(target, "Scale Duration");
                                scaleAnimation.duration = newDuration;
                                EditorUtility.SetDirty(target);
                            }

                            GUILayout.Label(delay, GUILayout.Width(labelWidth - 20f));
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
                            GUILayout.Label(from, GUILayout.Width(labelWidth - 2f));
                            if (GUILayout.Button(scaleAnimation.isCustom ? fixedScale : currentScale, "DropDownButton"))
                            {
                                GenericMenu gm = new GenericMenu();
                                gm.AddItem(currentScale, !scaleAnimation.isCustom, () => { scaleAnimation.isCustom = false; EditorUtility.SetDirty(target); });
                                gm.AddItem(fixedScale, scaleAnimation.isCustom, () => { scaleAnimation.isCustom = true; EditorUtility.SetDirty(target); });
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
                            GUILayout.Label(to, GUILayout.Width(labelWidth));
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
                            GUILayout.Label(ease, GUILayout.Width(labelWidth));
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
            EditorGUILayout.EndFadeGroup();
        }
        public static void DrawFade(FadeAnimation fadeAnimation, AnimBool animBool, Object target)
        {
            if (EditorGUILayout.BeginFadeGroup(animBool.faded))
            {
                GUILayout.BeginHorizontal("Badge");
                {
                    GUILayout.BeginVertical();
                    {
                        GUILayout.Space(fadeAnimation.isCustom ? 40f : 30f);
                        GUILayout.Label(viewTool);
                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical();
                    {
                        //Duration、Delay
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(duration, GUILayout.Width(labelWidth));
                            var newDuration = EditorGUILayout.FloatField(fadeAnimation.duration);
                            if (newDuration != fadeAnimation.duration)
                            {
                                Undo.RecordObject(target, "Fade Duration");
                                fadeAnimation.duration = newDuration;
                                EditorUtility.SetDirty(target);
                            }

                            GUILayout.Label(delay, GUILayout.Width(labelWidth - 20f));
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
                            GUILayout.Label(from, GUILayout.Width(labelWidth - 2f));
                            if (GUILayout.Button(fadeAnimation.isCustom ? fixedAlpha : currentAlpha, "DropDownButton"))
                            {
                                GenericMenu gm = new GenericMenu();
                                gm.AddItem(currentAlpha, !fadeAnimation.isCustom, () => { fadeAnimation.isCustom = false; EditorUtility.SetDirty(target); });
                                gm.AddItem(fixedAlpha, fadeAnimation.isCustom, () => { fadeAnimation.isCustom = true; EditorUtility.SetDirty(target); });
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
                            GUILayout.Label(to, GUILayout.Width(labelWidth));
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
                            GUILayout.Label(ease, GUILayout.Width(labelWidth));
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
            EditorGUILayout.EndFadeGroup();
        }
    }
}
#endif