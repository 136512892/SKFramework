/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;

namespace SK.Framework.Actions
{
    public interface IActionChain 
    {
        bool isPaused { get; }
        
        IActionChain Append(IAction action);

        IActionChain Begin();

        void Stop();
        
        void Pause();
        
        void Resume();

        IActionChain StopWhen(Func<bool> predicate);

        IActionChain OnStop(Action onStop);
        
        IActionChain SetLoops(int loops);
    }
}