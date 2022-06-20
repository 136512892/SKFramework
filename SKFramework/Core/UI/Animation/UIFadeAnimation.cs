using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SK.Framework
{
    /// <summary>
    /// UI淡入淡出动画
    /// </summary>
    [Serializable]
    public class UIFadeAnimation
    {
        public float duration = 1f;

        public float delay;

        public Ease ease = Ease.Linear;

        public float startValue;

        public float endValue = 1f;

        public bool isCustom;

        public Tween Play(Graphic target, bool instant = false)
        {
            if (isCustom)
            {
                Color color = target.color;
                color.a = startValue;
                target.color = color;
            }
            return target.DOFade(endValue, instant ? 0f : duration).SetDelay(instant ? 0f : delay).SetEase(ease);
        }
    }
}