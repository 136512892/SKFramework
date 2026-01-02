/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Collections.Generic;

using UnityEngine;
using SK.Framework.Logger;
using ILogger = SK.Framework.Logger.ILogger;

namespace SK.Framework.FSM
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/SKFramework.FSM")]
    public class FSM : ModuleBase
    {
        private readonly List<IStateMachine> m_Machines = new List<IStateMachine>(4);
        private ILogger m_Logger;

        protected internal override void OnInitialization()
        {
            base.OnInitialization();
            m_Logger = SKFramework.Module<Log>().GetLogger<ModuleLogger>();
        }

        protected internal override void OnUpdate()
        {
            base.OnUpdate();
            for (int i = 0; i < m_Machines.Count; i++)
                m_Machines[i].OnUpdate();
        }

        protected internal override void OnTermination()
        {
            base.OnTermination();
            for (int i = 0; i < m_Machines.Count; i++)
            {
                var machine = m_Machines[i];
                machine?.OnTermination();
            }
            m_Machines.Clear();
            m_Logger = null;
        }

        public T Create<T>(string stateMachineName) where T : IStateMachine, new()
        {
            Type type = typeof(T);
            stateMachineName = string.IsNullOrEmpty(stateMachineName) ? type.Name : stateMachineName;
            if (m_Machines.Find(m => m.name == stateMachineName) == null)
            {
                T machine = (T)Activator.CreateInstance(type);
                machine.name = stateMachineName;
                machine.OnInitialization();
                m_Machines.Add(machine);
                m_Logger.Info("[FSM] Create state machine, type:{0} name:{1}", type.FullName, stateMachineName);
                return machine;
            }
            m_Logger.Warning("[FSM] A state machine with type {0} already exists.", type.FullName);
            return default;
        }

        public T Create<T>() where T : IStateMachine, new()
        {
            return Create<T>(typeof(T).Name);
        }

        public bool Destroy(string stateMachineName)
        {
            var targetMachine = m_Machines.Find(m => m.name == stateMachineName);
            if (targetMachine != null)
            {
                targetMachine.OnTermination();
                m_Machines.Remove(targetMachine);
                m_Logger.Info("[FSM] Destroy state machine:{0}", stateMachineName);
                return true;
            }
            m_Logger.Warning("[FSM] A state machine with name {0} does not exists.", stateMachineName);
            return false;
        }

        public bool Destroy(IStateMachine stateMachine)
        {
            if (m_Machines.Contains(stateMachine))
            {
                stateMachine.OnTermination();
                m_Machines.Remove(stateMachine);
                m_Logger.Info("[FSM] Destroy state machine:{0}", stateMachine.name);
                return true;
            }
            m_Logger.Warning("[FSM] A state machine with name {0} does not exists.", stateMachine.name);
            return false;
        }

        public bool IsExists(string stateMachineName)
        {
            return m_Machines.FindIndex(m => m.name == stateMachineName) != -1;
        }

        public bool IsExists<T>() where T : IStateMachine, new()
        {
            return IsExists(typeof(T).Name);
        }

        public T Get<T>(string stateMachineName) where T : IStateMachine, new()
        {
            var target = m_Machines.Find(m => m.name == stateMachineName);
            return target != null ? (T)target : default;
        }

        public IStateMachine Get(string stateMachineName)
        {
            return m_Machines.Find(m => m.name == stateMachineName);
        }

        public T Get<T>() where T : IStateMachine, new()
        {
            return Get<T>(typeof(T).Name);
        }

        public bool TryGet<T>(string stateMachineName, out T stateMachine) where T : IStateMachine, new()
        {
            int index = m_Machines.FindIndex(m => m.name == stateMachineName);
            stateMachine = index != -1 ? (T)m_Machines[index] : default;
            return index != -1;
        }

        public bool TryGet<T>(out T stateMachine) where T : IStateMachine, new()
        {
            return TryGet(typeof(T).Name, out stateMachine);
        }
    }
}