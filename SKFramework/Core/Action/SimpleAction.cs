/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;

namespace SK.Framework.Actions
{
    public class SimpleAction : AbstactAction
    {
        public SimpleAction(System.Action action)
        {
            m_OnCompleted = action;
        }

        protected override void OnInvoke()
        {
            base.OnReset();
            m_IsCompleted = true;
        }
    }
}