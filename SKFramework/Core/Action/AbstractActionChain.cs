using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace SK.Framework.Actions
{
    public abstract class AbstractActionChain : AbstractAction, IActionChain
    {
        protected MonoBehaviour executer;
        protected List<IAction> cacheList;
        protected List<IAction> invokeList;
        protected Func<bool> stopWhen;
        public bool IsPaused { get; protected set; }
        protected int loops = 1;

        public AbstractActionChain()
        {
            executer = Main.Actions;
            cacheList = new List<IAction>();
            invokeList = new List<IAction>();
        }
        public AbstractActionChain(MonoBehaviour executer)
        {
            this.executer = executer;
            cacheList = new List<IAction>();
            invokeList = new List<IAction>();
        }
        public IActionChain Append(IAction action)
        {
            cacheList.Add(action);
            invokeList.Add(action);
            return this;
        }
        public IActionChain StopWhen(Func<bool> predicate)
        {
            stopWhen = predicate;
            return this;
        }
        public IActionChain OnStop(UnityAction action)
        {
            onCompleted = action;
            return this;
        }
        public IActionChain Begin()
        {
            Main.Actions.Execute(this, executer);
            return this;
        }
        public void Pause()
        {
            IsPaused = true;
        }
        public void Resume()
        {
            IsPaused = false;
        }
        public void Stop()
        {
            isCompleted = true;
        }
        public IActionChain SetLoops(int loops)
        {
            this.loops = loops;
            return this;
        }
    }
}