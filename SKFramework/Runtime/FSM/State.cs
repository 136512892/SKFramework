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

        public virtual void OnInitialization()
        {
            onInitialization?.Invoke();
        }

        public virtual void OnEnter(object data = null)
        {
            onEnter?.Invoke();
        }

        public virtual void OnStay()
        {
            onStay?.Invoke();
        }

        public virtual void OnExit()
        {
            onExit?.Invoke();
        }

        public virtual void OnTermination()
        {
            onTermination?.Invoke();
        }

        public void SwitchWhen(Func<bool> predicate, string targetStateName)
        {
            machine.SwitchWhen(predicate, name, targetStateName);
        }
    }
}