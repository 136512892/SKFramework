using System;
using UnityEngine;
using UnityEngine.UI;

namespace SK.Framework
{
    [Serializable]
    public class UIAnimation
    {
        public bool moveToggle;

        public UIMoveAnimation moveAnimation;

        public bool rotateToggle;

        public UIRotateAnimation rotateAnimation;

        public bool scaleToggle;

        public UIScaleAnimation scaleAnimation;

        public bool fadeToggle;

        public UIFadeAnimation fadeAnimation;

        public bool IsAnyAnimation
        {
            get
            {
                return moveToggle || rotateToggle || scaleToggle || fadeToggle;
            }
        }

        public float Duration
        {
            get
            {
                return MathUtility.Max(moveAnimation.duration + moveAnimation.delay,
                    rotateAnimation.duration + rotateAnimation.delay,
                    scaleAnimation.duration + scaleAnimation.delay,
                    fadeAnimation.duration + fadeAnimation.delay);
            }
        }

        public IActionChain Play(MonoBehaviour behaviour, RectTransform rectTransform, bool instant = false, Action callback = null)
        {
            var concurrent = new ConcurrentActionChain();
            if (moveToggle) concurrent.Tween(() => moveAnimation.Play(rectTransform, instant));
            if (rotateToggle) concurrent.Tween(() => rotateAnimation.Play(rectTransform, instant));
            if (scaleToggle) concurrent.Tween(() => scaleAnimation.Play(rectTransform, instant));
            if (fadeToggle) concurrent.Tween(() => fadeAnimation.Play(rectTransform.GetComponent<Graphic>(), instant));
            return behaviour.Sequence()
                .Append(concurrent)
                .Event(() => callback?.Invoke())
                .Begin();
        }
    }
}