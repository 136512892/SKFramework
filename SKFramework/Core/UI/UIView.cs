/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;

namespace SK.Framework.UI
{
    public class UIView : MonoBehaviour, IUIView
    {
        public string viewName { get; set; }

        public virtual void OnLoad(object data = null) { }

        public virtual void OnOpen(object data = null) 
        {
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }

        public virtual void OnClose() 
        {
            gameObject.SetActive(false);
        }

        public virtual void OnUnload() { }
    }
}