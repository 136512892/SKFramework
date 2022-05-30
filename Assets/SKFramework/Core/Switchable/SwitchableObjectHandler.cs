using UnityEngine;

namespace SK.Framework
{
    /// <summary>
    /// 开关处理器
    /// </summary>
    public class SwitchableObjectHandler : SwitchableObject
    {
        [SerializeField] private SwitchableObject[] handleArray;

        public override void Open()
        {
            if (state == SwitchState.Open) return;
            state = SwitchState.Open;
            for (int i = 0; i < handleArray.Length; i++)
            {
                handleArray[i].Open();
            }
        }
        public override void Close()
        {
            if (state == SwitchState.Close) return;
            state = SwitchState.Close;
            for (int i = 0; i < handleArray.Length; i++)
            {
                handleArray[i].Close();
            }
        }
    }
}