using UnityEngine;

namespace SK.Framework
{
    /// <summary>
    /// UI视图接口
    /// </summary>
    public interface IUIView
    {
        string Name { get; set; }

        RectTransform RectTransform { get; }

        void Show(IViewData data = null, bool instant = false);

        void Hide(bool instant = false);

        void Init(IViewData viewData = null, bool instant = false);

        void Unload(bool instant = false);
    }
}