using UnityEngine;
using UnityEngine.Events;

namespace SK.Framework
{
    /// <summary>
    /// 可瞄准物体基类
    /// </summary>
    public class AimableObject : MonoBehaviour, IAimableObject
    {
        #region NonPublic Variables
        [SerializeField] protected string description;
        [SerializeField] protected float aimableDistance = 2f;
        [SerializeField] protected UnityEvent onEnter;
        [SerializeField] protected UnityEvent onExit;
        [SerializeField] protected UnityEvent onStay;
        #endregion

        #region Public Properties
        /// <summary>
        /// 物体描述
        /// </summary>
        public string Description
        {
            get
            {
                return description;
            }
        }
        /// <summary>
        /// 可瞄准检测的距离
        /// </summary>
        public float AimableDistance
        {
            get
            {
                return aimableDistance;
            }
        }
        #endregion

        #region NonPublic Methods
        //进入事件
        protected virtual void OnEnter() { }
        //退出事件
        protected virtual void OnExit() { }
        //停留事件
        protected virtual void OnStay() { }
        #endregion

        #region Public Methods
        /// <summary>
        /// 瞄准进入
        /// </summary>
        public void Enter()
        {
            OnEnter();
            onEnter?.Invoke();
        }
        /// <summary>
        /// 瞄准退出
        /// </summary>
        public void Exit()
        {
            OnExit();
            onExit?.Invoke();
        }
        /// <summary>
        /// 瞄准停留
        /// </summary>
        public void Stay()
        {
            OnStay();
            onStay?.Invoke();
        }
        #endregion
    }
}