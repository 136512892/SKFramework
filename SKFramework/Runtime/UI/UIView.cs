/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;

namespace SK.Framework.UI
{
    public abstract class UIView : MonoBehaviour, IUIView
    {
        public string viewName { get; set; }

        void IUIView.OnLoad(object data) => OnLoad(data);
        void IUIView.OnOpen(object data) => OnOpen(data);
        void IUIView.OnUpdate() => OnUpdate();
        void IUIView.OnClose() => OnClose();
        void IUIView.OnUnload() => OnUnload();
        
        protected internal virtual void OnLoad(object data) { }
        protected internal virtual void OnOpen(object data) { }
        protected internal virtual void OnUpdate() { }
        protected internal virtual void OnClose() { }
        protected internal virtual void OnUnload() { }
    }
}