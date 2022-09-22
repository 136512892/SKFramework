using System;
using UnityEngine;
using UnityEngine.Events;

namespace SK.Framework.Timer
{
    /// <summary>
    /// 时钟/计时器
    /// </summary>
    public sealed class Clock : ITimer
    {
        private float beginTime;

        private float pausedTime;

        private readonly bool isIgnoreTimeScale;

        private readonly MonoBehaviour executer;

        private UnityAction onLaunch;
        private UnityAction<float> onExecute;
        private UnityAction onPause;
        private UnityAction onResume;
        private UnityAction onStop;
        private Func<bool> stopWhen;

        /// <summary>
        /// 已经计时
        /// </summary>
        public float ElapsedTime { get; private set; }
       
        public bool IsCompleted { get; private set; }

        public bool IsPaused { get; private set; }

        public Clock(bool isIgnoreTimeScale, MonoBehaviour executer)
        {
            this.isIgnoreTimeScale = isIgnoreTimeScale;
            this.executer = executer;
        }

        public Clock OnLaunch(UnityAction onLaunch)
        {
            this.onLaunch = onLaunch;
            return this;
        }
        public Clock OnExecute(UnityAction<float> onExecute)
        {
            this.onExecute = onExecute;
            return this;
        }
        public Clock OnPause(UnityAction onPause)
        {
            this.onPause = onPause;
            return this;
        }
        public Clock OnResume(UnityAction onResume)
        {
            this.onResume = onResume;
            return this;
        }
        public Clock OnStop(UnityAction onStop)
        {
            this.onStop = onStop;
            return this;
        }
        public Clock StopWhen(Func<bool> predicate)
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
                ElapsedTime = (isIgnoreTimeScale ? Time.realtimeSinceStartup : Time.time) - beginTime;
                onExecute?.Invoke(ElapsedTime);
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