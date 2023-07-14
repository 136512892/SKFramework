using System;
using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework.FSM
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/FSM")]
    public class FSMComponent : MonoBehaviour, IFSMComponent
    {
        //状态机列表
        private readonly List<IStateMachine> machines = new List<IStateMachine>();

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
        public T Create<T>(string stateMachineName) where T : IStateMachine, new()
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
            var targetMachine = machines.Find(m => m.Name == stateMachineName);
            if (targetMachine != null)
            {
                targetMachine.OnTermination();
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
                stateMachine.OnTermination();
                machines.Remove(stateMachine);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 状态机是否存在
        /// </summary>
        /// <param name="stateMachineName">状态机名称</param>
        /// <returns>true：存在  false：不存在</returns>
        public bool IsExists(string stateMachineName)
        {
            return machines.FindIndex(m => m.Name == stateMachineName) != -1;
        }
        /// <summary>
        /// 状态机是否存在
        /// </summary>
        /// <param name="stateMachineName">状态机名称</param>
        /// <returns>true：存在  false：不存在</returns>
        public bool IsExists<T>() where T : IStateMachine, new()
        {
            return IsExists(typeof(T).Name);
        }

        /// <summary>
        /// 获取状态机
        /// </summary>
        /// <typeparam name="T">状态机类型</typeparam>
        /// <param name="stateMachineName">状态机名称</param>
        /// <returns>状态机</returns>
        public T Get<T>(string stateMachineName) where T : IStateMachine, new()
        {
            var target = machines.Find(m => m.Name == stateMachineName);
            return target != null ? (T)target : default;
        }
        /// <summary>
        /// 获取状态机
        /// </summary>
        /// <typeparam name="T">状态机类型</typeparam>
        /// <returns>状态机</returns>
        public T Get<T>() where T : IStateMachine, new()
        {
            return Get<T>(typeof(T).Name);
        }

        /// <summary>
        /// 尝试获取状态机
        /// </summary>
        /// <typeparam name="T">状态机类型</typeparam>
        /// <param name="stateMachineName">状态机名称</param>
        /// <param name="stateMachine">状态机</param>
        /// <returns>true：获取成功  false：获取失败</returns>
        public bool TryGet<T>(string stateMachineName, out T stateMachine) where T : IStateMachine, new()
        {
            int index = machines.FindIndex(m => m.Name == stateMachineName);
            stateMachine = index != -1 ? (T)machines[index] : default;
            return index != -1;
        }

        /// <summary>
        /// 尝试获取状态机
        /// </summary>
        /// <typeparam name="T">状态机类型</typeparam>
        /// <param name="stateMachine">状态机</param>
        /// <returns>true：获取成功  false：获取失败</returns>
        public bool TryGet<T>(out T stateMachine)where T : IStateMachine, new()
        {
            return TryGet<T>(typeof(T).Name, out stateMachine);
        }
    }
}