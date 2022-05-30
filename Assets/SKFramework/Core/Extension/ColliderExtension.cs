using UnityEngine;

namespace SK.Framework
{
    public static class ColliderExtension
    {
        public static T SetIsTrigger<T>(this T self, bool isTrigger) where T : Collider
        {
            self.isTrigger = isTrigger;
            return self;
        }
        public static T SetMaterial<T>(this T self, PhysicMaterial physicMaterial) where T : Collider
        {
            self.material = physicMaterial;
            return self;
        }
        public static T SetBoxCenter<T>(this T self, Vector3 center) where T : BoxCollider
        {
            self.center = center;
            return self;
        }
        public static T SetSize<T>(this T self, Vector3 size) where T : BoxCollider
        {
            self.size = size;
            return self;
        }
        public static T SetCapsuleCenter<T>(this T self, Vector3 center) where T : CapsuleCollider
        {
            self.center = center;
            return self;
        }
        public static T SetCapsuleRadius<T>(this T self, float radius) where T : CapsuleCollider
        {
            self.radius = radius;
            return self;
        }
        public static T SetHeight<T>(this T self, float height) where T : CapsuleCollider
        {
            self.height = height;
            return self;
        }
        public static T SetDirection<T>(this T self, int direction) where T : CapsuleCollider
        {
            self.direction = direction;
            return self;
        }
        public static T SetSphereCenter<T>(this T self, Vector3 center) where T : SphereCollider
        {
            self.center = center;
            return self;
        }
        public static T SetSphereRadius<T>(this T self, float radius) where T : SphereCollider
        {
            self.radius = radius;
            return self;
        }
    }
}