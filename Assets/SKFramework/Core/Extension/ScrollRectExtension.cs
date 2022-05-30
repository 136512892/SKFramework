using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace SK.Framework 
{
    public static class ScrollRectExtension
    {
        public static ScrollRect SetContent(this ScrollRect self, RectTransform content)
        {
            self.content = content;
            return self;
        }
        public static ScrollRect SetHorizontal(this ScrollRect self, bool horizontal)
        {
            self.horizontal = horizontal;
            return self;
        }
        public static ScrollRect SetVertical(this ScrollRect self, bool vertical)
        {
            self.vertical = vertical;
            return self;
        }
        public static ScrollRect SetMovementType(this ScrollRect self, ScrollRect.MovementType movementType)
        {
            self.movementType = movementType;
            return self;
        }
        public static ScrollRect SetElasticity(this ScrollRect self, float elasticity)
        {
            self.elasticity = elasticity;
            return self;
        }
        public static ScrollRect SetInertia(this ScrollRect self, bool inertia)
        {
            self.inertia = inertia;
            return self;
        }
        public static ScrollRect SetDecelerationRate(this ScrollRect self, float decelerationRate)
        {
            self.decelerationRate = decelerationRate;
            return self;
        }
        public static ScrollRect SetScrollSensitivity(this ScrollRect self, float scrollSensitivity)
        {
            self.scrollSensitivity = scrollSensitivity;
            return self;
        }
        public static ScrollRect SetViewport(this ScrollRect self, RectTransform viewport)
        {
            self.viewport = viewport;
            return self;
        }
        public static ScrollRect SetHorizontalScrollbar(this ScrollRect self, Scrollbar horizontalScrollbar)
        {
            self.horizontalScrollbar = horizontalScrollbar;
            return self;
        }
        public static ScrollRect SetVerticalScrollbar(this ScrollRect self, Scrollbar verticalScrollbar)
        {
            self.verticalScrollbar = verticalScrollbar;
            return self;
        }
        public static ScrollRect SetOnValueChanged(this ScrollRect self, UnityAction<Vector2> unityAction)
        {
            self.onValueChanged.AddListener(unityAction);
            return self;
        }
    }
}