using UnityEngine;
using UObject = UnityEngine.Object;

namespace SK.Framework
{
    public static class GameObjectExtension
    {
        public static GameObject Activate(this GameObject self)
        {
            self.SetActive(true);
            return self;
        }
        public static GameObject Deactivate(this GameObject self)
        {
            self.SetActive(false);
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
        public static GameObject RemoveComponent<T>(this GameObject self) where T : Component
        {
            T targetComponent = self.GetComponent<T>();
            if (null == targetComponent)
            {
                UObject.Destroy(targetComponent);
            }
            return self;
        }
        public static void Destroy(this GameObject self)
        {
            if (null != self)
            {
                UObject.Destroy(self);
            }
        }
        public static void Destroy(this GameObject self, float delay)
        {
            if (null != self)
            {
                UObject.Destroy(self, delay);
            }
        }
    }
}