/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace SK.Framework.FSM
{
    [CustomEditor(typeof(FSM))]
    public class FSMEditor : BaseEditor
    {
        private List<IStateMachine> m_Machines;
        private FieldInfo m_StatesFieldInfo;
        private int m_CurrentMachineIndex;
        private string[] m_MachinesName;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (EditorApplication.isPlaying)
            {
                m_Machines = typeof(FSM).GetField("m_Machines", BindingFlags.Instance | BindingFlags.NonPublic)
                    .GetValue(SKFramework.Module<FSM>()) as List<IStateMachine>;
                m_StatesFieldInfo = typeof(StateMachine).GetField("m_States", BindingFlags.Instance | BindingFlags.NonPublic);
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (EditorApplication.isPlaying)
            {
                if (m_MachinesName == null || m_Machines.Count != m_MachinesName.Length)
                {
                    m_CurrentMachineIndex = 0;
                    m_MachinesName = m_Machines.Select(m => m.name).ToArray();
                }
                if (m_Machines.Count > 0)
                {
                    m_CurrentMachineIndex = EditorGUILayout.Popup("State Machine：", m_CurrentMachineIndex, m_MachinesName);
                    var currentMachine = m_Machines[m_CurrentMachineIndex];
                    var states = m_StatesFieldInfo.GetValue(currentMachine) as List<IState>;

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Last", "ButtonLeft"))
                    {
                        currentMachine.Switch2Last();
                    }
                    if (GUILayout.Button("Next", "ButtonMid"))
                    {
                        currentMachine.Switch2Next();
                    }
                    if (GUILayout.Button("Exit", "ButtonRight"))
                    {
                        currentMachine.Exit();
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginVertical("Box");
                    for (int i = 0; i < states.Count; i++)
                    {
                        var state = states[i];
                        GUILayout.BeginHorizontal(currentMachine.currentState == state ? "SelectionRect" : "IN Title");
                        GUILayout.Label(state.name);

                        if (currentMachine.currentState != state)
                        {
                            if (GUILayout.Button("Switch", GUILayout.Width(55f)))
                            {
                                currentMachine.Switch(state.name);
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndVertical();
                }
            }
        }
    }
}