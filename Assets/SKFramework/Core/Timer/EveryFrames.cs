using System;
using UnityEngine;

namespace SK.Framework
{
    public sealed class EveryFrames : ITimer
    {
        private int beginFrame;

        private readonly int duration;

        private int pausedFrame;

        private int remainingFrame;

        private readonly MonoBehaviour executer;

        private Action onLaunch;
        private Action onExecute;
        private Action onPause;
        private Action onResume;
        private Action onStop;
        private readonly Action everyAction;
        private Func<bool> stopWhen;

        private int loops;

        public bool IsCompleted { get; private set; }

        public bool IsPaused { get; private set; }

        public EveryFrames(Action everyAction, int duration = 1, MonoBehaviour executer = null, int loops = -1)
        {
            this.duration = duration;
            this.everyAction = everyAction;
            this.executer = executer;
            this.loops = loops;
        }

        public EveryFrames OnLaunch(Action onLaunch)
        {
            this.onLaunch = onLaunch;
            return this;
        }
        public EveryFrames OnExecute(Action onExecute)
        {
            this.onExecute = onExecute;
            return this;
        }
        public EveryFrames OnPause(Action onPause)
        {
            this.onPause = onPause;
            return this;
        }
        public EveryFrames OnResume(Action onResume)
        {
            this.onResume = onResume;
            return this;
        }
        public EveryFrames OnStop(Action onStop)
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