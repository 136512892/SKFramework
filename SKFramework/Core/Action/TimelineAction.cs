using UnityEngine;
using UnityEngine.Events;

namespace SK.Framework.Actions
{
    public class TimelineAction : AbstractAction
    {
        protected float beginTime;

        protected float duration;

        protected UnityAction<float> playAction;

        public float currentTime;

        protected override void OnInvoke()
        {
            float t = (currentTime - beginTime) / duration;
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