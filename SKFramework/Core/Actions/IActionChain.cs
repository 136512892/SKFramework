using UnityEngine.Events;

namespace SK.Framework.Actions
{
    /// <summary>
    /// 事件链接口
    /// </summary>
    public interface IActionChain 
    {
        IActionChain Append(IAction action);

        IActionChain Begin();

        void Stop();

        void Pause();

        void Resume();

        bool IsPaused { get; }

        IActionChain StopWhen(System.Func<bool> predicate);

        IActionChain OnStop(UnityAction action);

        IActionChain SetLoops(int loops);
    }
}