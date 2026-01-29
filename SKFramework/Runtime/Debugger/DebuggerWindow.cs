/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

namespace SK.Framework.Debugger
{
    public abstract class DebuggerWindow : IDebuggerWindow
    {
        public string title { get; internal set; }

        public virtual void OnInitialized() { }
        public virtual void OnDestroy() { }
        public virtual void OnEnter() { }
        public virtual void OnExit() { }

        public abstract void OnGUI();
    }
}