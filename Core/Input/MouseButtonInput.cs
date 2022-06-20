using System;

namespace SK.Framework
{
    [Serializable]
    public sealed class MouseButtonInput : AbstractInput<int, bool>
    {
        public override bool IsValid
        {
            get
            {
                return key == 0 || key == 1 || key == 2;
            }
        }

        public bool IsPressed
        {
            get
            {
                return InputMaster.Toggle && InputMaster.Mouse.GetKeyDown(this);
            }
        }
        public bool IsHeld
        {
            get
            {
                return InputMaster.Toggle && InputMaster.Mouse.GetKey(this);
            }
        }
        public bool IsReleased
        {
            get
            {
                return InputMaster.Toggle && InputMaster.Mouse.GetKeyUp(this);
            }
        }

        public MouseButtonInput() { }
        public MouseButtonInput(int mouseButton) : base(mouseButton) { }
        public MouseButtonInput(MouseButtonCode mouseButtonCode)
        {
            key = (int)mouseButtonCode;
        }

        protected override bool IsEqual(int k1, int k2)
        {
            return k1 == k2;
        }
        protected override void Register()
        {
            InputMaster.Mouse.Register(this);
        }
        protected override void Unregister()
        {
            InputMaster.Mouse.Unregister(this);
        }
    }
}