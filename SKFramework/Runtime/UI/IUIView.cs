/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

namespace SK.Framework.UI
{
    public interface IUIView
    {
        string viewName { get; set; }
        
        void OnLoad(object data);

        void OnOpen(object data);

        void OnClose();

        void OnUnload();
    }
}