using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace SK.Framework.Timer
{
    public static class TimerExtension 
    {
        public static void Begin(this ITimer self, MonoBehaviour executer)
        {
            executer.StartCoroutine(ExecuteCoroutine(self));
        }
        private static IEnumerator ExecuteCoroutine(ITimer timer)
        {
            while (!timer.Execute())
            {
                yield return null;
            }
        }

        public static Countdown Countdown(this MonoBehaviour self, float duration, bool isIgnoreTimeScale = false)
        {
            return new Countdown(duration, isIgnoreTimeScale, self);
        }

        public static Clock Clock(this MonoBehaviour self, bool isIgnoreTimeScale = false)
        {
            return new Clock(isIgnoreTimeScale, self);
        }

        public static Chronometer Chronometer(this MonoBehaviour self, bool isIgnoreTimeScale = false)
        {
            return new Chronometer(isIgnoreTimeScale, self);
        }

        public static EverySeconds EverySecond(this MonoBehaviour self, UnityAction everyAction, bool isIgnoreTimeScale = false, int loops = -1)
        {
            return new EverySeconds(everyAction, 1f, isIgnoreTimeScale, self, loops);
        }

        public static EverySeconds EverySeconds(this MonoBehaviour self, float seconds, UnityAction everyAction, bool isIgnoreTimeScale = false, int loops = -1)
        {
            return new EverySeconds(everyAction, seconds, isIgnoreTimeScale, self, loops);
        }

        public static EveryFrames EveryFrame(this MonoBehaviour self, UnityAction everyAction, int loops = -1)
        {
            return new EveryFrames(everyAction, 1, self, loops);
        }

        public static EveryFrames EveryFrames(this MonoBehaviour self, int frameCount, UnityAction everyAction, int loops = -1)
        {
            return new EveryFrames(everyAction, frameCount, self, loops);
        }

        public static EveryFrames NextFrame(this MonoBehaviour self, UnityAction callback)
        {
            return new EveryFrames(callback, 1, self, 1);
        }

        public static Alarm Alarm(this MonoBehaviour self, int hour, int minute, int second, UnityAction callback)
        {
            return new Alarm(hour, minute, second, callback, self);
        }
    }
}