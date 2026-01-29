/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

namespace SK.Framework.UI
{
    public interface IUIView
    {
        string viewName { get; set; }

        bool isActive { get; }
        
        void OnLoad(object data);

        void OnOpen(object data);

        void OnUpdate();

        void OnClose();

        void OnUnload();
    }
}