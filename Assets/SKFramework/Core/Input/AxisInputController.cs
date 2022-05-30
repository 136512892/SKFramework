using UnityEngine;
using System.Collections.Generic;

namespace SK.Framework
{
    public sealed class AxisInputController
    {
        private class AxisInputInfo
        {
            public readonly string axisName;
            public readonly List<AxisInput> axisInputs;
            public float Value = 0f;
            public float ValueRaw = 0f;

            public AxisInputInfo(string axisName)
            {
                this.axisName = axisName;
                axisInputs = new List<AxisInput>();
            }

            public void Reset()
            {
                Value = 0f;
                ValueRaw = 0f;
            }
        }

        private readonly List<AxisInput> axisInputs;
        private readonly List<AxisInputInfo> infos;
        public float lerpModifier = 20f;

        public AxisInputController()
        {
            axisInputs = new List<AxisInput>();
            infos = new List<AxisInputInfo>();
        }

        public void Update()
        {
            for (int i = 0; i < axisInputs.Count; i++)
            {
                axisInputs[i].Value = Input.GetAxisRaw(axisInputs[i].Key);
            }
            for (int i = 0; i < infos.Count; i++)
            {
                var info = infos[i];
                info.ValueRaw = 0f;
                for (int j = 0; j < info.axisInputs.Count; j++)
                {
                    AxisInput axisInput = info.axisInputs[j];
                    if (axisInput.Value != 0f)
                    {
                        info.ValueRaw = axisInput.Value;
                        break;
                    }
                }
                info.Value = Mathf.Lerp(info.Value, info.ValueRaw, lerpModifier * Time.deltaTime);
                if(info.ValueRaw == 0f && info.Value != 0f)
                {
                    if (Mathf.Abs(info.Value) < 0.025f)
                    {
                        info.Value = 0f;
                    }
                }
            }
        }
        public void Reset()
        {
            for (int i = 0; i < axisInputs.Count; i++)
            {
                axisInputs[i].Reset();
            }
        }

        public bool Register(AxisInput axisInput)
        {
            if (axisInputs.Contains(axisInput))
            {
                return false;
            }
            axisInputs.Add(axisInput);

            var target = infos.Find(m => m.axisName == axisInput.Key);
            if (target == null)
            {
                target = new AxisInputInfo(axisInput.Key);
                infos.Add(target);
            }
            target.axisInputs.Add(axisInput);
            return true;
        }

        public bool Unregister(AxisInput axisInput)
        {
            if (!axisInputs.Contains(axisInput))
            {
                return false;
            }
            axisInputs.Remove(axisInput);

            var target = infos.Find(m => m.axisName == axisInput.Key);
            if (target != null)
            {
                target.axisInputs.Remove(axisInput);
                if (target.axisInputs.Count == 0)
                {
                    infos.Remove(target);
                }
            }
            return true;
        }

        public float GetAxis(AxisInput axisInput)
        {
            var target = infos.Find(m => m.axisName == axisInput.Key);
            return target != null ? target.Value : 0f;
        }
        public float GetAxisRaw(AxisInput axisInput)
        {
            var target = infos.Find(m => m.axisName == axisInput.Key);
            return target != null ? target.ValueRaw : 0f;
        }
    }
}