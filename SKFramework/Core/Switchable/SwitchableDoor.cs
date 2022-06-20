using UnityEngine;

namespace SK.Framework
{
    /// <summary>
    /// 可开关门
    /// </summary>
    public abstract class SwitchableDoor : SwitchableObject
    {
        //开/关所用的时长
        [SerializeField] protected float duration = 0.5f;
        //打开状态的值
        protected Vector3 openValue;
        //关闭状态的值
        protected Vector3 closeValue;
    }
}