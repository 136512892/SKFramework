using UnityEngine.Events;

namespace SK.Framework.Actions
{
    /// <summary>
    /// 普通事件
    /// </summary>
    public class SimpleAction : AbstractAction
    {
        public SimpleAction(UnityAction action)
        {
            onCompleted = action;
        }

        protected override void OnInvoke()
        {
            isCompleted = true;
        }
    }
}