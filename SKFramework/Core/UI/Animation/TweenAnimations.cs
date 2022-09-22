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

        public IActionChain Play(MonoBehaviour behaviour, RectTransform rectTransform, bool instant = false, UnityAction callback = null)
        {
            var concurrent = new ConcurrentActionChain();
            if (move.toggle) concurrent.Tween(() => move.Play(rectTransform, instant));
            if (rotate.toggle) concurrent.Tween(() => rotate.Play(rectTransform, instant));
            if (scale.toggle) concurrent.Tween(() => scale.Play(rectTransform, instant));
            if (fade.toggle) concurrent.Tween(() => fade.Play(rectTransform.GetComponent<Graphic>(), instant));
            return behaviour.Sequence()
                .Append(concurrent)
                .Event(() => callback?.Invoke())
                .Begin();
        }
    }
}