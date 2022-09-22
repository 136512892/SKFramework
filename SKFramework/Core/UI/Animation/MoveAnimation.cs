using System;
using UnityEngine;
using DG.Tweening;

namespace SK.Framework.UI
{
    /// <summary>
    /// 移动动画
    /// </summary>
    [Serializable]
    public class MoveAnimation : TweenAnimation
    {
        public enum MoveMode
        {
            MoveIn,
            MoveOut
        }

        public Direction direction = Direction.Left;

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
                case Direction.Left: pos = new Vector3(-xOffset, 0f, 0f); break;
                case Direction.Right: pos = new Vector3(xOffset, 0f, 0f); break;
                case Direction.Top: pos = new Vector3(0f, yOffset, 0f); break;
                case Direction.Bottom: pos = new Vector3(0f, -yOffset, 0f); break;
                case Direction.TopLeft: pos = new Vector3(-xOffset, yOffset, 0f); break;
                case Direction.TopRight: pos = new Vector3(xOffset, yOffset, 0f); break;
                case Direction.MiddleCenter: pos = Vector3.zero; break;
                case Direction.BottomLeft: pos = new Vector3(-xOffset, -yOffset, 0f); break;
                case Direction.BottomRight: pos = new Vector3(xOffset, -yOffset, 0f); break;
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