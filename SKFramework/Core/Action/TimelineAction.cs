using System;
using UnityEngine;

namespace SK.Framework
{
    /// <summary>
    /// 时间轴事件
    /// </summary>
    public class TimelineAction : AbstractAction
    {
        protected float beginTime;

        protected float duration;

        protected Action<float> playAction;

        public float currentTime;

        protected override void OnInvoke()
        {
            //求得插值
            float t = (currentTime - beginTime) / duration;
            //钳制 0-1
            t = Mathf.Clamp01(t);
            playAction.Invoke(t);
        }

        public TimelineAction(float beginTime, float duration, Action<float> playAction)
        {
            this.beginTime = beginTime;
            this.duration = duration;
            this.playAction = playAction;
        }
    }
}