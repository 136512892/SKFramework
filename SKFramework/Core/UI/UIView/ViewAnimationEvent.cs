using System;
using UnityEngine.Events;
using SK.Framework.Audios;

namespace SK.Framework.UI
{
    /// <summary>
    /// 视图动画事件
    /// </summary>
    [Serializable]
    public class ViewAnimationEvent
    {
        public ViewAnimation animation;

        public UnityEvent onBeganEvent;

        public UnityEvent onEndEvent;

        public Sound onBeganSound;

        public Sound onEndSound;
    }
}