/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Collections.Generic;

using SK.Framework.Logger;
using ILogger = SK.Framework.Logger.ILogger;

namespace SK.Framework.FSM
{
    public class StateMachine : IStateMachine
    {
        protected readonly List<IState> m_States = new List<IState>(8);
        private readonly List<StateSwitchCondition> m_Conditions = new List<StateSwitchCondition>(0);
        private ILogger m_Logger;

        public string name { get; set; }

        public IState currentState { get; protected set; }

        public int Add(IState state)
        {
            if (!m_States.Contains(state))
            {
                if (m_States.Find(m => m.name == state.name) == null)
                {
                    m_States.Add(state);
                    state.OnInitialization();
                    state.machine = this;
                    m_Logger.Info("[FSM] State machine {0} add state {1}", name, state.name);
                    return 0;
                }
                m_Logger.Warning("[FSM] A state with name {0} already exists in the state machine {1}", state.name, name);
                return -2;
            }
            m_Logger.Warning("[FSM] The state {0} already exists in the state machine {1}", state.name, name);
            return -1;
        }

        public int Add<T>(string stateName = null) where T : IState, new()
        {
            Type type = typeof(T);
            T t = (T)Activator.CreateInstance(type);
            t.name = string.IsNullOrEmpty(stateName) ? type.Name : stateName;
            return Add(t);
        }

        public bool Remove(string stateName)
        {
            var target = m_States.Find(m => m.name == stateName);
            if (target != null)
            {
                if (currentState == target)
                {
                    currentState.OnExit();
                    currentState = null;
                }
                target.OnTermination();
                m_States.Remove(target);
                m_Logger.Info("[FSM] State machine {0} remove state {1}", name, stateName);
                return true;
            }
            m_Logger.Warning("[FSM] A state with name {0} does not exists in the state machine {1}", stateName, name);
            return false;
        }

        public bool Remove(IState state)
        {
            return Remove(state.name);
        }

        public bool Remove<T>() where T : IState
        {
            return Remove(typeof(T).Name);
        }

        public bool IsExists(string stateName)
        {
            return m_States.FindIndex(m => m.name == stateName) != -1;
        }

        public bool IsExists<T>()
        {
            return IsExists(typeof(T).Name);
        }

        public T Get<T>(string stateName) where T : IState, new()
        {
            var target = m_States.Find(m => m.name == stateName);
            return target != null ? (T)target : default;
        }

        public T Get<T>() where T : IState, new()
        {
            return Get<T>(typeof(T).Name);
        }

        public bool TryGet<T>(string stateName, out T state) where T : IState, new()
        {
            int index = m_States.FindIndex(m => m.name == stateName);
            state = index != -1 ? (T)m_States[index] : default;
            return index != -1;
        }

        public bool TryGet<T>(out T state) where T : IState, new()
        {
            return TryGet(typeof(T).Name, out state);
        }

        public int Switch(string stateName, object data = null)
        {
            var target = m_States.Find(m => m.name == stateName);
            if (target == null)
            {
                m_Logger.Debug("[FSM] The target state({0}) does not exist in the state machine {1}.", stateName, name);
                return -1;
            }
            if (currentState == target && !target.canSwitch2Self)
            {
                m_Logger.Debug("[FSM] The current state of the state machine {0} is already the target state({1}) and cannot be switched to itself.", stateName);
                return -2;
            }
            currentState?.OnExit();
            currentState = target;
            currentState.OnEnter(data);
            return 0;
        }

        public int Switch<T>(object data = null) where T : IState
        {
            return Switch(typeof(T).Name, data);
        }

        public bool Switch2Next()
        {
            if (m_States.Count != 0)
            {
                if (currentState != null)
                {
                    int index = m_States.IndexOf(currentState);
                    index = index + 1 < m_States.Count ? index + 1 : 0;
                    IState targetState = m_States[index];
                    currentState.OnExit();
                    currentState = targetState;
                }
                else
                {
                    currentState = m_States[0];
                }
                currentState.OnEnter();
                return true;
            }
            m_Logger.Debug("[FSM] The state list of the state machine {0} is empty", name);
            return false;
        }

        public bool Switch2Last()
        {
            if (m_States.Count != 0)
            {
                if (currentState != null)
                {
                    int index = m_States.IndexOf(currentState);
                    index = index - 1 >= 0 ? index - 1 : m_States.Count - 1;
                    IState targetState = m_States[index];
                    currentState.OnExit();
                    currentState = targetState;
                }
                else
                {
                    currentState = m_States[m_States.Count - 1];
                }
                currentState.OnEnter();
                return true;
            }
            m_Logger.Debug("[FSM] The state list of the state machine {0} is empty", name);
            return false;
        }

        public void Exit()
        {
            if (currentState != null)
            {
                currentState.OnExit();
                currentState = null;
            }
        }

        public virtual void OnInitialization() 
        {
            m_Logger = SKFramework.Module<Log>().GetLogger<ModuleLogger>();
        }

        public virtual void OnUpdate()
        {
            currentState?.OnStay();
            for (int i = 0; i < m_Conditions.Count; i++)
            {
                var condition = m_Conditions[i];
                if (condition.predicate.Invoke())
                {
                    if (string.IsNullOrEmpty(condition.sourceStateName))
                        Switch(condition.targetStateName);
                    else
                    {
                        if (currentState.name == condition.sourceStateName)
                            Switch(condition.targetStateName);
                    }
                }
            }
        }

        public virtual void OnTermination()
        {
            for (int i = 0; i < m_States.Count; i++)
                m_States[i].OnTermination();
        }

        public IStateMachine SwitchWhen(Func<bool> predicate, string targetStateName)
        {
            m_Conditions.Add(new StateSwitchCondition(predicate, null, targetStateName));
            return this;
        }

        public IStateMachine SwitchWhen(Func<bool> predicate, string sourceStateName, string targetStateName)
        {
            m_Conditions.Add(new StateSwitchCondition(predicate, sourceStateName, targetStateName));
            return this;
        }

        public StateBuilder<T> Build<T>(string stateName = null) where T : State, new()
        {
            Type type = typeof(T);
            string name = string.IsNullOrEmpty(stateName) ? type.Name : stateName;
            if (m_States.Find(m => m.name == name) == null)
            {
                T state = (T)Activator.CreateInstance(type);
                state.name = name;
                state.machine = this;
                m_States.Add(state);
                m_Logger.Info("[FSM] State machine {0} build state {1}", name, state.name);
                return new StateBuilder<T>(state);
            }
            return null;
        }
    }
}