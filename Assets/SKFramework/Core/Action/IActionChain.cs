using System;

namespace SK.Framework
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

        IActionChain StopWhen(Func<bool> predicate);

        IActionChain OnStop(Action action);

        IActionChain SetLoops(int loops);
    }
}