using UnityEngine;
using UnityEditor;
using System.Reflection;
using UnityEditor.AnimatedValues;

namespace SK.Framework.UI
{
    [CustomEditor(typeof(GraphicTweenAnimation))]
    public class GraphicTweemAnimationEditor : Editor
    {
        private MoveAnimation move;
        private RotateAnimation rotate;
        private ScaleAnimation scale;
        private FadeAnimation fade;

        private AnimBool moveAnimBool;
        private AnimBool rotateAnimBool;
        private AnimBool scaleAnimBool;
        private AnimBool fadeAnimBool;

        private SerializedProperty autoPlay;

        private void OnEnable()
        {
            autoPlay = serializedObject.FindProperty("autoPlay");
        }

        public override void OnInspectorGUI()
        {
            if (move == null || rotate == null || scale == null)
            {
                move = typeof(GraphicTweenAnimation).GetField("move",
                    BindingFlags.Instance | BindingFlags.NonPublic).GetValue(target) as MoveAnimation;
                rotate = typeof(GraphicTweenAnimation).GetField("rotate",
                    BindingFlags.Instance | BindingFlags.NonPublic).GetValue(target) as RotateAnimation;
                scale = typeof(GraphicTweenAnimation).GetField("scale",
                    BindingFlags.Instance | BindingFlags.NonPublic).GetValue(target) as ScaleAnimation;
                fade = typeof(GraphicTweenAnimation).GetField("fade",
                    BindingFlags.Instance | BindingFlags.NonPublic).GetValue(target) as FadeAnimation;

                moveAnimBool = new AnimBool(move.toggle, Repaint);
                rotateAnimBool = new AnimBool(rotate.toggle, Repaint);
                scaleAnimBool = new AnimBool(scale.toggle, Repaint);
                fadeAnimBool = new AnimBool(fade.toggle, Repaint);
            }

            GUILayout.Space(5f);
            //Move、Rotate、Scale、Fade
            GUILayout.BeginHorizontal(GUILayout.Height(25f));
            {
                AnimationsDrawer.DrawMoveToggle(move, moveAnimBool, target, "ButtonLeft");
                AnimationsDrawer.DrawRotateToggle(rotate, rotateAnimBool, target, "ButtonMid");
                AnimationsDrawer.DrawScaleToggle(scale, scaleAnimBool, target, "ButtonMid");
                AnimationsDrawer.DrawFadeToggle(fade, fadeAnimBool, target, "ButtonRight");

                GUILayout.FlexibleSpace();

                bool newAutoPlay = GUILayout.Toggle(autoPlay.boolValue, "Auto Play");
                if (newAutoPlay != autoPlay.boolValue)
                {
                    Undo.RecordObject(target, "Auto Play");
                    autoPlay.boolValue = newAutoPlay;
                    EditorUtility.SetDirty(target);
                    serializedObject.ApplyModifiedProperties();
                }
            }
            GUILayout.EndHorizontal();
            //MoveAnimation
            AnimationsDrawer.DrawMove(move, moveAnimBool, target, true);
            GUILayout.Space(3f);
            //RotateAnimation
            AnimationsDrawer.DrawRotate(rotate, rotateAnimBool, target);
            GUILayout.Space(3f);
            //ScaleAnimation
            AnimationsDrawer.DrawScale(scale, scaleAnimBool, target);
            GUILayout.Space(3f);
            //FadeAnimation
            AnimationsDrawer.DrawFade(fade, fadeAnimBool, target);
            GUILayout.Space(5f);
        }
    }
}