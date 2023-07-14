using System;
using UnityEngine;
using SK.Framework.Actions;

namespace SK.Framework.UI
{
    [Serializable]
    public class UIViewAnimation
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

        public IActionChain Play(UIView view, bool instant = false, Action callback = null)
        {
            var concurrent = new ConcurrentActionChain();
            if (move.toggle) concurrent.Tween(() => move.Play(view.transform as RectTransform, instant));
            if (rotate.toggle) concurrent.Tween(() => rotate.Play(view.transform as RectTransform, instant));
            if (scale.toggle) concurrent.Tween(() => scale.Play(view.transform as RectTransform, instant));
            if (fade.toggle) concurrent.Tween(() => fade.Play(view.GetComponent<CanvasGroup>(), instant));
            return Main.Actions.Sequence(view)
                .Append(concurrent)
                .Event(() => callback?.Invoke())
                .Begin();
        }
    }
}