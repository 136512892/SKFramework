using System;
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
using System.Reflection;
#endif

namespace SK.Framework.FSM
{
    /// <summary>
    /// 有限状态机管理器
    /// </summary>
    [AddComponentMenu("")]
    internal class FSM : MonoBehaviour
    {
        #region NonPublic Variables
        private static FSM instance;

        //状态机列表
        private List<StateMachine> machines;
        #endregion

        #region Public Properties
        public static FSM Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("[SKFramework.FSM]").AddComponent<FSM>();
                    instance.machines = new List<StateMachine>();
                    DontDestroyOnLoad(instance);
                }
                return instance;
            }
        }
        #endregion

        #region NonPublic Methods
        private void Update()
        {
            for (int i = 0; i < machines.Count; i++)
            {
                //更新状态机
                machines[i].OnUpdate();
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 创建状态机
        /// </summary>
        /// <typeparam name="T">状态机类型</typeparam>
        /// <param name="stateMachineName">状态机名称</param>
        /// <returns>状态机</returns>
        internal T Create<T>(string stateMachineName) where T : StateMachine, new()
        {
            Type type = typeof(T);
            stateMachineName = string.IsNullOrEmpty(stateMachineName) ? type.Name : stateMachineName;
            if (machines.Find(m => m.Name == stateMachineName) == null)
            {
                T machine = (T)Activator.CreateInstance(type);
                machine.Name = stateMachineName;
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
        internal bool Destroy(string stateMachineName)
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
        internal bool Destroy<T>(T stateMachine) where T : StateMachine, new()
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
        internal T GetMachine<T>(string stateMachineName) where T : StateMachine
        {
            return (T)machines.Find(m => m.Name == stateMachineName);
        }
        #endregion
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(FSM))]
    public class FSMEditor : Editor
    {
        private List<StateMachine> machines;
        private FieldInfo statesFieldInfo;
        private int currentMachineIndex;
        private string[] machinesName;

        private readonly GUIContent switch2Next = new GUIContent("Next", "切换到下一状态");
        private readonly GUIContent switch2Last = new GUIContent("Last", "切换到上一状态");
        private readonly GUIContent switch2Null = new GUIContent("Null", "切换到空状态 （退出当前状态）");

        private void OnEnable()
        {
            //通过反射获取状态机列表
            machines = typeof(FSM).GetField("machines", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(FSM.Instance) as List<StateMachine>;
            //获取状态列表字段
            statesFieldInfo = typeof(StateMachine).GetField("states", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public override void OnInspectorGUI()
        {
            //当状态机名称数组为空（初始化） 或数量与状态机数量不等时（状态机列表发生变化）
            if (machinesName == null || machines.Count != machinesName.Length)
            {
                //重置当前状态机索引数值
                currentMachineIndex = 0;
                //重新获取状态机名称数组
                machinesName = machines.Select(m => m.Name).ToArray();
            }

            if (machines.Count > 0)
            {
                currentMachineIndex = EditorGUILayout.Popup("状态机：", currentMachineIndex, machinesName);
                var currentMachine = machines[currentMachineIndex];
                //获取当前状态机的状态列表
                var states = statesFieldInfo.GetValue(currentMachine) as List<State>;

                GUILayout.BeginHorizontal();
                //提供切换到上一状态的Button按钮
                if (GUILayout.Button(switch2Last, "ButtonLeft"))
                {
                    currentMachine.Switch2Last();
                }
                //提供切换到下一状态的Button按钮
                if (GUILayout.Button(switch2Next, "ButtonMid"))
                {
                    currentMachine.Switch2Next();
                }
                //提供切换到空状态的Button按钮
                if (GUILayout.Button(switch2Null, "ButtonRight"))
                {
                    currentMachine.Switch2Null();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginVertical("Box");
                for (int i = 0; i < states.Count; i++)
                {
                    var state = states[i];
                    //如果状态为当前状态 使用SelectionRect Style 否则使用IN Title Style进行区分
                    GUILayout.BeginHorizontal(currentMachine.CurrentState == state ? "SelectionRect" : "IN Title");
                    GUILayout.Label(state.name);

                    //如果状态不是当前状态 提供切换到该状态的Button按钮
                    if (currentMachine.CurrentState != state)
                    {
                        if (GUILayout.Button("Switch", GUILayout.Width(55f)))
                        {
                            currentMachine.Switch(state);
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
        }
    }

#endif
}