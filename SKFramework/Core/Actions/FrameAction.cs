using UnityEngine;
using UnityEngine.Events;

namespace SK.Framework.Actions
{
    /// <summary>
    /// 延时事件的补充 
    /// 不同于DelayAction的是FrameAction以帧为单位
    /// </summary>
    public class FrameAction : AbstractAction
    {
        //延时帧数
        private readonly int duration;
        //开始帧数
        private int beginFrame;
        //是否已经开始
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