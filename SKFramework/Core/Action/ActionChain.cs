using System;
using UnityEngine;
using UnityEngine.Events;

namespace SK.Framework.Actions
{
    public static class ActionChain
    {
        public static IActionChain Event(this IActionChain chain, UnityAction action)
        {
            return chain.Append(new SimpleAction(action));
        }
        public static IActionChain Events(this IActionChain chain, params UnityAction[] actions)
        {
            for (int i = 0; i < actions.Length; i++)
            {
                chain.Append(new SimpleAction(actions[i]));
            }
            return chain;
        }
        public static IActionChain Delay(this IActionChain chain, float duration, UnityAction action = null)
        {
            return chain.Append(new DelayAction(duration, action));
        }
        public static IActionChain Frame(this IActionChain chain, int duration, UnityAction action = null)
        {
            return chain.Append(new FrameAction(duration, action));
        }
        public static IActionChain Timer(this IActionChain chain, float duration, bool isReverse, UnityAction<float> action)
        {
            return chain.Append(new TimerAction(duration, isReverse, action));
        }
        public static IActionChain Until(this IActionChain chain, Func<bool> predicate, UnityAction action = null)
        {
            return chain.Append(new UntilAction(predicate, action));
        }
        public static IActionChain While(this IActionChain chain, Func<bool> predicate, UnityAction action = null)
        {
            return chain.Append(new WhileAction(predicate, action));
        }
        public static IActionChain Animate(this IActionChain chain, Animator animator, string stateName, int layerIndex = 0)
        {
            return chain.Append(new AnimateAction(animator, stateName, layerIndex));
        }
        public static TimelineActionChain Append(this TimelineActionChain chain, float beginTime, float duration, UnityAction<float> playAction)
        {
            return chain.Append(new TimelineAction(beginTime, duration, playAction)) as TimelineActionChain;
        }
    }
}