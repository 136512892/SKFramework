using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SK.Framework.Actions
{
    public class UntilAction : AbstractAction
    {
        private readonly Func<bool> predicate;

        public UntilAction(Func<bool> predicate, UnityAction action)
        {
            this.predicate = predicate;
            onCompleted = action;
        }

        protected override void OnInvoke()
        {
            isCompleted = predicate.Invoke();
        }
    }

    public class UntilUIBehaviourAction : AbstractAction
    {
        public enum Mode
        {
            Enter,
            Click,
            Exit,
        }

        private bool trigger;

        public UntilUIBehaviourAction(UIBehaviour uiBehaviour, Mode mode, UnityAction action)
        {
            EventTrigger eventTrigger = uiBehaviour.GetComponent<EventTrigger>();
            if (eventTrigger == null) eventTrigger = uiBehaviour.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            switch (mode)
            {
                case Mode.Enter: entry.eventID = EventTriggerType.PointerEnter; break;
                case Mode.Click: entry.eventID = EventTriggerType.PointerClick; break;
                case Mode.Exit: entry.eventID = EventTriggerType.PointerExit; break;
            }
            entry.callback.AddListener(OnTriggerEvent);
            eventTrigger.triggers.Add(entry);

            onCompleted = () =>
            {
                eventTrigger.triggers.Remove(entry);
                if (eventTrigger.triggers.Count == 0)
                {
                    UnityEngine.Object.Destroy(eventTrigger);
                }
                action?.Invoke();
            };
        }

        protected override void OnReset()
        {
            base.OnReset();
            trigger = false;
        }

        protected override void OnInvoke()
        {
            isCompleted = trigger;
        }

        private void OnTriggerEvent(BaseEventData eventData)
        {
            trigger = true;
        }
    }

    public static class UntilActionExtension
    {
        public static UntilUIBehaviourAction Enter(this UIBehaviour uiBehaviour, UnityAction action = null)
        {
            return new UntilUIBehaviourAction(uiBehaviour, UntilUIBehaviourAction.Mode.Enter, action);
        }
        public static UntilUIBehaviourAction Click(this UIBehaviour uiBehaviour, UnityAction action = null)
        {
            return new UntilUIBehaviourAction(uiBehaviour, UntilUIBehaviourAction.Mode.Click, action);
        }
        public static UntilUIBehaviourAction Exit(this UIBehaviour uiBehaviour, UnityAction action = null)
        {
            return new UntilUIBehaviourAction(uiBehaviour, UntilUIBehaviourAction.Mode.Exit, action);
        }
        public static IActionChain Until(this IActionChain chain, UntilUIBehaviourAction untilUIBehaviourAction)
        {
            return chain.Append(untilUIBehaviourAction);
        }
    }
}