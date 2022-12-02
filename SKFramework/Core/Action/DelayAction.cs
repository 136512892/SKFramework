using UnityEngine;
using UnityEngine.Events;

namespace SK.Framework.Actions
{
    public class DelayAction : AbstractAction
    {
        private readonly float duration;

        private float beginTime;
        
        private bool isBegan;

        public DelayAction(float duration, UnityAction action)
        {
            this.duration = duration;
            onCompleted = action;
        }

        protected override void OnInvoke()
        {
            if (!isBegan)
            {
                isBegan = true;
                beginTime = Time.time;
            }
            isCompleted = Time.time - beginTime >= duration;
        }

        protected override void OnReset()
        {
            isBegan = false;
        }
    }
}