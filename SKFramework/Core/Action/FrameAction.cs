using UnityEngine;
using UnityEngine.Events;

namespace SK.Framework.Actions
{
    public class FrameAction : AbstractAction
    {
        private readonly int duration;

        private int beginFrame;

        private bool isBegan;

        public FrameAction(int duration, UnityAction action)
        {
            this.duration = duration;
            onCompleted = action;
        }

        protected override void OnInvoke()
        {
            if (!isBegan)
            {
                isBegan = true;
                beginFrame = Time.frameCount;
            }
            isCompleted = Time.frameCount - beginFrame >= duration;
        }

        protected override void OnReset()
        {
            isBegan = false;
        }
    }
}