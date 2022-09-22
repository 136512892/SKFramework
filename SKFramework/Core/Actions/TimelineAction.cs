using UnityEngine;
using UnityEngine.Events;

namespace SK.Framework.Actions
{
    /// <summary>
    /// 时间轴事件
    /// </summary>
    public class TimelineAction : AbstractAction
    {
        protected float beginTime;

        protected float duration;

        protected UnityAction<float> playAction;

        public float currentTime;

        protected override void OnInvoke()
        {
            //求得插值
            float t = (currentTime - beginTime) / duration;
            //钳制 0-1
            t = Mathf.Clamp01(t);
            playAction.Invoke(t);
        }

        public TimelineAction(float beginTime, float duration, UnityAction<float> playAction)
        {
            this.beginTime = beginTime;
            this.duration = duration;
            this.playAction = playAction;
        }
    }
}