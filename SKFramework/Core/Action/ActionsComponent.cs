using UnityEngine;
using System.Collections;

namespace SK.Framework.Actions
{
    [DisallowMultipleComponent]
    [AddComponentMenu("SKFramework/Actions")]
    public class ActionsComponent : MonoBehaviour
    {
        public IActionChain Sequence()
        {
            return new SequenceActionChain(this);
        }
        public IActionChain Sequence(MonoBehaviour executer)
        {
            return new SequenceActionChain(executer);
        }

        public IActionChain Concurrent()
        {
            return new ConcurrentActionChain(this);
        }
        public IActionChain Concurrent(MonoBehaviour executer)
        {
            return new ConcurrentActionChain(executer);
        }

        public TimelineActionChain Timeline()
        {
            return new TimelineActionChain(this);
        }
        public TimelineActionChain Timeline(MonoBehaviour executer)
        {
            return new TimelineActionChain(executer);
        }

        public void Execute(IAction action, MonoBehaviour executer)
        {
            executer.StartCoroutine(ExecuteCoroutine(action));
        }
        private IEnumerator ExecuteCoroutine(IAction self)
        {
            while (!self.Invoke())
            {
                yield return null;
            }
        }
    }
}