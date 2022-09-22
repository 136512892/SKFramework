using System;
using UnityEngine;
using UnityEngine.Events;

namespace SK.Framework.Timer
{
    /// <summary>
    /// 闹钟
    /// </summary>
    public sealed class Alarm : ITimer
    {
        private readonly int hour = -1;
        private readonly int minute = -1;
        private readonly int second = -1;

        private readonly MonoBehaviour executer;

        private readonly UnityAction callback;
        private UnityAction onStop;

        public bool IsCompleted { get; private set; }

        public bool IsPaused { get; private set; }

        public Alarm(int hour, int minute, int second, UnityAction callback, MonoBehaviour executer = null)
        {
            this.hour = hour;
            this.minute = minute;
            this.second = second;
            this.callback = callback;
            this.executer = executer;
        }

        public Alarm OnStop(UnityAction onStop)
        {
            this.onStop = onStop;
            return this;
        }

        public void Launch()
        {
            this.Begin(executer != null ? executer : Timer.Instance);
        }

        public void Pause()
        {
            IsPaused = true;
        }

        public void Resume()
        {
            IsPaused = false;
        }

        public void Stop()
        {
            IsCompleted = true;
            onStop?.Invoke();
        }

        public bool Execute()
        {
            if (!IsCompleted && !IsPaused)
            {
                DateTime now = DateTime.Now;
                IsCompleted = now.Hour == hour && now.Minute == minute && now.Second == second;
            }
            if (IsCompleted)
            {
                callback?.Invoke();
            }
            return IsCompleted;
        }
    }
}