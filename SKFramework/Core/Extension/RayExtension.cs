using UnityEngine;

namespace SK.Framework
{
    public static class RayExtension
    {
        public static GameObject GetHittedObject(this Ray self)
        {
            if(Physics.Raycast(self, out RaycastHit hitInfo))
            {
                return hitInfo.collider.gameObject;
            }
            return null;
        }
        public static GameObject GetHittedObject(this Ray self, float maxDistance)
        {
            if (Physics.Raycast(self, out RaycastHit hitInfo, maxDistance))
            {
                return hitInfo.collider.gameObject;
            }
            return null;
        }
        public static GameObject GetHittedObject(this Ray self, float maxDistance, int layerMask) 
        {
            if (Physics.Raycast(self, out RaycastHit hitInfo, maxDistance, layerMask))
            {
                return hitInfo.collider.gameObject;
            }
            return null;
        }
        public static bool IsHittedTarget(this Ray self, GameObject target)
        {
            return Physics.Raycast(self, out RaycastHit hitInfo) && hitInfo.collider.gameObject == target;
        }
        public static bool IsHittedTarget(this Ray self, float maxDistance, GameObject target)
        {
            return Physics.Raycast(self, out RaycastHit hitInfo, maxDistance) && hitInfo.collider.gameObject == target;
        }
        public static bool IsHittedTarget(this Ray self, float maxDistance, int layerMask, GameObject target)
        {
            return Physics.Raycast(self, out RaycastHit hitInfo, maxDistance, layerMask) && hitInfo.collider.gameObject == target;
        }
        public static bool IsHittedTarget<T>(this Ray self, out T t) where T : Component
        {
            if (Physics.Raycast(self, out RaycastHit hitInfo))
            {
                t = hitInfo.collider.GetComponent<T>();
                return t != null;
            }
            t = null;
            return false;
        }
        public static bool IsHittedTarget<T>(this Ray self, float maxDistance, out T t) where T : Component
        {
            if (Physics.Raycast(self, out RaycastHit hitInfo, maxDistance))
            {
                t = hitInfo.collider.GetComponent<T>();
                return t != null;
            }
            t = null;
            return false;
        }
        public static bool IsHittedTarget<T>(this Ray self, float maxDistance, int layerMask, out T t) where T : Component
        {
            if (Physics.Raycast(self, out RaycastHit hitInfo, maxDistance, layerMask))
            {
                t = hitInfo.collider.GetComponent<T>();
                return t != null;
            }
            t = null;
            return false;
        }
    }
}