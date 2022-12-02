using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SK.Framework.Debugger
{
    public class InspectorWindow : IDebuggerWIndow
    {
        private GameObject selected;
        private Component[] components;
        private Vector2 listScroll;
        private Vector2 inspectorScroll;
        private Component currentComponent;

        private Dictionary<string, IComponentInspector> inspectorDic;

        public void OnInitilization()
        {
            inspectorDic = new Dictionary<string, IComponentInspector>();

            var assembly = typeof(InspectorWindow).Assembly;
            var types = assembly.GetTypes().Where(m => m.IsSubclassOf(typeof(ComponentInspector))).ToArray();
            for (int i = 0; i < types.Length; i++)
            {
                var type = types[i];
                var attributes = type.GetCustomAttributes(false);
                if (attributes.Any(m => m is ComponentInspectorAttribute))
                {
                    var target = Array.Find(attributes, m => m is ComponentInspectorAttribute);

                    inspectorDic.Add((target as ComponentInspectorAttribute).ComponentType.FullName, Activator.CreateInstance(type) as IComponentInspector);
                }
            }
        }

        public void OnEnter()
        {
            selected = Main.Debugger.currentSelected;
            if (selected != null)
            {
                components = selected.GetComponents<Component>();
                currentComponent = components[0];
            }
        }

        public void OnTermination()
        {
            selected = null;
            components = null;
            currentComponent = null;

            inspectorDic?.Clear();
            inspectorDic = null;
        }

        public void OnWindowGUI()
        {
            if (selected == null || currentComponent == null)
            {
                GUILayout.Label("未选中任何物体");
                return;
            }
            GUILayout.BeginHorizontal("Box");
            {
                bool active = GUILayout.Toggle(selected.activeSelf, string.Empty);
                if (active != selected.activeSelf)
                {
                    selected.SetActive(active);
                }
                selected.name = GUILayout.TextField(selected.name, GUILayout.Width(Screen.width * .1f));
                GUILayout.FlexibleSpace();
                GUILayout.Label(string.Format("Tag:{0}", selected.tag));
                GUILayout.Space(10f);
                GUILayout.Label(string.Format("Layer:{0}", LayerMask.LayerToName(selected.layer)));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical("Box", GUILayout.ExpandHeight(true), GUILayout.Width(Screen.width * .075f));
                OnListGUI();
                GUILayout.EndVertical();

                GUILayout.BeginVertical("Box", GUILayout.ExpandHeight(true));
                OnComponentInspector();
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        private void OnListGUI()
        {
            listScroll = GUILayout.BeginScrollView(listScroll);
            for (int i = 0; i < components.Length; i++)
            {
                if (GUILayout.Toggle(components[i] == currentComponent, components[i].GetType().Name))
                {
                    currentComponent = components[i];
                }
            }
            GUILayout.EndScrollView();
        }

        private void OnComponentInspector()
        {
            inspectorScroll = GUILayout.BeginScrollView(inspectorScroll);
            string name = currentComponent.GetType().FullName;
            if (inspectorDic.ContainsKey(name))
            {
                inspectorDic[name].Draw(currentComponent);
            }
            else
            {
                GUILayout.Label("暂不支持该类型组件的调试");
            }
            GUILayout.EndScrollView();
        }
    }
}