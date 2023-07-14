using System;
using UnityEngine;
using SK.Framework.Actions;

namespace SK.Framework.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class RectTransformTweenAnimation : MonoBehaviour
    {
        private IActionChain actionChain;

        [SerializeField] private bool autoPlay = true;

        [SerializeField] private MoveAnimation move;

        [SerializeField] private RotateAnimation rotate;

        [SerializeField] private ScaleAnimation scale;

        private float beginTime;

        public bool IsAny
        {
            get
            {
                return move.toggle || rotate.toggle || scale.toggle;
            }
        }

        public float Duration
        {
            get
            {
                return Mathf.Max(move.Length, rotate.Length, scale.Length);
            }
        }

        public bool IsPlaying
        {
            get
            {
                return actionChain != null;
            }
        }

        public float Progress
        {
            get
            {
                return actionChain != null ? Mathf.Clamp01((Time.time - beginTime) / Duration) : 0f;
            }
        }

        private void Start()
        {
            if (autoPlay)
            {
                Play();
            }
        }

        public void Play(bool instant = false, Action callback = null)
        {
            actionChain?.Stop();
            beginTime = Time.time;
            RectTransform rectTransform = transform as RectTransform;
            var concurrent = new ConcurrentActionChain();
            if (move.toggle) concurrent.Tween(() => move.Play(rectTransform, instant));
            if (rotate.toggle) concurrent.Tween(() => rotate.Play(rectTransform, instant));
            if (scale.toggle) concurrent.Tween(() => scale.Play(rectTransform, instant));
            actionChain = Main.Actions.Sequence(this)
                .Append(concurrent)
                .Event(() =>
                {
                    callback?.Invoke();
                    actionChain = null;
                })
                .Begin();
        }

        public void Stop()
        {
            actionChain?.Stop();
            actionChain = null;
        }
    }
}