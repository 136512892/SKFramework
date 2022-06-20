using System;

namespace SK.Framework
{
    /// <summary>
    /// 状态接口
    /// </summary>
    public interface IState 
    {
        /// <summary>
        /// 状态名称
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 状态初始化事件
        /// </summary>
        void OnInitialization();
        /// <summary>
        /// 状态进入事件
        /// </summary>
        void OnEnter();
        /// <summary>
        /// 状态停留事件（Update）
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
        /// 状态切换条件
        /// </summary>
        /// <param name="predicate">切换条件</param>
        /// <param name="targetStateName">目标状态名称</param>
        void SwitchWhen(Func<bool> predicate, string targetStateName);
    }
}