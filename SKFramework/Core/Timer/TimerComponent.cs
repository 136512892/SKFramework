using UnityEngine;
using UnityEngine.Events;

namespace SK.Framework.Timer
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/Timer")]
    public class TimerComponent : MonoBehaviour
    {
        public Clock Clock(bool isIgnoreTimeScale = false)
        {
            return new Clock(isIgnoreTimeScale, this);
        }
        public Clock Clock(MonoBehaviour executer, bool isIgnoreTimeScale = false)
        {
            return new Clock(isIgnoreTimeScale, executer);
        }

        public Countdown Countdown(float duration, bool isIgnoreTimeScale = false)
        {
            return new Countdown(duration, isIgnoreTimeScale, this);
        }
        public Countdown Countdown(MonoBehaviour executer, float duration, bool isIgnoreTimeScale = false)
        {
            return new Countdown(duration, isIgnoreTimeScale, executer);
        }

        public Chronometer Chronometer(bool isIgnoreTimeScale = false)
        {
            return new Chronometer(isIgnoreTimeScale, this);
        }
        public Chronometer Chronometer(MonoBehaviour executer, bool isIgnoreTimeScale = false)
        {
            return new Chronometer(isIgnoreTimeScale, executer);
        }

        public EverySeconds EverySecond(UnityAction everyAction, bool isIgnoreTimeScale = false, int loops = -1)
        {
            return new EverySeconds(everyAction, 1f, isIgnoreTimeScale, this, loops);
        }
        public EverySeconds EverySecond(MonoBehaviour executer, UnityAction everyAction, bool isIgnoreTimeScale = false, int loops = -1)
        {
            return new EverySeconds(everyAction, 1f, isIgnoreTimeScale, executer, loops);
        }

        public EverySeconds EverySeconds(float seconds, UnityAction everyAction, bool isIgnoreTimeScale = false, int loops = -1)
        {
            return new EverySeconds(everyAction, seconds, isIgnoreTimeScale, this, loops);
        }
        public EverySeconds EverySeconds(MonoBehaviour executer, float seconds, UnityAction everyAction, bool isIgnoreTimeScale = false, int loops = -1)
        {
            return new EverySeconds(everyAction, seconds, isIgnoreTimeScale, executer, loops);
        }

        public EveryFrames EveryFrame(UnityAction everyAction, int loops = -1)
        {
            return new EveryFrames(everyAction, 1, this, loops);
        }
        public EveryFrames EveryFrame(MonoBehaviour executer, UnityAction everyAction, int loops = -1)
        {
            return new EveryFrames(everyAction, 1, executer, loops);
        }

        public EveryFrames EveryFrames(int frameCount, UnityAction everyAction, int loops = -1)
        {
            return new EveryFrames(everyAction, frameCount, this, loops);
        }
        public EveryFrames EveryFrames(MonoBehaviour executer, int frameCount, UnityAction everyAction, int loops = -1)
        {
            return new EveryFrames(everyAction, frameCount, executer, loops);
        }

        public EveryFrames NextFrame(UnityAction callback)
        {
            return new EveryFrames(callback, 1, this, 1);
        }
        public EveryFrames NextFrame(MonoBehaviour executer, UnityAction callback)
        {
            return new EveryFrames(callback, 1, executer, 1);
        }

        public Alarm Alarm(int hour, int minute, int second, UnityAction callback)
        {
            return new Alarm(hour, minute, second, callback, this);
        }
        public Alarm Alarm(MonoBehaviour executer, int hour, int minute, int second, UnityAction callback)
        {
            return new Alarm(hour, minute, second, callback, executer);
        }
    }
}