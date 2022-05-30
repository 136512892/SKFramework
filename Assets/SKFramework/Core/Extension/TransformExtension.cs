using UnityEngine;
using System.Text;
using System.Collections.Generic;

namespace SK.Framework
{
    public static class TransformExtension
    {
        public static T SetSiblingIndex<T>(this T self, int index) where T : Component
        {
            self.transform.SetSiblingIndex(index);
            return self;
        }
        public static T SetAsFirstSibling<T>(this T self) where T : Component
        {
            self.transform.SetAsFirstSibling();
            return self;
        }
        public static T SetAsLastSibling<T>(this T self) where T : Component
        {
            self.transform.SetAsLastSibling();
            return self;
        }
        public static T GetSiblingIndex<T>(this T self, out int index) where T : Component
        {
            index = self.transform.GetSiblingIndex();
            return self;
        }
        public static T GetPosition<T>(this T self, out Vector3 position) where T : Component
        {
            position = self.transform.position;
            return self;
        }
        public static T SetPosition<T>(this T self, Vector3 pos) where T : Component
        {
            self.transform.position = pos;
            return self;
        }
        public static T SetPosition<T>(this T self, float x, float y, float z) where T : Component
        {
            Vector3 pos = self.transform.position;
            pos.x = x;
            pos.y = y;
            pos.z = z;
            self.transform.position = pos;
            return self;
        }
        public static T SetPositionX<T>(this T self, float x) where T : Component
        {
            Vector3 pos = self.transform.position;
            pos.x = x;
            self.transform.position = pos;
            return self;
        }
        public static T SetPositionY<T>(this T self, float y) where T : Component
        {
            Vector3 pos = self.transform.position;
            pos.y = y;
            self.transform.position = pos;
            return self;
        }
        public static T SetPositionZ<T>(this T self, float z) where T : Component
        {
            Vector3 pos = self.transform.position;
            pos.z = z;
            self.transform.position = pos;
            return self;
        }
        public static T PositionIdentity<T>(this T self) where T : Component
        {
            self.transform.position = Vector3.zero;
            return self;
        }
        public static T RotationIdentity<T>(this T self) where T : Component
        {
            self.transform.rotation = Quaternion.identity;
            return self;
        }
        public static T SetEulerAngles<T>(this T self, Vector3 eulerAngles) where T : Component
        {
            self.transform.eulerAngles = eulerAngles;
            return self;
        }
        public static T SetEulerAngles<T>(this T self, float x, float y, float z) where T : Component
        {
            Vector3 eulerAngles = self.transform.eulerAngles;
            eulerAngles.x = x;
            eulerAngles.y = y;
            eulerAngles.z = z;
            self.transform.eulerAngles = eulerAngles;
            return self;
        }
        public static T SetEulerAnglesX<T>(this T self, float x) where T : Component
        {
            Vector3 eulerAngles = self.transform.eulerAngles;
            eulerAngles.x = x;
            self.transform.eulerAngles = eulerAngles;
            return self;
        }
        public static T SetEulerAnglesY<T>(this T self, float y) where T : Component
        {
            Vector3 eulerAngles = self.transform.eulerAngles;
            eulerAngles.y = y;
            self.transform.eulerAngles = eulerAngles;
            return self;
        }
        public static T SetEulerAnglesZ<T>(this T self, float z) where T : Component
        {
            Vector3 eulerAngles = self.transform.eulerAngles;
            eulerAngles.z = z;
            self.transform.eulerAngles = eulerAngles;
            return self;
        }
        public static T EulerAnglesIdentity<T>(this T self) where T : Component
        {
            self.transform.eulerAngles = Vector3.zero;
            return self;
        }
        public static T SetLocalPosition<T>(this T self, Vector3 localPos) where T : Component
        {
            self.transform.localPosition = localPos;
            return self;
        }
        public static T SetLocalPosition<T>(this T self, float x, float y, float z) where T : Component
        {
            Vector3 localPos = self.transform.localPosition;
            localPos.x = x;
            localPos.y = y;
            localPos.z = z;
            self.transform.localPosition = localPos;
            return self;
        }
        public static T SetLocalPositionX<T>(this T self, float x) where T : Component
        {
            Vector3 localPos = self.transform.localPosition;
            localPos.x = x;
            self.transform.localPosition = localPos;
            return self;
        }
        public static T SetLocalPositionY<T>(this T self, float y) where T : Component
        {
            Vector3 localPos = self.transform.localPosition;
            localPos.y = y;
            self.transform.localPosition = localPos;
            return self;
        }
        public static T SetLocalPositionZ<T>(this T self, float z) where T : Component
        {
            Vector3 localPos = self.transform.localPosition;
            localPos.z = z;
            self.transform.localPosition = localPos;
            return self;
        }
        public static T LocalPositionIdentity<T>(this T self) where T : Component
        {
            self.transform.localPosition = Vector3.zero;
            return self;
        }
        public static T LocalRotationIdentity<T>(this T self) where T : Component
        {
            self.transform.localRotation = Quaternion.identity;
            return self;
        }
        public static T SetLocalEulerAngles<T>(this T self, Vector3 localEulerAngles) where T : Component
        {
            self.transform.localEulerAngles = localEulerAngles;
            return self;
        }
        public static T SetLocalEulerAngles<T>(this T self, float x, float y, float z) where T : Component
        {
            Vector3 localEulerAngles = self.transform.localEulerAngles;
            localEulerAngles.x = x;
            localEulerAngles.y = y;
            localEulerAngles.z = z;
            self.transform.localEulerAngles = localEulerAngles;
            return self;
        }
        public static T SetLocalEulerAnglesX<T>(this T self, float x) where T : Component
        {
            Vector3 localEulerAngles = self.transform.localEulerAngles;
            localEulerAngles.x = x;
            self.transform.localEulerAngles = localEulerAngles;
            return self;
        }
        public static T SetLocalEulerAnglesY<T>(this T self, float y) where T : Component
        {
            Vector3 localEulerAngles = self.transform.localEulerAngles;
            localEulerAngles.y = y;
            self.transform.localEulerAngles = localEulerAngles;
            return self;
        }
        public static T SetLocalEulerAnglesZ<T>(this T self, float z) where T : Component
        {
            Vector3 localEulerAngles = self.transform.localEulerAngles;
            localEulerAngles.z = z;
            self.transform.localEulerAngles = localEulerAngles;
            return self;
        }
        public static T LocalEulerAnglesIdentity<T>(this T self) where T : Component
        {
            self.transform.localEulerAngles = Vector3.zero;
            return self;
        }
        public static T SetLocalScale<T>(this T self, Vector3 localScale) where T : Component
        {
            self.transform.localScale = localScale;
            return self;
        }
        public static T SetLocalScale<T>(this T self, float x, float y, float z) where T : Component
        {
            Vector3 localScale = self.transform.localScale;
            localScale.x = x;
            localScale.y = y;
            localScale.z = z;
            self.transform.localScale = localScale;
            return self;
        }
        public static T SetLocalScaleX<T>(this T self, float x) where T : Component
        {
            Vector3 localScale = self.transform.localScale;
            localScale.x = x;
            self.transform.localScale = localScale;
            return self;
        }
        public static T SetLocalScaleY<T>(this T self, float y) where T : Component
        {
            Vector3 localScale = self.transform.localScale;
            localScale.y = y;
            self.transform.localScale = localScale;
            return self;
        }
        public static T SetLocalScaleZ<T>(this T self, float z) where T : Component
        {
            Vector3 localScale = self.transform.localScale;
            localScale.z = z;
            self.transform.localScale = localScale;
            return self;
        }
        public static T LocalScaleIdentity<T>(this T self) where T : Component
        {
            self.transform.localScale = Vector3.one;
            return self;
        }
        public static T Identity<T>(this T self) where T : Component
        {
            self.transform.position = Vector3.zero;
            self.transform.rotation = Quaternion.Euler(Vector3.zero);
            self.transform.localScale = Vector3.one;
            return self;
        }
        public static T LocalIdentity<T>(this T self) where T : Component
        {
            self.transform.localPosition = Vector3.zero;
            self.transform.localRotation = Quaternion.Euler(Vector3.zero);
            self.transform.localScale = Vector3.one;
            return self;
        }
        public static T SetParent<T>(this T self, Component parent, bool worldPositionStays = true) where T : Component
        {
            self.transform.SetParent(parent.transform, worldPositionStays);
            return self;
        }
        public static T SetAsRootTransform<T>(this T self) where T : Component
        {
            self.transform.SetParent(null);
            return self;
        }
        public static T DetachChildren<T>(this T self) where T : Component
        {
            self.transform.DetachChildren();
            return self;
        }
        public static T LookAt<T>(this T self, Vector3 worldPosition) where T : Component
        {
            self.transform.LookAt(worldPosition);
            return self;
        }
        public static T LookAt<T>(this T self, Vector3 worldPosition, Vector3 worldUp) where T : Component
        {
            self.transform.LookAt(worldPosition, worldUp);
            return self;
        }
        public static T LookAt<T>(this T self, Transform target) where T : Component
        {
            self.transform.LookAt(target);
            return self;
        }
        public static T LookAt<T>(this T self, Transform target, Vector3 worldUp) where T : Component
        {
            self.transform.LookAt(target, worldUp);
            return self;
        }
        public static T Rotate<T>(this T self, Vector3 eulers) where T : Component
        {
            self.transform.Rotate(eulers);
            return self;
        }
        public static T Rotate<T>(this T self, Vector3 eulers, Space relativeTo) where T : Component
        {
            self.transform.Rotate(eulers, relativeTo);
            return self;
        }
        public static T Rotate<T>(this T self, Vector3 axis, float angle) where T : Component
        {
            self.transform.Rotate(axis, angle);
            return self;
        }
        public static T Rotate<T>(this T self, Vector3 axis, float angle, Space relativeTo) where T : Component
        {
            self.transform.Rotate(axis, angle, relativeTo);
            return self;
        }
        public static T Rotate<T>(this T self, float xAngle, float yAngle, float zAngle) where T : Component
        {
            self.transform.Rotate(xAngle, yAngle, zAngle);
            return self;
        }
        public static T Rotate<T>(this T self, float xAngle, float yAngle, float zAngle, Space relativeTo) where T : Component
        {
            self.transform.Rotate(xAngle, yAngle, zAngle, relativeTo);
            return self;
        }
        public static T CopyTransformValues<T>(this T self, Component target) where T : Component
        {
            self.transform.position = target.transform.position;
            self.transform.rotation = target.transform.rotation;
            self.transform.localScale = target.transform.localScale;
            return self;
        }
        public static T GetFullName<T>(this T self, out string fullName) where T : Component
        {
            List<Transform> tfs = new List<Transform>();
            Transform tf = self.transform;
            tfs.Add(tf);
            while (tf.parent)
            {
                tf = tf.parent;
                tfs.Add(tf);
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(tfs[tfs.Count - 1].name);
            for (int i = tfs.Count - 2; i >= 0; i--)
            {
                sb.Append("/" + tfs[i].name);
            }
            fullName = sb.ToString();
            return self;
        }
        public static T GetComponentOnChild<T>(this Transform self, int childIndex) where T : Component
        {
            if (childIndex > self.childCount - 1) return null;
            return self.GetChild(childIndex).GetComponent<T>();
        }
    }
}