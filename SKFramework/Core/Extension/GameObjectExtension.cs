/*============================================================
 * SKFramework
 * Copyright © 2019-2024 Zhang Shoukun. All rights reserved.
 * Feedback: mailto:136512892@qq.com
 *============================================================*/

using UnityEngine;

namespace SK.Framework
{
    public static class GameObjectExtension
    {
        public static GameObject Activate(this GameObject self, bool active)
        {
            self.SetActive(active);
            return self;
        }

        public static GameObject ActiveInvert(this GameObject self)
        {
            self.SetActive(!self.activeSelf);
            return self;
        }

        public static GameObject SetName(this GameObject self, string name)
        {
            self.name = name;
            return self;
        }

        public static GameObject SetLayer(this GameObject self, int layer)
        {
            self.layer = layer;
            return self;
        }

        public static GameObject SetLayer(this GameObject self, string layer)
        {
            self.layer = LayerMask.NameToLayer(layer);
            return self;
        }

        public static GameObject SetTag(this GameObject self, string tag)
        {
            self.tag = tag;
            return self;
        }

        public static T GetOrAddComponent<T>(this GameObject self) where T : Component
        {
            T component = self.GetComponent<T>();
            return component ?? self.AddComponent<T>();
        }

        public static GameObject RemoveComponent<T>(this GameObject self) where T : Component
        {
            T component = self.GetComponent<T>();
            if (component != null)
                Object.Destroy(component);
            return self;
        }

        public static void Destroy(this GameObject self)
        {
            if (self != null)
                Object.Destroy(self);
        }

        public static void Destroy(this GameObject self, float delay)
        {
            if (self != null)
                Object.Destroy(self, delay);
        }
    }
}