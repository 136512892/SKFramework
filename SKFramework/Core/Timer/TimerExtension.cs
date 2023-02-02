using UnityEngine;
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
    }
}