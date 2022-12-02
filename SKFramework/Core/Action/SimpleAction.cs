using UnityEngine.Events;

namespace SK.Framework.Actions
{
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