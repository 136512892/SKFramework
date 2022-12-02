using System;
using UnityEngine.Events;

namespace SK.Framework.Actions
{
    public class UntilAction : AbstractAction
    {
        private readonly Func<bool> predicate;

        public UntilAction(Func<bool> predicate, UnityAction action)
        {
            this.predicate = predicate;
            onCompleted = action;
        }

        protected override void OnInvoke()
        {
            isCompleted = predicate.Invoke();
        }
    }
}