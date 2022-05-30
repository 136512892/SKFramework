using System;

namespace SK.Framework
{
    public sealed class InputTrigger
    {
        public readonly InputTriggerType type;

        public readonly Func<bool> disposeWhen;

        public InputTrigger(InputTriggerType type)
        {
            this.type = type;
        }

        public InputTrigger(InputTriggerType type, Func<bool> disposeWhen)
        {
            this.type = type;
            this.disposeWhen = disposeWhen;
        }
    }
}