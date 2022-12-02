using System;
using UnityEngine.Events;

namespace SK.Framework.Actions
{
    public class WhileAction : AbstractAction
    {
        private readonly UnityAction action;
        private readonly Func<bool> predicate;

        public WhileAction(Func<bool> predicate, UnityAction action)
        {
            this.predicate = predicate;
            this.action = action;
        }

        protected override void OnInvoke()
        {
            action?.Invoke();
            isCompleted = !predicate.Invoke();
        }
    }
}