using System;
using UnityEngine;
using UnityEngine.Events;

namespace SK.Framework.Timer
{
    public sealed class Countdown : ITimer
    {
        private float beginTime;

        private readonly float duration;

        private float pausedTime;

        private readonly bool isIgnoreTimeScale;

        private readonly MonoBehaviour executer;

        private UnityAction onLaunch;
        private UnityAction<float> onExecute;
        private UnityAction onPause;
        private UnityAction onResume;
        private UnityAction onStop;
        private Func<bool> stopWhen;

        public float RemainingTime { get; private set; }

        public bool IsCompleted { get; private set; }

        public bool IsPaused { get; private set; }

        public Countdown(float duration, bool isIgnoreTimeScale = false, MonoBehaviour executer = null)
        {
            this.duration = duration;
            this.isIgnoreTimeScale = isIgnoreTimeScale;
            this.executer = executer;
        }

        public Countdown OnLaunch(UnityAction onLaunch)
        {
            this.onLaunch = onLaunch;
            return this;
        }
        public Countdown OnExecute(UnityAction<float> onExecute)
        {
            this.onExecute = onExecute;
            return this;
        }
        public Countdown OnPause(UnityAction onPause)
        {
            this.onPause = onPause;
            return this;
        }
        public Countdown OnResume(UnityAction onResume)
        {
            this.onResume = onResume;
            return this;
        }
        public Countdown OnStop(UnityAction onStop)
        {
            this.onStop = onStop;
            return this;
        }
        public Countdown StopWhen(Func<bool> predicate)
        {
            stopWhen = predicate;
            return this;
        }

        public void Launch()
        {
            beginTime = isIgnoreTimeScale ? Time.realtimeSinceStartup : Time.time;
            onLaunch?.Invoke();
            this.Begin(executer != null ? executer : Main.Timer);
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
                RemainingTime = duration - ((isIgnoreTimeScale ? Time.realtimeSinceStartup : Time.time) - beginTime);
                RemainingTime = Mathf.Clamp(RemainingTime, 0f, duration);
                onExecute?.Invoke(RemainingTime);
            }
            IsCompleted = RemainingTime <= 0;
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