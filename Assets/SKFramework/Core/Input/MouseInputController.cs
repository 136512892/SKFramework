using System;
using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework
{
    public sealed class MouseInputController
    {
        private class MouseButtonInputInfo
        {
            public readonly int mouseButton;
            public readonly List<MouseButtonInput> mouseButtonInputs;
            public InputState state = InputState.None;

            public MouseButtonInputInfo(int mouseButton)
            {
                this.mouseButton = mouseButton;
                mouseButtonInputs = new List<MouseButtonInput>();
            }
            public void Reset()
            {
                state = InputState.None;
            }
        }

        private readonly List<MouseButtonInput> mouseButtonInputs;
        private readonly List<MouseButtonInputInfo> infos;
        private readonly Dictionary<int, InputTrigger> triggers;

        public MouseInputController()
        {
            mouseButtonInputs = new List<MouseButtonInput>();
            infos = new List<MouseButtonInputInfo>();
            triggers = new Dictionary<int, InputTrigger>();
        }
        private Vector2 cacheMousePosition;
        public Vector2 Delta { get; private set; }

        public void Update()
        {
            Vector2 currentMousePosition = Input.mousePosition;
            Delta = currentMousePosition - cacheMousePosition;
            for (int i = 0; i < mouseButtonInputs.Count; i++)
            {
                mouseButtonInputs[i].Value = Input.GetMouseButton(mouseButtonInputs[i].Key);
            }
            for (int i = 0; i < infos.Count; i++)
            {
                var info = infos[i];
                bool flag = false;
                for (int j = info.mouseButtonInputs.Count - 1; j >= 0; j--)
                {
                    if (info.mouseButtonInputs[j].Value)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    info.state = info.state == InputState.None || info.state == InputState.Released
                        ? InputState.Pressed
                        : InputState.Held;
                    triggers.Clear();
                }
                else if (triggers.ContainsKey(info.mouseButton))
                {
                    switch (triggers[info.mouseButton].type)
                    {
                        case InputTriggerType.Pressed:
                            info.state = InputState.Pressed;
                            triggers.Remove(info.mouseButton);
                            break;
                        case InputTriggerType.Held:
                            info.state = InputState.Held;
                            if (triggers[info.mouseButton].disposeWhen.Invoke())
                                triggers.Remove(info.mouseButton);
                            break;
                        case InputTriggerType.Released:
                            info.state = InputState.Released;
                            triggers.Remove(info.mouseButton);
                            break;
                    }
                }
                else
                {
                    info.state = info.state == InputState.Pressed || info.state == InputState.Held
                        ? InputState.Released
                        : InputState.None;
                }
            }
            cacheMousePosition = Input.mousePosition;
        }
        public void Reset()
        {
            for (int i = 0; i < mouseButtonInputs.Count; i++)
            {
                mouseButtonInputs[i].Reset();
            }
            cacheMousePosition = Input.mousePosition;
            Delta = Vector2.zero;
        }

        public bool Register(MouseButtonInput mouseButtonInput)
        {
            if (mouseButtonInputs.Contains(mouseButtonInput))
            {
                return false;
            }
            mouseButtonInputs.Add(mouseButtonInput);

            var target = infos.Find(m => m.mouseButton == mouseButtonInput.Key);
            if (target == null)
            {
                target = new MouseButtonInputInfo(mouseButtonInput.Key);
                infos.Add(target);
            }
            target.mouseButtonInputs.Add(mouseButtonInput);
            return true;
        }

        public bool Unregister(MouseButtonInput mouseButtonInput)
        {
            if (!mouseButtonInputs.Contains(mouseButtonInput))
            {
                return false;
            }
            mouseButtonInputs.Remove(mouseButtonInput);

            var target = infos.Find(m => m.mouseButton == mouseButtonInput.Key);
            if (target != null)
            {
                target.mouseButtonInputs.Remove(mouseButtonInput);
                if (target.mouseButtonInputs.Count == 0)
                {
                    infos.Remove(target);
                }
            }
            return true;
        }

        public bool GetKeyDown(MouseButtonInput mouseButtonInput)
        {
            var target = infos.Find(m => m.mouseButton == mouseButtonInput.Key);
            return target != null && target.state == InputState.Pressed;
        }
        public bool GetKey(MouseButtonInput mouseButtonInput)
        {
            var target = infos.Find(m => m.mouseButton == mouseButtonInput.Key);
            return target != null && (target.state == InputState.Held || target.state == InputState.Pressed);
        }
        public bool GetKeyUp(MouseButtonInput mouseButtonInput)
        {
            var target = infos.Find(m => m.mouseButton == mouseButtonInput.Key);
            return target != null && target.state == InputState.Released;
        }

        public void Trigger(int mouseButton, InputTriggerType type)
        {
            if (mouseButton == 0 || mouseButton == 1 || mouseButton == 2)
            {
                if (!triggers.ContainsKey(mouseButton) && type != InputTriggerType.Held)
                {
                    triggers.Add(mouseButton, new InputTrigger(type));
                    Debug.Log($"触发鼠标按键 [{mouseButton}] {(type == InputTriggerType.Pressed ? "按下" : "抬起")}");
                }
            }
        }
        public void Trigger(int mouseButton, InputTriggerType type, Func<bool> disposeWhen)
        {
            if (mouseButton == 0 || mouseButton == 1 || mouseButton == 2)
            {
                if (!triggers.ContainsKey(mouseButton) && type == InputTriggerType.Held)
                {
                    triggers.Add(mouseButton, new InputTrigger(type, disposeWhen));
                    Debug.Log($"触发鼠标按键 [{mouseButton}] 持续按下");
                }
            }
        }
        public void Trigger(MouseButtonCode mouseButtonCode, InputTriggerType type)
        {
            Trigger((int)mouseButtonCode, type);
        }
        public void Trigger(MouseButtonCode mouseButtonCode, InputTriggerType type, Func<bool> disposeWhen)
        {
            Trigger((int)mouseButtonCode, type, disposeWhen);
        }
    }
}