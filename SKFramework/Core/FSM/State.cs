using System;

namespace SK.Framework.FSM
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
        /// 是否可切换至自身
        /// </summary>
        public virtual bool CanSwitch2Self { get; set; }
        /// <summary>
        /// 所属状态机
        /// </summary>
        public IStateMachine Machine { get; set; }
        /// <summary>
        /// 状态初始化事件
        /// </summary>
        internal Action onInitialization;
        /// <summary>
        /// 状态进入事件
        /// </summary>
        internal Action onEnter;
        /// <summary>
        /// 状态停留事件
        /// </summary>
        internal Action onStay;
        /// <summary>
        /// 状态退出事件
        /// </summary>
        internal Action onExit;
        /// <summary>
        /// 状态终止事件
        /// </summary>
        internal Action onTermination;

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
        public virtual void OnEnter(object data = null)
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
            Machine.SwitchWhen(predicate, Name, targetStateName);
        }
    }
}