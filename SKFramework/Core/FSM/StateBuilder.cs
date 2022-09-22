using System;

namespace SK.Framework.FSM
{
    /// <summary>
    /// 状态构建器
    /// </summary>
    /// <typeparam name="T">状态类型</typeparam>
    public class StateBuilder<T> where T : State, new()
    {
        //构建的状态
        private readonly T state;
        //构建的状态所属的状态机
        private readonly StateMachine stateMachine;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="state"></param>
        /// <param name="stateMachine"></param>
        public StateBuilder(T state, StateMachine stateMachine)
        {
            this.state = state;
            this.stateMachine = stateMachine;
        }

        /// <summary>
        /// 设置状态初始化事件
        /// </summary>
        /// <param name="onInitialization">状态初始化事件</param>
        /// <returns>状态构建器</returns>
        public StateBuilder<T> OnInitialization(Action<T> onInitialization)
        {
            state.onInitialization = () => onInitialization(state);
            return this;
        }
        /// <summary>
        /// 设置状态进入事件
        /// </summary>
        /// <param name="onEnter">状态进入事件</param>
        /// <returns>状态构建器</returns>
        public StateBuilder<T> OnEnter(Action<T> onEnter)
        {
            state.onEnter = () => onEnter(state);
            return this;
        }
        /// <summary>
        /// 设置状态停留事件
        /// </summary>
        /// <param name="onStay">状态停留事件</param>
        /// <returns>状态构建器</returns>
        public StateBuilder<T> OnStay(Action<T> onStay)
        {
            state.onStay = () => onStay(state);
            return this;
        }
        /// <summary>
        /// 设置状态退出事件
        /// </summary>
        /// <param name="onExit">状态退出事件</param>
        /// <returns>状态构建器</returns>
        public StateBuilder<T> OnExit(Action<T> onExit)
        {
            state.onExit = () => onExit(state);
            return this;
        }
        /// <summary>
        /// 设置状态终止事件
        /// </summary>
        /// <param name="onTermination">状态终止事件</param>
        /// <returns>状态构建器</returns>
        public StateBuilder<T> OnTermination(Action<T> onTermination)
        {
            state.onTermination = () => onTermination(state);
            return this;
        }
        /// <summary>
        /// 设置状态切换条件
        /// </summary>
        /// <param name="predicate">切换条件</param>
        /// <param name="targetStateName">目标状态名称</param>
        /// <returns>状态构建器</returns>
        public StateBuilder<T> SwitchWhen(Func<bool> predicate, string targetStateName)
        {
            state.SwitchWhen(predicate, targetStateName);
            return this;
        }
        /// <summary>
        /// 构建完成
        /// </summary>
        /// <returns>状态机</returns>
        public StateMachine Complete()
        {
            state.OnInitialization();
            return stateMachine;
        }
    }
}