using System;
using DG.Tweening;

namespace SK.Framework.Actions
{
    public class TweenAction : AbstractAction
    {
        private Tween tween;
        private readonly Func<Tween> action;
        private bool isBegan;

        public TweenAction(Func<Tween> action)
        {
            this.action = action;
        }

        protected override void OnInvoke()
        {
            if (!isBegan)
            {
                isBegan = true;
                tween = action.Invoke();
            }
            isCompleted = !tween.IsPlaying();
        }

        protected override void OnReset()
        {
            isBegan = false;
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