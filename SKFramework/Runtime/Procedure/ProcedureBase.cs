/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

namespace SK.Framework.Procedure
{
    public abstract class ProcedureBase
    {
        protected internal virtual void OnEnter(object data) { }
        
        protected internal virtual void OnUpdate() { }
        
        protected internal virtual void OnExit() { }
    }
}