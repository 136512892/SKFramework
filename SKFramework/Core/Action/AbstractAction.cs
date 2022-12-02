using UnityEngine.Events;

namespace SK.Framework.Actions
{
    public abstract class AbstractAction : IAction
    {
        protected bool isCompleted;

        protected UnityAction onCompleted;

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