using System;
using UnityEngine.Events;

namespace SK.Framework
{
    /// <summary>
    /// 视图动画事件
    /// </summary>
    [Serializable]
    public class ViewAnimationEvent
    {
        public ViewAnimation animation;

        public UnityEvent onBegan;

        public UnityEvent onEnd;

        public Sound beginSound;

        public Sound endSound;
    }
}