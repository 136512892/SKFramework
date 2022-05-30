using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace SK.Framework 
{
    public class RawImageContext
    {
        /// <summary>
        /// RawImage转Image
        /// </summary>
        [MenuItem("CONTEXT/RawImage/Convert 2 Image")]
        public static void RawImage2Image()
        {
            RawImage rawImage = Selection.activeGameObject.GetComponent<RawImage>();
            Sprite sprite = null;
            //如果RawImage组件中的texture不为空
            if (rawImage.texture != null)
            {
                //获取texture的资源路径
                var path = AssetDatabase.GetAssetPath(rawImage.texture);
                //根据该资源路径加载Sprite
                sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            }
            var raycastTarget = rawImage.raycastTarget;
            //销毁RawImage组件
            Object.DestroyImmediate(rawImage);
            //添加Image组件
            Image image = Selection.activeGameObject.AddComponent<Image>();
            image.sprite = sprite;
            image.raycastTarget = raycastTarget;
            //SetDirty以保存
            EditorUtility.SetDirty(Selection.activeGameObject);
        }
    }
}