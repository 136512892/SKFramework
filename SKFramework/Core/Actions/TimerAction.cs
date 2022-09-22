using UnityEngine;
using UnityEngine.Events;

namespace SK.Framework.Actions
{
    /// <summary>
    /// 定时事件
    /// </summary>
    public class TimerAction : AbstractAction
    {
        //定时时长
        private readonly float duration;
        //是否倒计时
        private readonly bool isReverse;
        //开始时间
        private float beginTime;
        //是否已经开始
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