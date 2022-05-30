using System;
using UnityEngine;
using DG.Tweening;

namespace SK.Framework
{
    /// <summary>
    /// UI移动动画
    /// </summary>
    [Serializable]
    public class UIMoveAnimation
    {
        public enum MoveMode
        {
            MoveIn,
            MoveOut
        }
        public float duration = 1f;

        public float delay;

        public Ease ease = Ease.Linear;

        public UIMoveAnimationDirection direction = UIMoveAnimationDirection.Left;

        public bool isCustom;

        public Vector3 startValue;

        public Vector3 endValue;

        public MoveMode moveMode = MoveMode.MoveIn;

        public Tween Play(RectTransform target, bool instant = false)
        {
            Vector3 pos = Vector3.zero;
            float xOffset = target.rect.width / 2 + target.rect.width * target.pivot.x;
            float yOffset = target.rect.height / 2 + target.rect.height * target.pivot.y;
            switch (direction)
            {
                case UIMoveAnimationDirection.Left: pos = new Vector3(-xOffset, 0f, 0f); break;
                case UIMoveAnimationDirection.Right: pos = new Vector3(xOffset, 0f, 0f); break;
                case UIMoveAnimationDirection.Top: pos = new Vector3(0f, yOffset, 0f); break;
                case UIMoveAnimationDirection.Bottom: pos = new Vector3(0f, -yOffset, 0f); break;
                case UIMoveAnimationDirection.TopLeft: pos = new Vector3(-xOffset, yOffset, 0f); break;
                case UIMoveAnimationDirection.TopRight: pos = new Vector3(xOffset, yOffset, 0f); break;
                case UIMoveAnimationDirection.MiddleCenter: pos = Vector3.zero; break;
                case UIMoveAnimationDirection.BottomLeft: pos = new Vector3(-xOffset, -yOffset, 0f); break;
                case UIMoveAnimationDirection.BottomRight: pos = new Vector3(xOffset, -yOffset, 0f); break;
            }
            switch (moveMode)
            {
                case MoveMode.MoveIn:
                    target.anchoredPosition3D = isCustom ? startValue : pos;
                    return target.DOAnchorPos3D(endValue, instant ? 0f : duration).SetDelay(instant ? 0f : delay).SetEase(ease);
                case MoveMode.MoveOut:
                    target.anchoredPosition3D = startValue;
                    return target.DOAnchorPos3D(isCustom ? endValue : pos, instant ? 0f : duration).SetDelay(instant ? 0f : delay).SetEase(ease);
                default: return null;
            }
        }
    }
}