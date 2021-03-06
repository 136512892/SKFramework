using System;
using UnityEngine;
using System.Collections;

namespace SK.Framework
{
    public static class ActionChain
    {
        public static void Execute(IAction action, MonoBehaviour executer)
        {
            executer.StartCoroutine(ExecuteCoroutine(action));
        }
        private static IEnumerator ExecuteCoroutine(IAction self)
        {
            while (!self.Invoke())
            {
                yield return null;
            }
        }

        public static IActionChain Sequence()
        {
            return new SequenceActionChain(ActionMaster.Instance);
        }
        public static IActionChain Sequence(this MonoBehaviour executer)
        {
            return new SequenceActionChain(executer);
        }
        public static IActionChain Concurrent()
        {
            return new ConcurrentActionChain(ActionMaster.Instance);
        }
        public static IActionChain Concurrent(this MonoBehaviour executer)
        {
            return new ConcurrentActionChain(executer);
        }
        public static IActionChain Timeline()
        {
            return new TimelineActionChain(ActionMaster.Instance);
        }
        public static IActionChain Timeline(this MonoBehaviour executer)
        {
            return new TimelineActionChain(executer);
        }

        public static IActionChain Event(this IActionChain chain, Action action)
        {
            return chain.Append(new SimpleAction(action));
        }
        public static IActionChain Events(this IActionChain chain, params Action[] actions)
        {
            for (int i = 0; i < actions.Length; i++)
            {
                chain.Append(new SimpleAction(actions[i]));
            }
            return chain;
        }
        public static IActionChain Delay(this IActionChain chain, float duration, Action action = null)
        {
            return chain.Append(new DelayAction(duration, action));
        }
        public static IActionChain Timer(this IActionChain chain, float duration, bool isReverse, Action<float> action)
        {
            return chain.Append(new TimerAction(duration, isReverse, action));
        }
        public static IActionChain Until(this IActionChain chain, Func<bool> predicate, Action action = null)
        {
            return chain.Append(new UntilAction(predicate, action));
        }
        public static IActionChain While(this IActionChain chain, Func<bool> predicate, Action action = null)
        {
            return chain.Append(new WhileAction(predicate, action));
        }
        public static IActionChain Animate(this IActionChain chain, Animator animator, string stateName, int layerIndex = 0)
        {
            return chain.Append(new AnimateAction(animator, stateName, layerIndex));
        }
        public static IActionChain Append(this IActionChain chain, float beginTime, float duration, Action<float> playAction)
        {
            return chain.Append(new TimelineAction(beginTime, duration, playAction));
        }
    }
}