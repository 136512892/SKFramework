using System;
using UnityEngine;
using DG.Tweening;

namespace SK.Framework.UI
{
    /// <summary>
    /// 缩放动画
    /// </summary>
    [Serializable]
    public class ScaleAnimation : TweenAnimation
    {
        public Vector3 startValue = Vector3.zero;

        public Vector3 endValue = Vector3.one;

        public Tween Play(RectTransform target, bool instant = false)
        {
            if (isCustom)
            {
                target.localScale = startValue;
            }
            return target.DOScale(endValue, instant ? 0f : duration).SetDelay(instant ? 0f : delay).SetEase(ease);
        }
    }
}