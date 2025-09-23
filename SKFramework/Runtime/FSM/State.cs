/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;

namespace SK.Framework.FSM
{
    public class State : IState
    {
        public string name { get; set; }

        public IStateMachine machine { get; set; }

        public virtual bool canSwitch2Self { get; }

        internal Action onInitialization;
        internal Action onEnter;
        internal Action onStay;
        internal Action onExit;
        internal Action onTermination;

        void IState.OnInitialization() => OnInitialization();
        void IState.OnEnter(object data) => OnEnter(data);
        void IState.OnStay() => OnStay();
        void IState.OnExit() => OnExit();
        void IState.OnTermination() => OnTermination();

        protected internal virtual void OnInitialization()
        {
            onInitialization?.Invoke();
        }

        protected internal virtual void OnEnter(object data = null)
        {
            onEnter?.Invoke();
        }

        protected internal virtual void OnStay()
        {
            onStay?.Invoke();
        }

        protected internal virtual void OnExit()
        {
            onExit?.Invoke();
        }

        protected internal virtual void OnTermination()
        {
            onTermination?.Invoke();
        }

        public void SwitchWhen(Func<bool> predicate, string targetStateName)
        {
            machine.SwitchWhen(predicate, name, targetStateName);
        }
    }
}