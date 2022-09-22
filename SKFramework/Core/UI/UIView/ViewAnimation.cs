using System;
using UnityEngine;
using UnityEngine.Events;
using SK.Framework.Actions;

namespace SK.Framework.UI
{
    [Serializable]
    public class ViewAnimation
    {
        public AnimationType type = AnimationType.Tween;

        public TweenAnimations animations;

        public string stateName;

        public IActionChain Play(UIView view, bool instant = false, UnityAction callback = null)
        {
            switch (type)
            {
                case AnimationType.Tween:
                    return animations.Play(view, view.transform as RectTransform, instant, callback);
                case AnimationType.Animator:
                    return view.Sequence()
                        .Animate(view.GetComponent<Animator>(), stateName)
                        .Event(callback)
                        .Begin();
                default: return null;
            }
        }
    }
}