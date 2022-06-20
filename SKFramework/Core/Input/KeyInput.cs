using System;
using UnityEngine;

namespace SK.Framework
{
    [Serializable]
    public sealed class KeyInput : AbstractInput<KeyCode, bool>
    {
        public override bool IsValid
        {
            get
            {
                return key != KeyCode.None;
            }
        }

        public bool IsPressed
        {
            get
            {
                return InputMaster.Toggle && InputMaster.Key.GetKeyDown(this);
            }
        }
        public bool IsHeld
        {
            get
            {
                return InputMaster.Toggle && InputMaster.Key.GetKey(this);
            }
        }
        public bool IsReleased
        {
            get
            {
                return InputMaster.Toggle && InputMaster.Key.GetKeyUp(this);
            }
        }

        public KeyInput() { }
        public KeyInput(KeyCode keyCode) : base(keyCode) { }

        protected override bool IsEqual(KeyCode k1, KeyCode k2)
        {
            return k1 == k2;
        }
        protected override void Register()
        {
            InputMaster.Key.Register(this);
        }
        protected override void Unregister()
        {
            InputMaster.Key.Unregister(this);
        }
    }
}