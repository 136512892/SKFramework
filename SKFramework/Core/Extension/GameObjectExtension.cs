using System;
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
        public static void ActiveReplace(this GameObject self, GameObject target, bool destroy)
        {
            if (destroy) UObject.Destroy(self);
            else self.SetActive(false);
            target.SetActive(true);
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
            if(null == targetComponent)
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
        public static T GetOrAddComponent<T>(this GameObject self) where T : Component
        {
            T retT = self.GetComponent<T>();
            return retT ?? self.AddComponent<T>();
        }

        public static bool IsActiveSelf<T>(this T self) where T : Component
        {
            return self.gameObject.activeSelf;
        }
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
        public static void DestroyGameObject<T>(this T self) where T : Component
        {
            if (self && self.gameObject)
            {
                UObject.Destroy(self.gameObject);
            }
        }
        public static void DestroyGameObject<T>(this T self, float delay) where T : Component
        {
            if(self && self.gameObject)
            {
                UObject.Destroy(self.gameObject, delay);
            }
        }
        public static Component GetOrAddComponent<T>(this T self, Type type) where T : Component
        {
            Component retComponent = self.gameObject.GetComponent(type);
            return retComponent != null ? retComponent : self.gameObject.AddComponent(type);
        }

        public static Mesh GetMeshFromMeshFilter(this GameObject self)
        {
            MeshFilter meshFilter = self.GetComponent<MeshFilter>();
            return meshFilter && meshFilter.sharedMesh ? meshFilter.sharedMesh : null;
        }
        public static Mesh GetMeshFromSkinnedMeshRenderer(this GameObject self)
        {
            SkinnedMeshRenderer skinnedMeshRenderer = self.GetComponent<SkinnedMeshRenderer>();
            return skinnedMeshRenderer && skinnedMeshRenderer.sharedMesh ? skinnedMeshRenderer.sharedMesh : null;
        }
        public static Material GetMaterial(this GameObject self)
        {
            MeshRenderer meshRenderer = self.GetComponent<MeshRenderer>();
            return meshRenderer ? meshRenderer.material : null;
        }
        public static bool IsVisible(this GameObject self, Camera camera)
        {
            Collider collider = self.GetComponent<Collider>();
            if (null == collider) return false;
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
            return GeometryUtility.TestPlanesAABB(planes, collider.bounds);
        }
    }
}