using System;
using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework
{
    [Serializable]
    public class ViewAnimation
    {
        public UIAnimationType type = UIAnimationType.Tween;

        public List<UIAnimationActor> actors = new List<UIAnimationActor>(0);

        public string stateName;

        public IActionChain Play(UIView view, bool instant = false, Action callback = null)
        {
            switch (type)
            {
                case UIAnimationType.Tween:
                    IActionChain concurrent = new ConcurrentActionChain();
                    for (int i = 0; i < actors.Count; i++)
                    {
                        var actor = actors[i];
                        concurrent.Append(actor.animation.Play(view, actor.actor, instant) as IAction);
                    }
                    return view.Sequence()
                        .Append(concurrent as IAction)
                        .Event(callback)
                        .Begin();
                case UIAnimationType.Animator:
                    return view.Sequence()
                        .Animate(view.GetComponent<Animator>(), stateName)
                        .Event(callback)
                        .Begin();
                default: return null;
            }
        }
    }
}