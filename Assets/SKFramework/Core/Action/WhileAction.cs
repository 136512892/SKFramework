using System;

namespace SK.Framework
{
    /// <summary>
    /// 条件事件（当...）
    /// </summary>
    public class WhileAction : AbstractAction
    {
        private readonly Action action;
        private readonly Func<bool> predicate;

        public WhileAction(Func<bool> predicate, Action action)
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