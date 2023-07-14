using System;

namespace SK.Framework.FSM
{
    public interface IState
    {
        /// <summary>
        /// 状态名称
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// 所属状态机
        /// </summary>
        IStateMachine Machine { get; set; }

        /// <summary>
        /// 是否可切换至自身
        /// </summary>
        bool CanSwitch2Self { get; }

        /// <summary>
        /// 状态初始化事件
        /// </summary>
        void OnInitialization();

        /// <summary>
        /// 状态进入事件
        /// </summary>
        /// <param name="data"></param>
        void OnEnter(object data = null);

        /// <summary>
        /// 状态停留事件
        /// </summary>
        void OnStay();

        /// <summary>
        /// 状态退出事件
        /// </summary>
        void OnExit();

        /// <summary>
        /// 状态终止事件
        /// </summary>
        void OnTermination();

        /// <summary>
        /// 设置切换条件
        /// </summary>
        /// <param name="predicate">切换条件</param>
        /// <param name="targetStateName">目标状态名称</param>
        void SwitchWhen(Func<bool> predicate, string targetStateName);
    }
}