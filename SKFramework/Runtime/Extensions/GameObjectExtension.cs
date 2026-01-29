/*============================================================
 * SKFramework
 * Copyright © 2019-2026 Zhang Shoukun. All rights reserved.
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

        public static GameObject ActivateParent(this GameObject self, bool active)
        {
            var parent = self.transform.parent;
            if (parent != null)
                parent.gameObject.SetActive(active);
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

        public static GameObject SetLayer(this GameObject self, string layer)
        {
            self.layer = LayerMask.NameToLayer(layer);
            return self;
        }

        public static GameObject SetLayer(this GameObject self, int layer)
        {
            self.layer = layer;
            return self;
        }

        public static GameObject SetLayerRecursively(this GameObject self, int layer)
        {
            SetLayerRecursivelyInternal(self, layer);
            return self;
        }
        private static void SetLayerRecursivelyInternal(this GameObject self, int layer)
        {
            self.layer = layer;
            foreach (Transform child in self.transform)
            {
                SetLayerRecursively(child.gameObject, layer);
            }
        }

        public static GameObject SetTag(this GameObject self, string tag)
        {
            self.tag = tag;
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

        public static T GetOrAddComponent<T>(this GameObject self) where T : Component
        {
            T component = self.GetComponent<T>();
            return component ?? self.AddComponent<T>();
        }

        public static GameObject RemoveComponent<T>(this GameObject self) where T : Component
        {
            T component = self != null ? self.GetComponent<T>() : null;
            if (component != null)
                Object.Destroy(component);
            return self;
        }
    }

    public static class ComponentExtension
    {
        public static T Activate<T>(this T self, bool active) where T : Component
        {
            self.gameObject.SetActive(active);
            return self;
        }

        public static T ActiveInvert<T>(this T self) where T : Component
        {
            self.gameObject.SetActive(!self.gameObject.activeSelf);
            return self;
        }

        public static T SetName<T>(this T self, string name) where T : Component
        {
            self.gameObject.name = name;
            return self;
        }

        public static T SetLayer<T>(this T self, string layer) where T : Component
        {
            self.gameObject.layer = LayerMask.NameToLayer(layer);
            return self;
        }

        public static T SetLayer<T>(this T self, int layer) where T : Component
        {
            self.gameObject.layer = layer;
            return self;
        }

        public static T SetLayerRecursively<T>(this T self, int layer) where T : Component
        {
            SetLayerRecursively(self.gameObject, layer);
            return self;
        }

        private static void SetLayerRecursively(this GameObject self, int layer)
        {
            self.layer = layer;
            foreach (Transform child in self.transform)
            {
                SetLayerRecursively(child.gameObject, layer);
            }
        }

        public static T SetTag<T>(this T self, string tag) where T : Component
        {
            self.gameObject.tag = tag;
            return self;
        }

        public static bool TryGetComponentInChildren<T>(this Component self, out T component) where T : Component
        {
            component = null;
            foreach (Transform child in self.transform)
            {
                if (child.gameObject.TryGetComponent(out component))
                    return true;
            }
            return false;
        }
    }
}