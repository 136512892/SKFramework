using System;

namespace SK.Framework
{
    [Serializable]
    public sealed class AxisInput : AbstractInput<string, float>
    {
        public override bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(key);
            }
        }

        public float ReadValue()
        {
            return InputMaster.Toggle ? InputMaster.Axis.GetAxis(this) : 0f;
        }
        public float ReadRawValue()
        {
            return InputMaster.Toggle ? InputMaster.Axis.GetAxisRaw(this) : 0f;
        }

        public AxisInput() { }
        public AxisInput(string axisName) : base(axisName) { }

        protected override bool IsEqual(string k1, string k2)
        {
            return k1 == k2;
        }
        protected override void Register()
        {
            InputMaster.Axis.Register(this);
        }
        protected override void Unregister()
        {
            InputMaster.Axis.Unregister(this);
        }
    }
}