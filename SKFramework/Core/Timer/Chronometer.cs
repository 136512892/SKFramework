using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SK.Framework.Timer
{
    /// <summary>
    /// 秒表
    /// </summary>
    public sealed class Chronometer : ITimer
    {
        public sealed class Record
        {
            public object context;

            public float time;

            public Record(object context, float time)
            {
                this.context = context;
                this.time = time;
            }
        }

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
        private Func<bool> shotWhen;

        private readonly List<Record> records;

        public float ElapsedTime { get; private set; }

        public bool IsCompleted { get; private set; }

        public bool IsPaused { get; private set; }

        public ReadOnlyCollection<Record> Records
        {
            get
            {
                return new ReadOnlyCollection<Record>(records);
            }
        }

        public Chronometer(bool isIgnoreTimeScale = false, MonoBehaviour executer = null)
        {
            this.isIgnoreTimeScale = isIgnoreTimeScale;
            this.executer = executer;
            records = new List<Record>();
        }

        public Chronometer OnLaunch(UnityAction onLaunch)
        {
            this.onLaunch = onLaunch;
            return this;
        }
        public Chronometer OnExecute(UnityAction<float> onExecute)
        {
            this.onExecute = onExecute;
            return this;
        }
        public Chronometer OnPause(UnityAction onPause)
        {
            this.onPause = onPause;
            return this;
        }
        public Chronometer OnResume(UnityAction onResume)
        {
            this.onResume = onResume;
            return this;
        }
        public Chronometer OnStop(UnityAction onStop)
        {
            this.onStop = onStop;
            return this;
        }
        public Chronometer StopWhen(Func<bool> predicate)
        {
            stopWhen = predicate;
            return this;
        }
        public Chronometer ShotWhen(Func<bool> predicate)
        {
            shotWhen = predicate;
            return this;
        }
        public void Shot(object context = null)
        {
            records.Add(new Record(context, ElapsedTime));
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
                if (shotWhen != null && shotWhen.Invoke())
                {
                    Shot();
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