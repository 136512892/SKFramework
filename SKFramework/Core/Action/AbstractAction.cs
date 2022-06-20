using System;

namespace SK.Framework
{
    /// <summary>
    /// 抽象事件
    /// </summary>
    public abstract class AbstractAction : IAction
    {
        //事件是否完成标识
        protected bool isCompleted;
        //完成事件
        protected Action onCompleted;

        public bool Invoke()
        {
            if (!isCompleted)
            {
                OnInvoke(); 
            }
            if (isCompleted)
            {
                onCompleted?.Invoke();
            }
            return isCompleted;
        }
        public void Reset()
        {
            isCompleted = false;
            OnReset();
        }

        protected abstract void OnInvoke();
        protected virtual void OnReset() { }
    }
}