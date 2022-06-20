using System;
using UnityEngine;
using DG.Tweening;

namespace SK.Framework
{
    /// <summary>
    /// UI旋转动画
    /// </summary>
    [Serializable]
    public class UIRotateAnimation
    {
        public float duration = 1f;

        public float delay;

        public Ease ease = Ease.Linear;

        public Vector3 startValue;

        public Vector3 endValue;

        public RotateMode rotateMode = RotateMode.Fast;

        public bool isCustom;

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