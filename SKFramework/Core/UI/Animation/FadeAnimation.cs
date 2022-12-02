using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SK.Framework.UI
{
    /// <summary>
    /// 淡入淡出动画
    /// </summary>
    [Serializable]
    public class FadeAnimation : TweenAnimation
    {
        public float startValue;

        public float endValue = 1f;

        public Tween Play(CanvasGroup target, bool instant = false)
        {
            if (isCustom)
            {
                target.alpha = startValue;
            }
            return target.DOFade(endValue, instant ? 0f : duration).SetDelay(instant ? 0f : delay).SetEase(ease);
        }

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