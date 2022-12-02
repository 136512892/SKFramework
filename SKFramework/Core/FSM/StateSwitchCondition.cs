using System;

namespace SK.Framework.FSM
{
    public class StateSwitchCondition
    {
        public readonly Func<bool> predicate;

        public readonly string sourceStateName;

        public readonly string targetStateName;

        public StateSwitchCondition(Func<bool> predicate, string sourceStateName, string targetStateName)
        {
            this.predicate = predicate;
            this.sourceStateName = sourceStateName;
            this.targetStateName = targetStateName;
        }
    }
}