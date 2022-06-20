using System;

namespace SK.Framework
{
    /// <summary>
    /// 抽象状态类
    /// </summary>
    public class State : IState
    {
        /// <summary>
        /// 状态名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 所属状态机
        /// </summary>
        public StateMachine machine;
        /// <summary>
        /// 状态初始化事件
        /// </summary>
        public Action onInitialization;
        /// <summary>
        /// 状态进入事件
        /// </summary>
        public Action onEnter;
        /// <summary>
        /// 状态停留事件
        /// </summary>
        public Action onStay;
        /// <summary>
        /// 状态退出事件
        /// </summary>
        public Action onExit;
        /// <summary>
        /// 状态终止事件
        /// </summary>
        public Action onTermination;

        /// <summary>
        /// 状态初始化事件
        /// </summary>
        public virtual void OnInitialization()
        {
            onInitialization?.Invoke();
        }
        /// <summary>
        /// 状态进入事件
        /// </summary>
        public virtual void OnEnter()
        {
            onEnter?.Invoke();
        }
        /// <summary>
        /// 状态停留事件
        /// </summary>
        public virtual void OnStay()
        {
            onStay?.Invoke();
        }
        /// <summary>
        /// 状态退出事件
        /// </summary>
        public virtual void OnExit()
        {
            onExit?.Invoke();
        }
        /// <summary>
        /// 状态终止事件
        /// </summary>
        public virtual void OnTermination()
        {
            onTermination?.Invoke();
        }
        /// <summary>
        /// 设置状态切换条件
        /// </summary>
        /// <param name="predicate">切换条件</param>
        /// <param name="targetStateName">目标状态名称</param>
        public void SwitchWhen(Func<bool> predicate, string targetStateName)
        {
            machine.SwitchWhen(predicate, Name, targetStateName);
        }
    }
}