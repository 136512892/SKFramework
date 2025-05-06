/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
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

        IStateMachine SwitchWhen(Func<bool> predicate, string targetStateName);

        IStateMachine SwitchWhen(Func<bool> predicate, string sourceStateName, string targetStateName);
    }
}