/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using System;
using UnityEngine;

namespace SK.Framework.UI
{
    public abstract class UIView : MonoBehaviour, IUIView
    {
        [Serializable]
        public class AssetInfo
        {
            public string name;
            public string path;
        }

        public string viewName { get; set; }

        public bool isActive => gameObject.activeSelf;

        void IUIView.OnLoad(object data) => OnLoad(data);
        void IUIView.OnOpen(object data) => OnOpen(data);
        void IUIView.OnUpdate() => OnUpdate();
        void IUIView.OnClose() => OnClose();
        void IUIView.OnUnload() => OnUnload();
        
        protected internal virtual void OnLoad(object data) { }

        protected internal virtual void OnOpen(object data)
        {
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }
        
        protected internal virtual void OnUpdate() { }

        protected internal virtual void OnClose()
        {
            gameObject.SetActive(false);
        }

        protected internal virtual void OnUnload()
        {
            Destroy(gameObject);
        }
    }
}