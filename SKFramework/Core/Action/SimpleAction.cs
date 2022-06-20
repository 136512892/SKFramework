using System;

namespace SK.Framework
{
    /// <summary>
    /// 普通事件
    /// </summary>
    public class SimpleAction : AbstractAction
    {
        public SimpleAction(Action action)
        {
            onCompleted = action;
        }

        protected override void OnInvoke()
        {
            isCompleted = true;
        }
    }
}