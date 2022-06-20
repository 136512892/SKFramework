using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace SK.Framework
{
    [CustomEditor(typeof(FSM))]
    [CSDNUrl("https://coderz.blog.csdn.net/article/details/124760502?spm=1001.2014.3001.5502")]
    public class FSMInspector : AbstractEditor<FSM>
    {
        private class GUIContents
        {
            public static GUIContent switch2Next = new GUIContent("Next", "切换到下一状态");
            public static GUIContent switch2Last = new GUIContent("Last", "切换到上一状态");
            public static GUIContent switch2Null = new GUIContent("Null", "切换到空状态 （退出当前状态）");
        }
        private List<StateMachine> machines;
        private FieldInfo statesFieldInfo;
        private int currentMachineIndex;
        private string[] machinesName;

        protected override bool IsEnableBaseInspectorGUI
        {
            get
            {
                return true;
            }
        }

        protected override void OnRuntimeEnable()
        {
            //通过反射获取状态机列表
            machines = typeof(FSM).GetField("machines", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(FSM.Instance) as List<StateMachine>;
            //获取状态列表字段
            statesFieldInfo = typeof(StateMachine).GetField("states", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        protected override void OnRuntimeInspectorGUI()
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
                var states = statesFieldInfo.GetValue(currentMachine) as List<IState>;

                GUILayout.BeginHorizontal();
                //提供切换到上一状态的Button按钮
                if (GUILayout.Button(GUIContents.switch2Last, "ButtonLeft"))
                {
                    currentMachine.Switch2Last();
                }
                //提供切换到下一状态的Button按钮
                if (GUILayout.Button(GUIContents.switch2Next, "ButtonMid"))
                {
                    currentMachine.Switch2Next();
                }
                //提供切换到空状态的Button按钮
                if (GUILayout.Button(GUIContents.switch2Null, "ButtonRight"))
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
                    GUILayout.Label(state.Name);

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
}