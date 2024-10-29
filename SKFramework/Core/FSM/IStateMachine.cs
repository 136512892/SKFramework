/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;

namespace SK.Framework.FSM
{
    public interface IStateMachine
    {
        string name { get; set; }

        IState currentState { get; }

        int Add(IState state);

        bool Remove(IState state);

        bool Remove(string stateName);

        bool IsExists(string stateName);

        T Get<T>(string stateName) where T : IState, new();

        bool TryGet<T>(string stateName, out T state) where T : IState, new();

        int Switch(string stateName, object data = null);

        bool Switch2Next();

        bool Switch2Last();

        void Exit();

        void OnInitialization();

        void OnUpdate();

        void OnTermination();

        /// <summary>
        /// 设置状态切换条件
        /// </summary>
        /// <param name="predicate">切换条件</param>
        /// <param name="targetStateName">目标状态名称</param>
        /// <returns>状态机</returns>
        IStateMachine SwitchWhen(Func<bool> predicate, string targetStateName);

        /// <summary>
        /// 设置状态切换条件
        /// </summary>
        /// <param name="predicate">切换条件</param>
        /// <param name="sourceStateName">源状态名称</param>
        /// <param name="tarxgetStateName">目标状态名称</param>
        /// <returns></returns>
        IStateMachine SwitchWhen(Func<bool> predicate, string sourceStateName, string targetStateName);
    }
}