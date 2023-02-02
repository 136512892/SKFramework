using System;
using UnityEngine.Events;

namespace SK.Framework.Actions
{
    public interface IActionChain 
    {
        IActionChain Append(IAction action);

        IActionChain Begin();

        void Stop();

        void Pause();

        void Resume();

        bool IsPaused { get; }

        IActionChain StopWhen(Func<bool> predicate);

        IActionChain OnStop(UnityAction action);

        IActionChain SetLoops(int loops);
    }
}