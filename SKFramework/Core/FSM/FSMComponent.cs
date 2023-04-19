using System;
using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework.FSM
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/FSM")]
    public class FSMComponent : MonoBehaviour
    {
        //状态机列表
        private readonly List<StateMachine> machines = new List<StateMachine>();

        private void Update()
        {
            for (int i = 0; i < machines.Count; i++)
            {
                //更新状态机
                machines[i].OnUpdate();
            }
        }

        /// <summary>
        /// 创建状态机
        /// </summary>
        /// <typeparam name="T">状态机类型</typeparam>
        /// <param name="stateMachineName">状态机名称</param>
        /// <returns>状态机</returns>
        public T Create<T>(string stateMachineName = null) where T : StateMachine, new()
        {
            Type type = typeof(T);
            stateMachineName = string.IsNullOrEmpty(stateMachineName) ? type.Name : stateMachineName;
            if (machines.Find(m => m.Name == stateMachineName) == null)
            {
                T machine = (T)Activator.CreateInstance(type);
                machine.Name = stateMachineName;
                machine.OnInitialization();
                machines.Add(machine);
                return machine;
            }
            return null;
        }
        /// <summary>
        /// 销毁状态机
        /// </summary>
        /// <param name="stateMachineName">状态机名称</param>
        /// <returns>true：销毁成功； false：目标状态机不存在，销毁失败</returns>
        public bool Destroy(string stateMachineName)
        {
            var targetMachine = machines.Find(m => m.Name == stateMachineName);
            if (targetMachine != null)
            {
                targetMachine.OnDestroy();
                machines.Remove(targetMachine);
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
            if (machines.Contains(stateMachine))
            {
                stateMachine.OnDestroy();
                machines.Remove(stateMachine);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取状态机
        /// </summary>
        /// <typeparam name="T">状态机类型</typeparam>
        /// <param name="stateMachineName">状态机名称</param>
        /// <returns>状态机</returns>
        public T GetMachine<T>(string stateMachineName = null) where T : StateMachine, new()
        {
            stateMachineName = string.IsNullOrEmpty(stateMachineName) ? typeof(T).Name : stateMachineName;
            var target = machines.Find(m => m.Name == stateMachineName);
            return target != null ? target as T : null;
        }
    }
}