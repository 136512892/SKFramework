/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

namespace SK.Framework.Custom
{
    public interface ICustomComponent
    {
        void OnInitialization();

        void OnUpdate();

        void OnTermination();
    }
}