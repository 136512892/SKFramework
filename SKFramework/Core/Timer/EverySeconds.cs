using System;
using UnityEngine;

namespace SK.Framework
{
    public sealed class EverySeconds : ITimer
    {
        private float beginTime;

        private readonly float duration;

        private float pausedTime;
 
        private float remainingTime;

        private readonly bool isIgnoreTimeScale;

        private readonly MonoBehaviour executer;

        private Action onLaunch;
        private Action<float> onExecute;
        private Action onPause;
        private Action onResume;
        private Action onStop;
        private readonly Action everyAction;
        private Func<bool> stopWhen;

        private int loops;

        public bool IsCompleted { get; private set; }

        public bool IsPaused { get; private set; }

        public EverySeconds(Action everyAction, float duration = 1f, bool isIgnoreTimeScale = false, MonoBehaviour executer = null, int loops = -1)
        {
            this.duration = duration;
            this.everyAction = everyAction;
            this.isIgnoreTimeScale = isIgnoreTimeScale;
            this.executer = executer;
            this.loops = loops;
        }

        public EverySeconds OnLaunch(Action onLaunch)
        {
            this.onLaunch = onLaunch;
            return this;
        }
        public EverySeconds OnExecute(Action<float> onExecute)
        {
            this.onExecute = onExecute;
            return this;
        }
        public EverySeconds OnPause(Action onPause)
        {
            this.onPause = onPause;
            return this;
        }
        public EverySeconds OnResume(Action onResume)
        {
            this.onResume = onResume;
            return this;
        }
        public EverySeconds OnStop(Action onStop)
        {
            this.onStop = onStop;
            return this;
        }
        public EverySeconds StopWhen(Func<bool> predicate)
        {
            stopWhen = predicate;
            return this;
        }

        public void Launch()
        {
            beginTime = isIgnoreTimeScale ? Time.realtimeSinceStartup : Time.time;
            onLaunch?.Invoke();
            this.Begin(executer != null ? executer : Timer.Instance);
        }

        public void Pause()
        {
            IsPaused = true;
            pausedTime = isIgnoreTimeScale ? Time.realtimeSinceStartup : Time.time;
            onPause?.Invoke();
        }

        public void Resume()
        {
            IsPaused = false;
            beginTime += (isIgnoreTimeScale ? Time.realtimeSinceStartup : Time.time) - pausedTime;
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
                remainingTime = duration - ((isIgnoreTimeScale ? Time.realtimeSinceStartup : Time.time) - beginTime);
                remainingTime = Mathf.Clamp(remainingTime, 0f, duration);
                onExecute?.Invoke(remainingTime);
            }
            if (remainingTime <= 0) 
            {
                everyAction?.Invoke();
                if (--loops == 0)
                {
                    IsCompleted = true;
                }
                else
                {
                    beginTime = isIgnoreTimeScale ? Time.realtimeSinceStartup : Time.time;
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