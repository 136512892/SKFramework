/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;

namespace SK.Framework.FSM
{
    public interface IState
    {
        string name { get; set; }

        IStateMachine machine { get; set; }

        bool canSwitch2Self { get; }

        void OnInitialization();

        void OnEnter(object data = null);

        void OnStay();

        void OnExit();

        void OnTermination();

        /// <summary>
        /// 设置切换条件
        /// </summary>
        /// <param name="predicate">切换条件</param>
        /// <param name="targetStateName">目标状态名称</param>
        void SwitchWhen(Func<bool> predicate, string targetStateName);
    }
}