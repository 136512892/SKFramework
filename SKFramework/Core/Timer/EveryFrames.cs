using System;
using UnityEngine;
using UnityEngine.Events;

namespace SK.Framework.Timer
{
    public sealed class EveryFrames : ITimer
    {
        private int beginFrame;

        private readonly int duration;

        private int pausedFrame;

        private int remainingFrame;

        private readonly MonoBehaviour executer;

        private UnityAction onLaunch;
        private UnityAction onExecute;
        private UnityAction onPause;
        private UnityAction onResume;
        private UnityAction onStop;
        private readonly UnityAction everyAction;
        private Func<bool> stopWhen;

        private int loops;

        public bool IsCompleted { get; private set; }

        public bool IsPaused { get; private set; }

        public EveryFrames(UnityAction everyAction, int duration = 1, MonoBehaviour executer = null, int loops = -1)
        {
            this.duration = duration;
            this.everyAction = everyAction;
            this.executer = executer;
            this.loops = loops;
        }

        public EveryFrames OnLaunch(UnityAction onLaunch)
        {
            this.onLaunch = onLaunch;
            return this;
        }
        public EveryFrames OnExecute(UnityAction onExecute)
        {
            this.onExecute = onExecute;
            return this;
        }
        public EveryFrames OnPause(UnityAction onPause)
        {
            this.onPause = onPause;
            return this;
        }
        public EveryFrames OnResume(UnityAction onResume)
        {
            this.onResume = onResume;
            return this;
        }
        public EveryFrames OnStop(UnityAction onStop)
        {
            this.onStop = onStop;
            return this;
        }
        public EveryFrames StopWhen(Func<bool> predicate)
        {
            stopWhen = predicate;
            return this;
        }

        public void Launch()
        {
            beginFrame = Time.frameCount;
            onLaunch?.Invoke();
            this.Begin(executer != null ? executer : Timer.Instance);
        }

        public void Pause()
        {
            IsPaused = true;
            pausedFrame = Time.frameCount;
            onPause?.Invoke();
        }

        public void Resume()
        {
            IsPaused = false;
            beginFrame += Time.frameCount - pausedFrame;
            onResume?.Invoke();
        }

        public void Stop()
        {
            IsCompleted = true;
        }

        public bool Execute()
        {
            if (!IsCompleted && !IsPaused)
            {
                remainingFrame = duration - (Time.frameCount - beginFrame);
                onExecute?.Invoke();
            }
            if (remainingFrame <= 0)
            {
                everyAction?.Invoke();
                if (--loops == 0)
                {
                    IsCompleted = true;
                }
                else
                {
                    beginFrame = Time.frameCount;
                }
            }
            if (!IsCompleted && stopWhen != null && stopWhen.Invoke())
            {
                IsCompleted = true;
            }
            if (IsCompleted)
            {
                onStop?.Invoke();
            }
            return IsCompleted;
        }
    }
}