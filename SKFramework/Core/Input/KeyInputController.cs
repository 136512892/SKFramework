using System;
using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework
{
    public sealed class KeyInputController
    {
        private class KeyInputInfo
        {
            public readonly KeyCode keyCode;
            public readonly List<KeyInput> keyInputs;
            public InputState state = InputState.None;

            public KeyInputInfo(KeyCode keyCode)
            {
                this.keyCode = keyCode;
                keyInputs = new List<KeyInput>();
            }
            public void Reset()
            {
                state = InputState.None;
            }
        }

        private readonly List<KeyInput> keyInputs;
        private readonly List<KeyInputInfo> infos;
        private readonly Dictionary<KeyCode, InputTrigger> triggers;

        public KeyInputController()
        {
            keyInputs = new List<KeyInput>();
            infos = new List<KeyInputInfo>();
            triggers = new Dictionary<KeyCode, InputTrigger>();
        }
        public void Update()
        {
            for (int i = 0; i < keyInputs.Count; i++)
            {
                keyInputs[i].Value = Input.GetKey(keyInputs[i].Key);
            }
            for (int i = 0; i < infos.Count; i++)
            {
                var info = infos[i];
                bool flag = false;
                for (int j = info.keyInputs.Count - 1; j >= 0; j--)
                {
                    if (info.keyInputs[j].Value)
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
                else if (triggers.ContainsKey(info.keyCode))
                {
                    switch (triggers[info.keyCode].type)
                    {
                        case InputTriggerType.Pressed:
                            info.state = InputState.Pressed;
                            triggers.Remove(info.keyCode);
                            break;
                        case InputTriggerType.Held:
                            info.state = InputState.Held;
                            if (triggers[info.keyCode].disposeWhen.Invoke()) 
                                triggers.Remove(info.keyCode);
                            break;
                        case InputTriggerType.Released:
                            info.state = InputState.Released;
                            triggers.Remove(info.keyCode);
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
        }
        public void Reset()
        {
            for (int i = 0; i < keyInputs.Count; i++)
            {
                keyInputs[i].Reset();
            }
        }

        public bool Register(KeyInput keyInput)
        {
            if (keyInputs.Contains(keyInput))
            {
                return false;
            }
            keyInputs.Add(keyInput);

            var target = infos.Find(m => m.keyCode == keyInput.Key);
            if (target == null)
            {
                target = new KeyInputInfo(keyInput.Key);
                infos.Add(target);
            }
            target.keyInputs.Add(keyInput);
            Log.Info("<color=cyan><b>[SKFramework.Input.Info]</b></color> 注册键盘按键[{0}]输入监听", keyInput.Key);
            return true;
        }
        public bool Unregister(KeyInput keyInput)
        {
            if (!keyInputs.Contains(keyInput))
            {
                return false;
            }
            keyInputs.Remove(keyInput);

            var target = infos.Find(m => m.keyCode == keyInput.Key);
            if (target != null)
            {
                target.keyInputs.Remove(keyInput);
                if (target.keyInputs.Count == 0)
                {
                    infos.Remove(target);
                }
            }
            Log.Info("<color=cyan><b>[SKFramework.Input.Info]</b></color> 注销键盘按键[{0}]输入监听", keyInput.Key);
            return true;
        }

        public bool GetKeyDown(KeyInput keyInput)
        {
            var target = infos.Find(m => m.keyCode == keyInput.Key);
            return target != null && target.state == InputState.Pressed;
        }
        public bool GetKey(KeyInput keyInput)
        {
            var target = infos.Find(m => m.keyCode == keyInput.Key);
            return target != null && (target.state == InputState.Held || target.state == InputState.Pressed);
        }
        public bool GetKeyUp(KeyInput keyInput)
        {
            var target = infos.Find(m => m.keyCode == keyInput.Key);
            return target != null && target.state == InputState.Released;
        }

        public void Trigger(KeyCode keyCode, InputTriggerType type)
        {
            if (keyCode != KeyCode.None && !triggers.ContainsKey(keyCode) && type != InputTriggerType.Held)
            {
                triggers.Add(keyCode, new InputTrigger(type));
                Log.Info("<color=cyan><b>[SKFramework.Input.Info]</b></color> 触发键盘按键[{0}]{1}", keyCode, type == InputTriggerType.Pressed ? "按下" : "抬起");
            }
        }
        public void Trigger(KeyCode keyCode, InputTriggerType type, Func<bool> disposeWhen)
        {
            if (keyCode != KeyCode.None && !triggers.ContainsKey(keyCode) && type == InputTriggerType.Held)
            {
                triggers.Add(keyCode, new InputTrigger(type, disposeWhen));
                Log.Info("<color=cyan><b>[SKFramework.Input.Info]</b></color> 触发键盘按键[{0}]持续按下", keyCode);
            }
        }
    }
}