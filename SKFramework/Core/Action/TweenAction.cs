/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

/*
using System;
using DG.Tweening;

namespace SK.Framework.Actions
{
    public class TweenAction : AbstactAction
    {
        private Tween m_Tween;
        private readonly Func<Tween> m_Action;
        private bool m_IsBegan;

        public TweenAction(Func<Tween> action)
        {
            m_Action = action;
        }

        protected override void OnInvoke()
        {
            if (!m_IsBegan)
            {
                m_IsBegan = true;
                m_Tween = m_Action.Invoke();
            }
            m_IsCompleted = !m_Tween.IsPlaying();
        }

        protected override void OnReset()
        {
            m_IsBegan = false;
        }
    }

    public static class TweenActionExtension
    {
        public static IActionChain Tween(this IActionChain chain, Func<Tween> tweenAction)
        {
            return chain.Append(new TweenAction(tweenAction));
        }
    }
}
*/