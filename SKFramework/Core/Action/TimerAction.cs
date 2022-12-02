using UnityEngine;
using UnityEngine.Events;

namespace SK.Framework.Actions
{
    public class TimerAction : AbstractAction
    {
        private readonly float duration;

        private readonly bool isReverse;

        private float beginTime;

        private bool isBegan;

        private readonly UnityAction<float> action;

        public TimerAction(float duration, bool isReverse, UnityAction<float> action)
        {
            this.duration = duration;
            this.isReverse = isReverse;
            this.action = action;
        }

        protected override void OnInvoke()
        {
            if (!isBegan)
            {
                isBegan = true;
                beginTime = Time.time;
            }
            float elapsedTime = Time.time - beginTime;
            action.Invoke(Mathf.Clamp(isReverse ? duration - elapsedTime : elapsedTime, 0f, duration));
            isCompleted = elapsedTime >= duration;
        }

        protected override void OnReset()
        {
            isBegan = false;
        }
    }
}