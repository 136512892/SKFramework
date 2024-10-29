/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using UnityEngine;

namespace SK.Framework.Actions
{
    public static class ActionChainExtension
    {
        public static IActionChain Event(this IActionChain chain, System.Action action)
        {
            return chain.Append(new SimpleAction(action));
        }

        public static IActionChain Events(this IActionChain chain, params System.Action[] actions)
        {
            for (int i = 0; i < actions.Length; i++)
                chain.Append(new SimpleAction(actions[i]));
            return chain;
        }

        public static IActionChain Delay(this IActionChain chain, float duration, System.Action action = null)
        {
            return chain.Append(new DelayAction(duration, action));
        }

        public static IActionChain Frame(this IActionChain chain, int duration, System.Action action = null)
        {
            return chain.Append(new FrameAction(duration, action));
        }

        public static IActionChain Timer(this IActionChain chain, float duration, bool isReverse, Action<float> action)
        {
            return chain.Append(new TimerAction(duration, isReverse, action));
        }

        public static IActionChain Until(this IActionChain chain, Func<bool> predicate, System.Action action = null)
        {
            return chain.Append(new UntilAction(predicate, action));
        }

        public static IActionChain While(this IActionChain chain, Func<bool> predicate, System.Action action = null)
        {
            return chain.Append(new WhileAction(predicate, action));
        }

        public static IActionChain Animation(this IActionChain chain, Animator animator, string stateName, int layerIndex = 0)
        {
            return chain.Append(new AnimatorAction(animator, stateName, layerIndex));
        }

        public static TimelineActionChain Append(this TimelineActionChain chain, float beginTime, float duration, Action<float> action)
        {
            return chain.Append(new TimelineAction(beginTime, duration, action)) as TimelineActionChain;
        }
    }
}