using System;
using UnityEngine;
using DG.Tweening;

namespace SK.Framework.UI
{
    /// <summary>
    /// 旋转动画
    /// </summary>
    [Serializable]
    public class RotateAnimation : TweenAnimation
    {
        public Vector3 startValue;

        public Vector3 endValue;

        public RotateMode rotateMode = RotateMode.Fast;

        public Tween Play(RectTransform target, bool instant = false)
        {
            if (isCustom)
            {
                target.localRotation = Quaternion.Euler(startValue);
            }
            return target.DORotate(endValue, instant ? 0f : duration, rotateMode).SetDelay(instant ? 0f : delay).SetEase(ease);
        }
    }
}