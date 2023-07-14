using System;

namespace SK.Framework.FSM
{
    public class StateSwitchCondition
    {
        /// <summary>
        /// 切换条件
        /// </summary>
        public readonly Func<bool> predicate;

        /// <summary>
        /// 源状态名称
        /// </summary>
        public readonly string sourceStateName;

        /// <summary>
        /// 目标状态名称
        /// </summary>
        public readonly string targetStateName;

        public StateSwitchCondition(Func<bool> predicate, string sourceStateName, string targetStateName)
        {
            this.predicate = predicate;
            this.sourceStateName = sourceStateName;
            this.targetStateName = targetStateName;
        }
    }
}