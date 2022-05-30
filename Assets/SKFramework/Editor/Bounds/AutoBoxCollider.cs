using UnityEditor;
using UnityEngine;

namespace SK.Framework
{
    public class AutoBoxCollider
    {
        /* 
         * 如果物体不包含Collider碰撞器，为其添加BoxCollider
         * 如果包含，先进行销毁，再添加BoxCollider
         * 添加完成后，自动适配BoxCollider大小，以包含子物体边界
         */
        [MenuItem("SKFramework/Bounds/Auto BoxCollider")]
        public static void Execute()
        {
            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                GameObject go = Selection.gameObjects[i];
                //先判断当前是否有碰撞器 进行销毁
                var currentCollider = go.GetComponent<Collider>();
                if (currentCollider != null) Object.DestroyImmediate(currentCollider);
                //添加BoxCollider
                var boxCollider = go.AddComponent<BoxCollider>();

                //记录当前坐标、旋转值、缩放值
                Vector3 position = go.transform.position;
                Quaternion rotation = go.transform.rotation;
                Vector3 scale = go.transform.localScale;

                //重置
                go.transform.position = Vector3.zero;
                go.transform.rotation = Quaternion.identity;

                Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
                var renders = go.GetComponentsInChildren<MeshRenderer>(true);
                for (int j = 0; j < renders.Length; j++)
                {
                    bounds.Encapsulate(renders[j].bounds);
                }
                boxCollider.center = bounds.center;
                Vector3 size = bounds.size;
                size.x /= scale.x;
                size.y /= scale.y;
                size.z /= scale.z;
                boxCollider.size = size;
                go.transform.position = position;
                go.transform.rotation = rotation;
            }
        }
        [MenuItem("SKFramework/Bounds/Auto BoxCollider", true)]
        public static bool Validate()
        {
            return Selection.gameObjects.Length > 0;
        }
    }
}