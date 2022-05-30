using UnityEngine;

namespace SK.Framework
{
    public abstract class SwitchableObject : MonoBehaviour, ISwitchableObject
    {
        //默认设为关闭状态
        [SerializeField] protected SwitchState state = SwitchState.Close;

        /// <summary>
        /// 当前状态
        /// </summary>
        public SwitchState State { get { return state; } }

        /// <summary>
        /// 切换 若为打开状态则关闭 若为关闭状态则打开
        /// </summary>
        public void Switch()
        {
            switch (State)
            {
                case SwitchState.Open: Close(); break;
                case SwitchState.Close: Open(); break;
            }
        }

        /// <summary>
        /// 开门
        /// </summary>
        public abstract void Open();
        /// <summary>
        /// 关门
        /// </summary>
        public abstract void Close();
    }
}