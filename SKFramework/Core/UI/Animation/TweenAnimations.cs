using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using SK.Framework.Actions;

namespace SK.Framework.UI
{
    [Serializable]
    public class TweenAnimations
    {
        public MoveAnimation move;

        public RotateAnimation rotate;

        public ScaleAnimation scale;

        public FadeAnimation fade;

        public bool IsAny
        {
            get
            {
                return move.toggle || rotate.toggle || scale.toggle || fade.toggle;
            }
        }

        public float Duration
        {
            get
            {
                return Mathf.Max(move.Length, rotate.Length, scale.Length, fade.Length);
            }
        }

        public IActionChain Play(MonoBehaviour behaviour, RectTransform rectTransform, CanvasGroup canvasGroup, bool instant = false, UnityAction callback = null)
        {
            var concurrent = new ConcurrentActionChain();
            if (move.toggle) concurrent.Tween(() => move.Play(rectTransform, instant));
            if (rotate.toggle) concurrent.Tween(() => rotate.Play(rectTransform, instant));
            if (scale.toggle) concurrent.Tween(() => scale.Play(rectTransform, instant));
            if (fade.toggle) concurrent.Tween(() => fade.Play(canvasGroup, instant));
            return Main.Actions.Sequence(behaviour)
                .Append(concurrent)
                .Event(() => callback?.Invoke())
                .Begin();
        }

        public IActionChain Play(MonoBehaviour behaviour, RectTransform rectTransform, Graphic graphic, bool instant = false, UnityAction callback = null)
        {
            var concurrent = new ConcurrentActionChain();
            if (move.toggle) concurrent.Tween(() => move.Play(rectTransform, instant));
            if (rotate.toggle) concurrent.Tween(() => rotate.Play(rectTransform, instant));
            if (scale.toggle) concurrent.Tween(() => scale.Play(rectTransform, instant));
            if (fade.toggle) concurrent.Tween(() => fade.Play(graphic, instant));
            return Main.Actions.Sequence(behaviour)
                .Append(concurrent)
                .Event(() => callback?.Invoke())
                .Begin();
        }
    }
}