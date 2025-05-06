/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;

namespace SK.Framework.FSM
{
    public class StateBuilder<T> where T : State, new()
    {
        private readonly T m_State;

        public StateBuilder(T state)
        {
            m_State = state;
        }

        public StateBuilder<T> OnInitialization(Action<T> onInitialization)
        {
            m_State.onInitialization = () => onInitialization(m_State);
            return this;
        }

        public StateBuilder<T> OnEnter(Action<T> onEnter)
        {
            m_State.onEnter = () => onEnter(m_State);
            return this;
        }

        public StateBuilder<T> OnStay(Action<T> onStay)
        {
            m_State.onStay = () => onStay(m_State);
            return this;
        }

        public StateBuilder<T> OnExit(Action<T> onExit)
        {
            m_State.onExit = () => onExit(m_State);
            return this;
        }

        public StateBuilder<T> OnTermination(Action<T> onTermination)
        {
            m_State.onTermination = () => onTermination(m_State);
            return this;
        }

        public StateBuilder<T> SwitchWhen(Func<bool> predicate, string targetStateName)
        {
            m_State.SwitchWhen(predicate, targetStateName);
            return this;
        }

        public StateMachine Complete()
        {
            m_State.OnInitialization();
            return m_State.machine as StateMachine;
        }
    }
}