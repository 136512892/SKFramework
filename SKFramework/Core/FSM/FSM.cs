/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using System.Collections.Generic;

namespace SK.Framework.FSM
{
    public class FSM : ModuleBase
    {
        //状态机列表
        private readonly List<IStateMachine> m_Machines = new List<IStateMachine>(4);

        private void Update()
        {
            for (int i = 0; i < m_Machines.Count; i++)
                m_Machines[i].OnUpdate();
        }

        /// <summary>
        /// 创建状态机
        /// </summary>
        /// <typeparam name="T">状态机类型</typeparam>
        /// <param name="stateMachineName">状态机名称</param>
        /// <returns>状态机</returns>
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
                return machine;
            }
            return default;
        }

        /// <summary>
        /// 创建状态机
        /// </summary>
        /// <typeparam name="T">状态机类型</typeparam>
        /// <returns>状态机</returns>
        public T Create<T>() where T : IStateMachine, new()
        {
            return Create<T>(typeof(T).Name);
        }

        /// <summary>
        /// 销毁状态机
        /// </summary>
        /// <param name="stateMachineName">状态机名称</param>
        /// <returns>true：销毁成功； false：目标状态机不存在，销毁失败</returns>
        public bool Destroy(string stateMachineName)
        {
            var targetMachine = m_Machines.Find(m => m.name == stateMachineName);
            if (targetMachine != null)
            {
                targetMachine.OnTermination();
                m_Machines.Remove(targetMachine);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 销毁状态机
        /// </summary>
        /// <typeparam name="T">状态机类型</typeparam>
        /// <param name="stateMachine">状态机</param>
        /// <returns>true：销毁成功； false：目标状态机不存在，销毁失败</returns>
        public bool Destroy<T>(T stateMachine) where T : StateMachine, new()
        {
            if (m_Machines.Contains(stateMachine))
            {
                stateMachine.OnTermination();
                m_Machines.Remove(stateMachine);
                return true;
            }
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