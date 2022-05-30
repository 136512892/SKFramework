using System;

namespace SK.Framework
{
    /// <summary>
    /// 条件事件（直到...）
    /// </summary>
    public class UntilAction : AbstractAction
    {
        private readonly Func<bool> predicate;

        public UntilAction(Func<bool> predicate, Action action)
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