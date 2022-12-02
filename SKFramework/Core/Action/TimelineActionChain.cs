using UnityEngine;

namespace SK.Framework.Actions
{
    public class TimelineActionChain : AbstractActionChain
    {
        public TimelineActionChain() : base() { }
        public TimelineActionChain(MonoBehaviour executer) : base(executer) { }

        public float CurrentTime { get; set; }

        public float Speed { get; set; } = 1f;

        protected override void OnInvoke()
        {
            if (stopWhen != null && stopWhen.Invoke())
            {
                isCompleted = true;
            }
            else if (!IsPaused)
            {
                CurrentTime += Time.deltaTime * Speed;
                for (int i = 0; i < invokeList.Count; i++)
                {
                    var action = invokeList[i];
                    if (action is TimelineAction)
                    {
                        TimelineAction ta = action as TimelineAction;
                        ta.currentTime = CurrentTime;
                        ta.Invoke();
                    }
                }
            }
            if (isCompleted)
            {
                loops--;
                if (loops != 0)
                {
                    Reset();
                }
                else
                {
                    isCompleted = true;
                }
            }
        }

        protected override void OnReset()
        {
            IsPaused = false;
            for (int i = 0; i < cacheList.Count; i++)
            {
                cacheList[i].Reset();
                invokeList.Add(cacheList[i]);
            }
            CurrentTime = 0f;
            Speed = 1f;
        }
    }
}