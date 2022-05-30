using UnityEditor;
using UnityEngine;

namespace SK.Framework
{
    public class AutoModelCenter
    {
        /*
         * 当物体坐标点与其模型实际所在位置偏差较大时
         * 为其添加添加一个空父级，以校准中心点
         */
        [MenuItem("SKFramework/Bounds/Auto Model Center")]
        public static void Execute()
        {
            Transform transform = Selection.activeTransform;
            Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
            var mrs = transform.GetComponentsInChildren<MeshRenderer>(true);
            Vector3 center = Vector3.zero;
            for (int i = 0; i < mrs.Length; i++)
            {
                center += mrs[i].bounds.center;
                bounds.Encapsulate(mrs[i].bounds);
            }
            center /= mrs.Length;
            GameObject obj = new GameObject(transform.name);
            obj.transform.position = center;
            obj.transform.rotation = Quaternion.identity;
            obj.transform.SetParent(transform.parent);
            transform.SetParent(obj.transform);
        }
        [MenuItem("SKFramework/Bounds/Auto Model Center", true)]
        public static bool Validate()
        {
            return Selection.activeTransform != null;
        }
    }
}