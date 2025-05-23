/*============================================================
 * SKFramework
 * Copyright © 2019-2025 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SK.Framework
{
    public static class GraphicExtension
    {
        public static T SetColor<T>(this T self, Color color) where T : Graphic
        {
            self.color = color;
            return self;
        }

        public static T SetColor<T>(this T self, float r, float g, float b) where T : Graphic
        {
            Color color = self.color;
            color.r = r;
            color.g = g;
            color.b = b;
            self.color = color;
            return self;
        }

        public static T SetColor<T>(this T self, float r, float g, float b, float a) where T : Graphic
        {
            Color color = self.color;
            color.r = r;
            color.g = g;
            color.b = b;
            color.a = a;
            self.color = color;
            return self;
        }

        public static T SetAlpha<T>(this T self, float alpha) where T : Graphic
        {
            Color color = self.color;
            color.a = alpha;
            self.color = color;
            return self;
        }

        public static T SetMaterial<T>(this T self, Material material) where T : Graphic
        {
            self.material = material;
            return self;
        }

        public static T SetRaycastTarget<T>(this T self, bool raycastTarget) where T : Graphic
        {
            self.raycastTarget = raycastTarget;
            return self;
        }

        public static T AddEventTrigger<T>(this T self, EventTriggerType eventTriggerType,
            UnityAction<BaseEventData> action) where T : Graphic
        {
            if (!self.TryGetComponent<EventTrigger>(out var eventTrigger))
                eventTrigger = self.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventTriggerType };
            entry.callback.AddListener(action);
            eventTrigger.triggers.Add(entry);
            return self;
        }
    }
}