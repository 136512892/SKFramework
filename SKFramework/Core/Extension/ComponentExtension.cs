using UnityEngine;

namespace SK.Framework
{
    public static class ComponentExtension
    {
        public static T Activate<T>(this T self) where T : Component
        {
            self.gameObject.SetActive(true);
            return self;
        }
        public static T Deactivate<T>(this T self) where T : Component
        {
            self.gameObject.SetActive(false);
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
        public static T SetLayer<T>(this T self, int layer) where T : Component
        {
            self.gameObject.layer = layer;
            return self;
        }
        public static T SetLayer<T>(this T self, string layer) where T : Component
        {
            self.gameObject.layer = LayerMask.NameToLayer(layer);
            return self;
        }
        public static T SetTag<T>(this T self, string tag) where T : Component
        {
            self.gameObject.tag = tag;
            return self;
        }
        public static T GetOrAddComponent<T>(this GameObject self) where T : Component
        {
            T retT = self.GetComponent<T>();
            return retT ?? self.AddComponent<T>();
        }
    }
}